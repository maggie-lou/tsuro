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
			IPlayer ourPlayer = new MostSymmetricPlayer("Jerry");

			NetworkAdmin networkPlayer = new NetworkAdmin(ourPlayer);
            
			string ipServer = Dns.GetHostEntry("localhost").AddressList[0].ToString();
            int port = 12345;
			byte[] dataBuffer = new byte[1024];

			// Connect to server
			try
			{
				// Establish remote endpoint for socket

				TcpClient client = new TcpClient();
				client.Connect("localhost", port);


    			while (true)
    			{
					//Console.WriteLine("beginning of loop");


					NetworkStream stream = client.GetStream();
					// Get response
					dataBuffer = new byte[1024];
					String responseData = String.Empty;
                    
					Int32 numBytesData = stream.Read(dataBuffer, 0, dataBuffer.Length);
					responseData = System.Text.Encoding.ASCII.GetString(dataBuffer, 0, numBytesData);
					Console.WriteLine(responseData);


					String responseToServer = networkPlayer.interpretQuery(responseData) + "\n";
					Console.WriteLine(responseToServer);

					dataBuffer = System.Text.Encoding.ASCII.GetBytes(responseToServer);
				    stream.Write(dataBuffer, 0, dataBuffer.Length);
					stream.Flush();

    				//stream.Close();
    				//client.Close();
			    }


			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}
	}
}
