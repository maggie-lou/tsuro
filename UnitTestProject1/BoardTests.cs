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
        static Path first = new Path(0, 1);
        static Path second = new Path(2, 4);
        static Path third = new Path(3, 6);
        static Path fourth = new Path(5, 7);

        static List<Path> paths = new List<Path>()
            {
                first,
                second,
                third,
                fourth
            };

        Tile t1 = new Tile(paths);

        [TestMethod]
        public void PlayerGetsEliminated()
        {
            SPlayer p1 = new SPlayer("blue", null);
            SPlayer p2 = new SPlayer("red", null);

            Board b = new Board();
            b.registerPlayer(p1);
            b.registerPlayer(p2);

            b.eliminatePlayer(p1);
            Assert.IsFalse(b.returnOnBoard().Contains(p1));
            Assert.IsTrue(b.returnEliminated().Contains(p1));
        }

        [TestMethod]
        public void CannotPlaceTileTurnLeadsToEdge()
        {

            SPlayer p1 = new SPlayer("blue", null);
            Board b = new Board();

            p1.setPosn(0, 1, 6);
            Assert.IsFalse(b.checkPlaceTile(p1, t1));
        }

        [TestMethod]
        public void CanPlaceTileLeadsToEmptySpace()
        {

            SPlayer p1 = new SPlayer("blue", null);
            Board b = new Board();

            p1.setPosn(0, 0, 3);
            Assert.IsTrue(b.checkPlaceTile(p1, t1));
        }

        [TestMethod]
        public void PlaceTilePlacesTile()
        {
            SPlayer p1 = new SPlayer("blue", null);
            Board b = new Board();

            p1.setPosn(0, 0, 3);

            SPlayer pcheck = b.placeTile(p1, t1);

            Assert.IsTrue(pcheck.getboardLocationCol() == 1);
            Assert.IsTrue(pcheck.getboardLocationRow() == 0);
            Assert.IsTrue(pcheck.getLocationOnTile() == 3);
        }
    }
}
