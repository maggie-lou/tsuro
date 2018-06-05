//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Collections.Generic;
//using System.Text;

//namespace tsuro
//{
//	public class NetworkAdminLauncher
//	{      
//		static void Main(String[] args)
//		{
//			IPlayer ourPlayer = new MostSymmetricPlayer("Jerry");

//			NetworkAdmin networkPlayer = new NetworkAdmin(ourPlayer);
            
//			string ipServer = Dns.GetHostEntry("localhost").AddressList[0].ToString();
//            int port = 12345;
//			byte[] dataBuffer = new byte[1024];

//			string myIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
//			Console.WriteLine(myIP);

//			// Connect to server
//			try
//			{
//				// Establish remote endpoint for socket

//				TcpClient client = new TcpClient();
//				client.ReceiveBufferSize = 5096;
//				client.Connect("localhost", port);


//    			while (true)
//    			{
//					//Console.WriteLine("beginning of loop");


//					NetworkStream stream = client.GetStream();
//					// Get response
//					dataBuffer = new byte[1024];

//					StringBuilder responseData = new StringBuilder();
//					int numBytesRead = 0;

//                    // Process data in buffer - query is until we see a new line,
//                    // if we don't see a new line, we need to read more
//					do
//					{
//						numBytesRead = stream.Read(dataBuffer, 0, dataBuffer.Length);
//						responseData.Append(Encoding.ASCII.GetString(dataBuffer, 0, numBytesRead));
//					} while (stream.DataAvailable);
                    
//					//Int32 numBytesData = stream.Read(dataBuffer, 0, dataBuffer.Length);
//					//responseData = System.Text.Encoding.ASCII.GetString(dataBuffer, 0, numBytesData);
//					Console.WriteLine(responseData);

//					dataBuffer = new byte[1024];
//					String responseToServer = networkPlayer.interpretQuery(responseData.ToString()) + "\n";
//					Console.WriteLine(responseToServer);

//					dataBuffer = System.Text.Encoding.ASCII.GetBytes(responseToServer);
//				    stream.Write(dataBuffer, 0, dataBuffer.Length);
//					stream.Flush();

//    				//stream.Close();
//    				//client.Close();
//			    }


//			}
//			catch (Exception e)
//			{
//				Console.WriteLine(e.ToString());
//			}
//		}
//	}
//}
