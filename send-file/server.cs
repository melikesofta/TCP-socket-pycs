using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Net;
using System.Text;
	
class Program{		
	static void Main(string[] args)
	{
	    CancellationTokenSource cts = new CancellationTokenSource();
	    TcpListener listener = new TcpListener(IPAddress.Any, 5000);
	    try
	    {
	        listener.Start();
	        //just fire and forget. We break from the "forgotten" async loops
	        //in AcceptClientsAsync using a CancellationToken from `cts`
	        AcceptClientsAsync(listener, cts.Token);
	        Thread.Sleep(60000); //block here to hold open the server
	    }
	    finally
	    {
	        cts.Cancel();
	        listener.Stop();
	    }
	}
	async static Task AcceptClientsAsync(TcpListener listener, CancellationToken ct)
	{
	    var clientCounter = 0;
	    while (!ct.IsCancellationRequested)
	    {
	        TcpClient client = await listener.AcceptTcpClientAsync()
	                                            .ConfigureAwait(false);
	        clientCounter++;
	        //once again, just fire and forget, and use the CancellationToken
	        //to signal to the "forgotten" async invocation.
	        EchoAsync(client, clientCounter, ct);
	    }

	}
	async static Task EchoAsync(TcpClient client,
	                     int clientIndex,
	                     CancellationToken ct)
	{
	    Console.WriteLine("New client ({0}) connected", clientIndex);
	    using (client)
	    {
	        var buf = new byte[4096];
	        var stream = client.GetStream();
	        while (!ct.IsCancellationRequested)
	        {
	           	//Array.Clear(buf, 0, buf.Length);
	            //under some circumstances, it's not possible to detect
	            //a client disconnecting if there's no data being sent
	            //so it's a good idea to give them a timeout to ensure that 
	            //we clean them up.
	            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(180));
	            var amountReadTask = stream.ReadAsync(buf, 0, buf.Length, ct);
	            string dataReceived = Encoding.ASCII.GetString(buf, 0, buf.Length);
                string receivedDate = DateTime.Now.ToString();
                byte[] dateBytes = ASCIIEncoding.ASCII.GetBytes(receivedDate);
                if(buf[0]!=0){
						using (var _FileStream = new System.IO.FileStream("../json/new.json", System.IO.FileMode.Create, System.IO.FileAccess.Write)){
								_FileStream.Write(buf, 0, buf.Length);
						}
	            	//Console.WriteLine("Received from " + clientIndex + " :    " + dataReceived);
	            }
	            var completedTask = await Task.WhenAny(timeoutTask, amountReadTask)
	                                          .ConfigureAwait(false);
	            if (completedTask == timeoutTask)
	            {
	                var msg = Encoding.ASCII.GetBytes("Client timed out");
	                await stream.WriteAsync(msg, 0, msg.Length);
	                break;
	            }
	            //now we know that the amountTask is complete so
	            //we can ask for its Result without blocking
	            var amountRead = amountReadTask.Result;
	            if (amountRead == 0) break; //end of stream.
	            await stream.WriteAsync(dateBytes, 0, dateBytes.Length, ct)
	                        .ConfigureAwait(false);
	        }
	    }
	    Console.WriteLine("Client ({0}) disconnected", clientIndex);
	}
}
