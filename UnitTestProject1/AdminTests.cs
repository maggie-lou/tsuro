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

        Tile t1 = new Tile(path1);

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

        Tile t2 = new Tile(path2);

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

        Tile t3 = new Tile(path3);

        [TestMethod]
        public void TileNotInEmptyHand()
        {
            Admin a = new Admin();
            Board b = new Board();
            SPlayer player1 = new SPlayer("blue", null);

            Assert.IsFalse(a.legalPlay(player1, b, t1));
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
            Assert.IsTrue(a.legalPlay(player1, b, t1));
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

            Assert.IsTrue(a.legalPlay(player1, b, t1_rotated));

        }
    }
}
