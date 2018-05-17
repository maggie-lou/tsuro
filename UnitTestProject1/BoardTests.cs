using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tsuro;

namespace TsuroTests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void PlayerGetsEliminated()
        {
            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            SPlayer p2 = new SPlayer("red", new List<Tile>(), true);

            Board b = new Board();
            b.registerPlayer(p1);
            p1.playerState = SPlayer.State.Playing;
            b.registerPlayer(p2);

            b.eliminatePlayer(p1);
            Assert.IsFalse(b.returnOnBoard().Contains(p1));
            Assert.IsTrue(b.returnEliminated().Contains(p1));
        }

        [TestMethod]
        public void PlaceTileFirstTurnLeadsToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), false);
            Board b = new Board();

            p1.setPosn(new Posn(-1, 0, 4));
            Assert.IsFalse(b.checkPlaceTile(p1, t1));
        }

        [TestMethod]
        public void PlaceTileLeadsToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            Board b = new Board();

            p1.setPosn(new Posn(1, 0, 0));
            Assert.IsFalse(b.checkPlaceTile(p1, t1));
        }

        [TestMethod]
        public void CanPlaceTileLeadsToEmptySpace()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            Board b = new Board();

            p1.setPosn(new Posn(0, 0, 3));
            Assert.IsTrue(b.checkPlaceTile(p1, t1));
        }
        [TestMethod]
        public void PlaceTileInTheMiddleOfBoardLeadsPlayerToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 2, 1, 5, 3, 7, 4, 6);
            Tile t2 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            Board b = new Board();

            p1.setPosn(new Posn(0, 1, 3));
            b.grid[0, 1] = t1;
            Assert.IsFalse(b.checkPlaceTile(p1, t2));
        }

        [TestMethod]
        public void PlaceTilePlacesTile()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            Board b = new Board();

            p1.setPosn(new Posn(0, 0, 3));

            SPlayer pcheck = b.placeTile(p1, t1);
            Posn playerPosn = pcheck.getPlayerPosn();
            Assert.IsTrue(playerPosn.returnCol() == 1);
            Assert.IsTrue(playerPosn.returnRow() == 0);
            Assert.IsTrue(playerPosn.returnLocationOnTile() == 3);
            Assert.IsTrue(b.occupied(0, 1));
        }

        [TestMethod]
        public void EmptyBoardNotOccupied()
        {
            Board b = new Board();

            Assert.IsFalse(b.occupied(3, 3));
        }

        [TestMethod]
        public void LocationOnBoardIsOccupied()
        {
            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            Board b = new Board();
            b.registerPlayer(p1);

			Posn newPosn = new Posn(0, 0, 3);
            p1.setPosn(newPosn);

            Assert.IsTrue(b.locationOccupied(newPosn));
        }

        [TestMethod]
        public void LocationOnBoardIsNotOccupied()
        {
            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            Board b = new Board();
            b.registerPlayer(p1);
            
			Posn newPosn = new Posn(0, 0, 3);

            p1.setPosn(newPosn);

            Assert.IsFalse(b.locationOccupied(new Posn(0,0,0)));
        }
    }
}
