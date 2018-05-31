using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tsuro;
using System.Diagnostics;

namespace TsuroTests
{
   [TestClass]
    public class Tournament
    {
        [TestMethod]
        [DeploymentItem("drawPilepaths.txt")]
        public void RunATournament()
        {
			int numOfTournaments = 10;
            int randomWins = 0;
            int lstSymWins = 0;
            int mostSymWins = 0;
			for (int i = 0; i < numOfTournaments; i++)
			{
				Admin a = new Admin();
				List<Tile> drawPile = a.initializeDrawPile("drawPilepaths.txt");
				Board b = new Board();
				b.drawPile = drawPile;

				List<string> allPlayers = new List<string>() { "blue", "hotpink", "green" };

				SPlayer randomPlayer = new SPlayer(allPlayers[0], new List<Tile>(), new RandomPlayer());
				randomPlayer.initialize(b);
				randomPlayer.placePawn(b);
				SPlayer leastSymPlayer = new SPlayer(allPlayers[1], new List<Tile>(), new LeastSymmetricPlayer());
				leastSymPlayer.initialize(b);
				leastSymPlayer.placePawn(b);
				SPlayer mostSymPlayer = new SPlayer(allPlayers[2], new List<Tile>(), new MostSymmetricPlayer());
				mostSymPlayer.initialize(b);
				mostSymPlayer.placePawn(b);

				a.dealTiles(b);
				List<SPlayer> winners = null;

				foreach (SPlayer inGamePlayer in b.returnOnBoard())
				{
					Debug.WriteLine(inGamePlayer.returnColor() + " is at:   " + 
					                string.Format("{0} {1} {2}", inGamePlayer.getPlayerPosn().returnRow(), 
					                              inGamePlayer.getPlayerPosn().returnCol(),
					                              inGamePlayer.getPlayerPosn().returnLocationOnTile()));
				}

				SPlayer currentPlayer = b.returnOnBoard()[0];
				Tile playTile = currentPlayer.playTurn(b, drawPile.Count);
				Debug.WriteLine(currentPlayer.returnColor() + " Turn");
				Debug.WriteLine("Playing Tile:");
				Debug.WriteLine(playTile.paths[0].loc1 + " " + playTile.paths[0].loc2);
				Debug.WriteLine(playTile.paths[1].loc1 + " " + playTile.paths[1].loc2);
				Debug.WriteLine(playTile.paths[2].loc1 + " " + playTile.paths[2].loc2);
				Debug.WriteLine(playTile.paths[3].loc1 + " " + playTile.paths[3].loc2);
				TurnResult tr = a.playATurn(drawPile, b.returnOnBoard(), b.returnEliminated(), b, playTile);
				//Debug.WriteLine("Eliminated: ");
				foreach (SPlayer p1 in tr.eliminatedPlayers)
				{
					//Debug.WriteLine(p1.returnColor());
				}
				while (winners == null)
				{
					foreach (SPlayer inGamePlayer in b.returnOnBoard())
					{
						Debug.WriteLine(inGamePlayer.returnColor() + " is at " +
						                string.Format("{0} {1} {2}", inGamePlayer.getPlayerPosn().returnRow(),
						                                               inGamePlayer.getPlayerPosn().returnCol(),
						                                               inGamePlayer.getPlayerPosn().returnLocationOnTile()));
                    }
                    SPlayer p = b.returnOnBoard()[0];
                    playTile = p.playTurn(b, drawPile.Count);
                    Debug.WriteLine(p.returnColor() + " Turn");
                    Debug.WriteLine("Playing Tile:");
                    Debug.WriteLine(playTile.paths[0].loc1 + " " + playTile.paths[0].loc2);
                    Debug.WriteLine(playTile.paths[1].loc1 + " " + playTile.paths[1].loc2);
                    Debug.WriteLine(playTile.paths[2].loc1 + " " + playTile.paths[2].loc2);
                    Debug.WriteLine(playTile.paths[3].loc1 + " " + playTile.paths[3].loc2);

                    tr = a.playATurn(tr.drawPile, tr.currentPlayers, tr.eliminatedPlayers, tr.b, playTile);
                    Debug.WriteLine("Eliminated: ");
                    foreach (SPlayer p1 in tr.eliminatedPlayers)
                    {
                        Debug.WriteLine(p1.returnColor());
                    }
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
                    Debug.WriteLine(p.returnColor() + " has won!");
                }
            }
			Console.WriteLine("Random Player Wins: " + randomWins + "/" + numOfTournaments);
			Console.WriteLine("Least Symmetric Player Wins: " + lstSymWins + "/" + numOfTournaments);
			Console.WriteLine("Most Symmetric Player Wins: " + mostSymWins + "/" + numOfTournaments);

    
        }
    }
}
