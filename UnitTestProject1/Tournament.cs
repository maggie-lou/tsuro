using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tsuro;

namespace TsuroTests
{
   [TestClass]
    public class Tournament
    {
        [TestMethod]
        [DeploymentItem("drawPilepaths.txt")]
        public void RunATournament()
        {
            int randomWins = 0;
            int lstSymWins = 0;
            int mostSymWins = 0;
            for (int i = 0; i < 20; i++)
            {
                Admin a = new Admin();
                List<Tile> drawPile = a.initializeDrawPile("drawPilepaths.txt");
                Board b = new Board();
                b.drawPile = drawPile;

                List<string> allPlayers = new List<string>() { "blue", "hotpink", "green" };

                SPlayer randomPlayer = new SPlayer(allPlayers[0], new List<Tile>(), false, "Random", allPlayers);
                randomPlayer.placePawn(b);
                SPlayer leastSymPlayer = new SPlayer(allPlayers[1], new List<Tile>(), false, "LeastSymmetric", allPlayers);
                leastSymPlayer.placePawn(b);
                SPlayer mostSymPlayer = new SPlayer(allPlayers[2], new List<Tile>(), false, "MostSymmetric", allPlayers);
                mostSymPlayer.placePawn(b);

                a.dealTiles(b);
                List<SPlayer> winners = null;

                foreach (SPlayer inGamePlayer in b.returnOnBoard())
                {
                    //Console.WriteLine(inGamePlayer.returnColor() + " is playing");
                    //Console.WriteLine("location: " +
                    //inGamePlayer.getboardLocationRow() + inGamePlayer.getboardLocationCol() +
                    //inGamePlayer.getLocationOnTile());

                }

                SPlayer currentPlayer = b.returnOnBoard()[0];
                Tile playTile = currentPlayer.playTurn(b, drawPile.Count);
                //Console.WriteLine(currentPlayer.returnColor() + " Turn");
                //Console.WriteLine("Playing Tile:");
                //Console.WriteLine(playTile.paths[0].loc1 + " " + playTile.paths[0].loc2);
                //Console.WriteLine(playTile.paths[1].loc1 + " " + playTile.paths[1].loc2);
                //Console.WriteLine(playTile.paths[2].loc1 + " " + playTile.paths[2].loc2);
                //Console.WriteLine(playTile.paths[3].loc1 + " " + playTile.paths[3].loc2);
                TurnResult tr = a.playATurn(drawPile, b.returnOnBoard(), b.returnEliminated(), b, playTile);
                //Console.WriteLine("Eliminated: ");
                foreach (SPlayer p1 in tr.eliminatedPlayers)
                {
                    //Console.WriteLine(p1.returnColor());
                }
                while (winners == null)
                {
                    foreach (SPlayer inGamePlayer in b.returnOnBoard())
                    {
                        //Console.WriteLine(inGamePlayer.returnColor() + " is playing");
                        //Console.WriteLine("location: " +
                        //inGamePlayer.getboardLocationRow() + inGamePlayer.getboardLocationCol() +
                        //inGamePlayer.getLocationOnTile());

                    }
                    SPlayer p = b.returnOnBoard()[0];
                    playTile = p.playTurn(b, drawPile.Count);
                    //Console.WriteLine(p.returnColor() + " Turn");
                    //Console.WriteLine("Playing Tile:");
                    //Console.WriteLine(playTile.paths[0].loc1 + " " + playTile.paths[0].loc2);
                    //Console.WriteLine(playTile.paths[1].loc1 + " " + playTile.paths[1].loc2);
                    //Console.WriteLine(playTile.paths[2].loc1 + " " + playTile.paths[2].loc2);
                    //Console.WriteLine(playTile.paths[3].loc1 + " " + playTile.paths[3].loc2);

                    tr = a.playATurn(tr.drawPile, tr.currentPlayers, tr.eliminatedPlayers, tr.b, playTile);
                    //Console.WriteLine("Eliminated: ");
                    foreach (SPlayer p1 in tr.eliminatedPlayers)
                    {
                        //Console.WriteLine(p1.returnColor());
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
                    //Console.WriteLine(p.returnColor() + " has won!");
                }
            }
            Console.WriteLine("Random Player Wins: " + randomWins + "/20");
            Console.WriteLine("Least Symmetric Player Wins: " + lstSymWins + "/20");
            Console.WriteLine("Most Symmetric Player Wins: " + mostSymWins + "/20");

    
        }
    }
}
