//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Collections.Generic;
//using System.IO;

//namespace tsuro
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
			
//			NetworkAdmin player = new NetworkAdmin(new MostSymmetricPlayer());
			
//			try {
//				Int32 port = 80;
//				IPHostEntry localHostInfo = Dns.GetHostEntry(Dns.GetHostName());
//                IPAddress localIP = localHostInfo.AddressList[0];

//				TcpListener server = new TcpListener(localIP, port);
//				server.Start();

//				Byte[] bytes = new byte[1024];
//				String data = null;

//				while (true) {
//					TcpClient client = server.AcceptTcpClient();
//					data = null;
//					NetworkStream stream = client.GetStream();

//					int i;
//					while((i=stream.Read(bytes, 0, bytes.Length)) != 0) {
//						data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
//						Console.WriteLine(String.Format("Received: {0}", data));
//						string response = player.interpretQuery(data);

//						byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
//						stream.Write(msg, 0, msg.Length);
//					}
//					client.Close();
//				}
//			} catch(SocketException e) {
//				Console.WriteLine(e.ToString());
//			}         
//       }


//    }
//}
