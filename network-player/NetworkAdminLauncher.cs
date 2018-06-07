using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace tsuro
{
	public class NetworkAdminLauncher
	{
		static void Main(String[] args)
		{
			IPlayer ourPlayer = new MostSymmetricPlayer("Jerry");

			NetworkAdmin networkPlayer = new NetworkAdmin(ourPlayer);

			string ipServer = Dns.GetHostEntry("localhost").AddressList[0].ToString();
			int port = 12345;
			byte[] dataBuffer = new byte[1024];

			string myIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
			//Console.WriteLine(myIP);

			// Connect to server
			try
			{
				// Establish remote endpoint for socket            
				TcpClient client = new TcpClient();
				client.Connect("localhost", port);

				while (true)
				{

					NetworkStream stream = client.GetStream();
					var reader = new StreamReader(stream);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
						String responseToServer = networkPlayer.interpretQuery(line) + "\n";
                        //Console.WriteLine(responseToServer);

                        byte[] dataBufferOut = System.Text.Encoding.ASCII.GetBytes(responseToServer);
                        stream.Write(dataBufferOut, 0, dataBufferOut.Length);
                        stream.Flush();
                    }
					stream.Close();
                    client.Close();
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}
	}
}
