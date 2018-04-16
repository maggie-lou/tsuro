using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tsuro;

namespace TsuroTests
{
    [TestClass]
    public class AdminTests
    {
        static Path first1 = new Path(0, 1);
        static Path second1 = new Path(2, 4);
        static Path third1 = new Path(3, 6);
        static Path fourth1 = new Path(5, 7);

        static List<Path> path1 = new List<Path>()
        {
            first1,
            second1,
            third1,
            fourth1
        };

        static Tile t1 = new Tile(path1);

        static Path first2 = new Path(0, 6);
        static Path second2 = new Path(1, 5);
        static Path third2 = new Path(2, 4);
        static Path fourth2 = new Path(3, 7);

        static List<Path> path2 = new List<Path>()
        {
            first2,
            second2,
            third2,
            fourth2
        };

        static Tile t2 = new Tile(path2);

        static Path first3 = new Path(0, 5);
        static Path second3 = new Path(1, 4);
        static Path third3 = new Path(2, 7);
        static Path fourth3 = new Path(3, 6);

        static List<Path> path3 = new List<Path>()
        {
            first3,
            second3,
            third3,
            fourth3
        };

        static Tile t3 = new Tile(path3);

        [TestMethod]
        public void TileNotInEmptyHand()
        {
            Admin a = new Admin();
            Board b = new Board();
            SPlayer player1 = new SPlayer("blue", null);

            Assert.IsFalse(a.tileInHand(player1, t1));
        }

        [TestMethod]
        public void TileInHandNotRotated()
        {
            Admin a = new Admin();
            Board b = new Board();

            List<Tile> hand = new List<Tile>()
            {
                t1,
                t2,
                t3
            };

            SPlayer player1 = new SPlayer("blue", hand);
            Assert.IsTrue(a.tileInHand(player1, t1));
        }

        [TestMethod]
        public void TileInHandRotated()
        {
            Admin a = new Admin();
            Board b = new Board();

            List<Tile> hand = new List<Tile>()
            {
                t1,
                t2,
                t3
            };

            SPlayer player1 = new SPlayer("blue", hand);

            Tile t1_rotated = t1;
            t1_rotated.rotate();

            Assert.IsTrue(a.tileInHand(player1, t1_rotated));

        }

        [TestMethod]
        public void drawATileDrawsTile()
        {
            Board b = new Board();
            Admin a = new Admin();

            a.addTileToDrawPile(t1);

            Tile tcheck = a.drawATile();

            Assert.IsTrue(tcheck.isEqual(t1));
            Assert.IsNull(a.drawATile());
        }

        [TestMethod]
        public void DrawATileFromEmptyDrawpile()
        {
            Board b = new Board();
            Admin a = new Admin();

            Assert.IsNull(a.drawATile());
        }

        [TestMethod]
        public void PlayATurnWithNoPlayers()
        {
            Admin a = new Admin();
            Board b = new Board();
            List<Tile> drawpile = new List<Tile>();
            List<SPlayer> l1 = new List<SPlayer>();
            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult noturnplayed = a.playATurn(drawpile, l1, l2, b, t1);
            Assert.IsTrue(noturnplayed.playResult == null);
        }

        [TestMethod]
        public void PlayAValidTurnRemovesTileFromDrawPile()
        {
            Admin a = new Admin();
            Board b = new Board();
            List<Tile> drawpile = new List<Tile>()
            {
                t2,t3
            };

            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(0, 0, 3);
            SPlayer p2 = new SPlayer("p2", new List<Tile>());
            
            List<SPlayer> l1 = new List<SPlayer>()
            {
                p1,p2
            };

            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult tmpturn = a.playATurn(drawpile, l1, l2, b, t1);

            Assert.IsTrue(tmpturn.drawPile.Count == 1);
            Assert.IsFalse(tmpturn.drawPile.Exists(x=> x.isEqual(t2)));

            List<Tile> hand = tmpturn.currentPlayers[1].returnHand();

            Assert.IsTrue(hand.Exists(x => x.isEqual(t2)));

        }

        [TestMethod]
        public void PlayAValidTurnChangesOrderOfInGamePlayers()
        {
            Admin a = new Admin();
            Board b = new Board();
            List<Tile> drawpile = new List<Tile>()
            {
                t2,t3
            };

            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(0, 0, 3);
            SPlayer p2 = new SPlayer("p2", new List<Tile>());

            List<SPlayer> l1 = new List<SPlayer>()
            {
                p1,p2
            };

            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult tmpturn = a.playATurn(drawpile, l1, l2, b, t1);

            Assert.IsTrue(tmpturn.currentPlayers[0].returnColor() == "p2");
            Assert.IsTrue(tmpturn.currentPlayers[1].returnColor() == "p1");
            Assert.IsTrue(tmpturn.currentPlayers.Count == 2);
        }

        [TestMethod]
        public void NotValidTurnCausePlayerToBeEliminated()
        {
            Admin a = new Admin();
            Board b = new Board();
            List<Tile> drawpile = new List<Tile>()
            {
                t2,t3
            };

            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(0, 1, 6);
            SPlayer p2 = new SPlayer("p2", new List<Tile>());

            List<SPlayer> l1 = new List<SPlayer>()
            {
                p1,p2
            };

            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult tmpturn = a.playATurn(drawpile, l1, l2, b, t1);

            Assert.IsTrue(tmpturn.eliminatedPlayers.Count == 1);
            Assert.IsTrue(tmpturn.eliminatedPlayers.Exists(x => x.returnColor() == "p1"));
            Assert.IsFalse(tmpturn.currentPlayers.Exists(x => x.returnColor() == "p1"));
            Assert.IsTrue(tmpturn.currentPlayers.Count == 1);
        }
    }
}
