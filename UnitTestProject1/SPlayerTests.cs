using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tsuro;

namespace TsuroTests
{
    [TestClass]
    public class SPlayerTests
    {
        [TestMethod]
        public void TileInHandTest()
        {
            Path first = new Path(0, 1);
            Path second = new Path(2, 4);
            Path third = new Path(3, 6);
            Path fourth = new Path(5, 7);

            List<Path> paths = new List<Path>()
            {
                first,
                second,
                third,
                fourth
            };
            Tile t1 = new Tile(paths);
            SPlayer p1 = new SPlayer();
            p1.addTileToHand(t1);
            Assert.IsTrue(p1.returnHand().Count == 1);
            Assert.IsTrue(p1.returnHand().Exists(x => x.isEqual(t1)));

            p1.removeTileFromHand(t1);

            Assert.IsTrue(p1.returnHand().Count == 0);
            Assert.IsFalse(p1.returnHand().Exists(x => x.isEqual(t1)));
        }

        [TestMethod]
        public void RemoveTileInHandTest()
        {
            Path first = new Path(0, 1);
            Path second = new Path(2, 4);
            Path third = new Path(3, 6);
            Path fourth = new Path(5, 7);

            List<Path> paths = new List<Path>()
            {
                first,
                second,
                third,
                fourth
            };
            Tile t1 = new Tile(paths);
            SPlayer p1 = new SPlayer();
            p1.addTileToHand(t1);

            Assert.IsTrue(p1.removeTileFromHand(t1));

            Assert.IsTrue(p1.returnHand().Count == 0);
            Assert.IsFalse(p1.returnHand().Exists(x => x.isEqual(t1)));

            Assert.IsFalse(p1.removeTileFromHand(t1));
        }

        [TestMethod]
        public void SettingPlayerPosition()
        {
            SPlayer p1 = new SPlayer();
            p1.setPosn(new Posn(1, 2, 3));
            Posn playerPosn = p1.getPlayerPosn();
            Assert.IsTrue(playerPosn.returnRow() == 1);
            Assert.IsTrue(playerPosn.returnCol() == 2);
            Assert.IsTrue(playerPosn.returnLocationOnTile() == 3);
        }

        [TestMethod]
        public void TileNotInEmptyHand()
        {
            Path first1 = new Path(0, 1);
            Path second1 = new Path(2, 4);
            Path third1 = new Path(3, 6);
            Path fourth1 = new Path(5, 7);

            List<Path> path1 = new List<Path>()
        {
            first1,
            second1,
            third1,
            fourth1
        };

            Path first2 = new Path(0, 6);
            Path second2 = new Path(1, 5);
            Path third2 = new Path(2, 4);
            Path fourth2 = new Path(3, 7);

            List<Path> path2 = new List<Path>()
        {
            first2,
            second2,
            third2,
            fourth2
        };

            Path first3 = new Path(0, 5);
            Path second3 = new Path(1, 4);
            Path third3 = new Path(2, 7);
            Path fourth3 = new Path(3, 6);

            List<Path> path3 = new List<Path>()
        {
            first3,
            second3,
            third3,
            fourth3
        };

            Path first4 = new Path(0, 1);
            Path second4 = new Path(2, 7);
            Path third4 = new Path(4, 5);
            Path fourth4 = new Path(3, 6);

            List<Path> path4 = new List<Path>()
        {
            first4,
            second4,
            third4,
            fourth4
        };
            Tile t1 = new Tile(path1);

            Admin a = new Admin();
            Board b = new Board();
            SPlayer player1 = new SPlayer("blue", new List<Tile>(), false);

            Assert.IsFalse(player1.tileInHand(t1));
        }

        [TestMethod]
        public void TileInHandNotRotated()
        {
            Path first1 = new Path(0, 1);
            Path second1 = new Path(2, 4);
            Path third1 = new Path(3, 6);
            Path fourth1 = new Path(5, 7);

            List<Path> path1 = new List<Path>()
        {
            first1,
            second1,
            third1,
            fourth1
        };

            Path first2 = new Path(0, 6);
            Path second2 = new Path(1, 5);
            Path third2 = new Path(2, 4);
            Path fourth2 = new Path(3, 7);

            List<Path> path2 = new List<Path>()
        {
            first2,
            second2,
            third2,
            fourth2
        };

            Path first3 = new Path(0, 5);
            Path second3 = new Path(1, 4);
            Path third3 = new Path(2, 7);
            Path fourth3 = new Path(3, 6);

            List<Path> path3 = new List<Path>()
        {
            first3,
            second3,
            third3,
            fourth3
        };

            Path first4 = new Path(0, 1);
            Path second4 = new Path(2, 7);
            Path third4 = new Path(4, 5);
            Path fourth4 = new Path(3, 6);

            List<Path> path4 = new List<Path>()
        {
            first4,
            second4,
            third4,
            fourth4
        };
            Tile t1 = new Tile(path1);
            Tile t2 = new Tile(path2);
            Tile t3 = new Tile(path3);
            Admin a = new Admin();
            Board b = new Board();

            List<Tile> hand = new List<Tile>()
            {
                t1,
                t2,
                t3
            };

            SPlayer player1 = new SPlayer("blue", hand, false);
            Assert.IsTrue(player1.tileInHand(t1));
        }

        [TestMethod]
        public void TileInHandRotated()
        {
            Path first1 = new Path(0, 1);
            Path second1 = new Path(2, 4);
            Path third1 = new Path(3, 6);
            Path fourth1 = new Path(5, 7);

            List<Path> path1 = new List<Path>()
        {
            first1,
            second1,
            third1,
            fourth1
        };

            Path first2 = new Path(0, 6);
            Path second2 = new Path(1, 5);
            Path third2 = new Path(2, 4);
            Path fourth2 = new Path(3, 7);

            List<Path> path2 = new List<Path>()
        {
            first2,
            second2,
            third2,
            fourth2
        };

            Path first3 = new Path(0, 5);
            Path second3 = new Path(1, 4);
            Path third3 = new Path(2, 7);
            Path fourth3 = new Path(3, 6);

            List<Path> path3 = new List<Path>()
        {
            first3,
            second3,
            third3,
            fourth3
        };

            Path first4 = new Path(0, 1);
            Path second4 = new Path(2, 7);
            Path third4 = new Path(4, 5);
            Path fourth4 = new Path(3, 6);

            List<Path> path4 = new List<Path>()
        {
            first4,
            second4,
            third4,
            fourth4
        };
            Tile t1 = new Tile(path1);
            Tile t2 = new Tile(path2);
            Tile t3 = new Tile(path3);
            Admin a = new Admin();
            Board b = new Board();

            List<Tile> hand = new List<Tile>()
            {
                t1,
                t2,
                t3
            };

            SPlayer player1 = new SPlayer("blue", hand, false);

            Tile t1_rotated = t1;
            t1_rotated.rotate();

            Assert.IsTrue(player1.tileInHand(t1_rotated));
        }
    }
}
