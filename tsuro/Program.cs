using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace tsuro
{
    class Program
    {
        static void Main(string[] args)
        {
			// run a tournament
			// create a server socket

            // Establish local endpoint of socket
            IPHostEntry localHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIP = localHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(localIP, 1234);

            // Create TCP socket
            Socket socket = new Socket(localIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            // get a player to accept on the socket
			Socket handler = getNewPlayer(socket, localEndPoint);
            
			var drawPileTxt = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "/drawPilepaths.txt");

                int numOfTournaments = 1;
                int randomWins = 0;
                int lstSymWins = 0;
                int mostSymWins = 0;
                for (int i = 0; i < numOfTournaments; i++)
                {
                    Admin a = new Admin();
                    List<Tile> drawPile = a.initializeDrawPile(drawPileTxt);
                    Board b = new Board();
                    b.drawPile = drawPile;

                    List<string> allPlayers = new List<string>() { "blue", "hotpink", "green" };

                    SPlayer randomPlayer = new SPlayer(allPlayers[0], new List<Tile>(), "Random");
                    randomPlayer.initialize(b);
                    randomPlayer.placePawn(b);
                    SPlayer leastSymPlayer = new SPlayer(allPlayers[1], new List<Tile>(), "LeastSymmetric");
                    leastSymPlayer.initialize(b);
                    leastSymPlayer.placePawn(b);
                    SPlayer networkPlayer = new SPlayer(allPlayers[2], new List<Tile>(), "Network", handler);
                    networkPlayer.initialize(b);
                    networkPlayer.placePawn(b);
                
                    a.dealTiles(b);
                    List<SPlayer> winners = null;

                    SPlayer currentPlayer = b.returnOnBoard()[0];
                    Tile playTile = currentPlayer.playTurn(b, drawPile.Count);
                    TurnResult tr = a.playATurn(drawPile, b.returnOnBoard(), b.returnEliminated(), b, playTile);
                   
                    while (winners == null)
                    {
                        
                        SPlayer p = b.returnOnBoard()[0];
                        playTile = p.playTurn(b, drawPile.Count);
                       
                        tr = a.playATurn(tr.drawPile, tr.currentPlayers, tr.eliminatedPlayers, tr.b, playTile);
                        
                        winners = tr.playResult;
                    }

                    foreach (SPlayer p in winners)
                    {
                        if (p.returnColor() == "blue")
                        {
                            randomWins++;
                        }
                        else if (p.returnColor() == "hotpink")
                        {
                            lstSymWins++;
                        }
                        else
                        {
                            mostSymWins++;
                        }
                    }
                }
                Console.WriteLine("Random Player Wins: " + randomWins + "/" + numOfTournaments);
                Console.WriteLine("Least Symmetric Player Wins: " + lstSymWins + "/" + numOfTournaments);
                Console.WriteLine("Network Player Wins: " + mostSymWins + "/" + numOfTournaments);


            }


		public static Socket getNewPlayer(Socket socket, IPEndPoint localEndPoint)
        {
			string incomingData;
			byte[] incomingBytes = new byte[1024];

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
                    //handler.Shutdown(SocketShutdown.Both);
                    //handler.Close();
					return handler;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
				return null;
            }

        }
    }
}
