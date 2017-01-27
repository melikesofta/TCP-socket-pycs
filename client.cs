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

            while(true){
                //---data to send to the server---
                Console.WriteLine("Write message to send: ");
                textToSend = Console.ReadLine();

                NetworkStream nwStream = client.GetStream();
                bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                //---send the text---
                Console.WriteLine("Sending : " + textToSend);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                //---read back the text---
                bytesToRead = new byte[client.ReceiveBufferSize];
                bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            }
            client.Close();
        }
}
