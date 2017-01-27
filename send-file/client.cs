using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;    
class Program
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        static string textToSend;
        static byte[] bytesToSend;
        static byte[] bytesToRead;
        static int bytesRead;
        static void Main(string[] args)
        {
            //---create a TCPClient object at the IP and port no.---
            TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
			string pathToFile = Directory.GetCurrentDirectory() + "/../json/simple.json";
			byte[] fileName = Encoding.UTF8.GetBytes(pathToFile);
			byte[] fileData = File.ReadAllBytes(pathToFile);
			//Console.WriteLine("Contents of the file: " + Encoding.ASCII.GetString(fileData, 0, fileData.Length));
			Console.WriteLine("Sending file " + pathToFile + " of length " + fileData.Length);
            NetworkStream nwStream = client.GetStream();
			nwStream.Write(fileData, 0, fileData.Length);
			bytesToRead = new byte[client.ReceiveBufferSize];
			bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
			Console.WriteLine("Server's message: " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
			//while(true){
                //---data to send to the server---
                //Console.WriteLine("Write message to send: ");
                //textToSend = Console.ReadLine();
                //NetworkStream nwStream = client.GetStream();
                //bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                //Console.WriteLine("Sending : " + textToSend);
                //nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                //bytesToRead = new byte[client.ReceiveBufferSize];
                //bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                //Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            //}
            client.Close();
        }
}