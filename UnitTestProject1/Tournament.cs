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

            RandomPlayer rp1 = new RandomPlayer();
            rp1.initialize("blue", new List<string>() { "blue", "hotpink"});
            rp1.placePawn(b);

            RandomPlayer rp2 = new RandomPlayer();
            rp2.initialize("hotpink", new List<string>() { "blue", "hotpink" });
            rp2.placePawn(b);

            a.dealTiles(b);

            TurnResult tr = a.playATurn(drawPile, b.returnOnBoard(),
                b.returnEliminated(), b,
                rp1.playTurn(b, rp1.currPlayer.returnHand(), drawPile.Count));

            Assert.AreEqual(tr.currentPlayers[0].getboardLocationRow(), 0);
            Assert.AreEqual(tr.currentPlayers[0].getboardLocationCol(), 0);
            Assert.AreNotEqual(tr.currentPlayers[0].getLocationOnTile(), 0);

        }


    }
}
