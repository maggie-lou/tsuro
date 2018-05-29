using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tsuro
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
			StartListening();
        }


		public static string incomingData = null;
		public static void StartListening()
        {
            byte[] incomingBytes = new byte[1024];

            // Establish local endpoint of socket
            IPHostEntry localHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIP = localHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(localIP, 1234);

            // Create TCP socket
            Socket socket = new Socket(localIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind socket to endpoint and listen for incoming connections
            try
            {
                socket.Bind(localEndPoint);
                socket.Listen(1);

                while (true)
                {
                    Console.WriteLine("New connection");
                    Socket handler = socket.Accept();

                    // Clearing old data
                    incomingData = null;

                    while (true)
                    {
                        Console.WriteLine("Processing data");
                        // Process data from connection
                        int bytesReceived = handler.Receive(incomingBytes);
                        incomingData += Encoding.ASCII.GetString(incomingBytes, 0, bytesReceived);
                        if (incomingData.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    Console.WriteLine("Text received : {0}", incomingData);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
