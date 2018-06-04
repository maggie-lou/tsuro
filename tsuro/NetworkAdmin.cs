using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace tsuro
{
	public class NetworkAdminLauncher
	{      
		static void Main(String[] args)
		{
			IPlayer ourPlayer = new MostSymmetricPlayer();
			ourPlayer.initialize("blue", new List<string>());
			NetworkAdmin networkPlayer = new NetworkAdmin(ourPlayer);

			byte[] dataBuffer = new byte[1024];

			// Connect to server
			try
			{
				// Establish remote endpoint for socket
				string ipServer = Dns.GetHostEntry("localhost").AddressList[0].ToString();
				int port = 12345;

				TcpClient client = new TcpClient(ipServer, port);


				while (true)
				{
					NetworkStream stream = client.GetStream();
					// Get response
					dataBuffer = new byte[1024];
					String responseData = String.Empty;

					Int32 numBytesData = stream.Read(dataBuffer, 0, dataBuffer.Length);
					responseData = System.Text.Encoding.ASCII.GetString(dataBuffer, 0, numBytesData);
					Console.WriteLine(responseData);


					String responseToServer = networkPlayer.interpretQuery(responseData);
					Console.WriteLine(responseToServer);

					dataBuffer = System.Text.Encoding.ASCII.GetBytes(responseToServer);
					stream.Write(dataBuffer, 0, dataBuffer.Length);
					Console.WriteLine("After write to stream");
					//stream.Close();
				}

				client.Close();

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}
	}
}
