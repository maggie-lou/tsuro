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
            Admin a = new Admin();
            List<Tile> drawPile = a.initializeDrawPile("drawPilepaths.txt");
            Board b = new Board();
            b.drawPile = drawPile;

            List<string> allPlayers = new List<string>() { "blue", "hotpink", "green" };

            SPlayer randomPlayer = new SPlayer(allPlayers[0], new List<Tile>(), false, "Random", allPlayers);
            randomPlayer.placePawn(b);
            SPlayer leastSymPlayer = new SPlayer(allPlayers[1], new List<Tile>(), false, "LeastSymmetric", allPlayers);
            leastSymPlayer.placePawn(b);
            SPlayer mostSymPlayer = new SPlayer(allPlayers[0], new List<Tile>(), false, "MostSymmetric", allPlayers);
            mostSymPlayer.placePawn(b);

            a.dealTiles(b);
            List<SPlayer> winners = null;

            while (winners == null)
            {
                SPlayer currentPlayer = b.returnOnBoard()[0];
                Tile playTile = currentPlayer.playTurn(b, drawPile.Count);

                Console.WriteLine(currentPlayer.returnColor() + " is playing");
                Console.WriteLine("played tile: " + playTile.paths[0].loc1 +" " + playTile.paths[0].loc2);
                Console.WriteLine(playTile.paths[1].loc1 +" "+ playTile.paths[1].loc2);
                Console.WriteLine(playTile.paths[2].loc1 +" " + playTile.paths[2].loc2);
                Console.WriteLine(playTile.paths[3].loc1 +" "+ playTile.paths[3].loc2);
                Console.WriteLine(currentPlayer.returnColor() + " is at location " +
                    currentPlayer.getboardLocationRow() + currentPlayer.getboardLocationCol() +
                    currentPlayer.getLocationOnTile());

                TurnResult tr = a.playATurn(drawPile, b.returnOnBoard(), b.returnEliminated(), b, playTile);
                winners = tr.playResult;
                
            }

            foreach (SPlayer p in winners)
            {
                Console.WriteLine(p.returnColor()+" has won!");
            }
            
        }


    }
}
