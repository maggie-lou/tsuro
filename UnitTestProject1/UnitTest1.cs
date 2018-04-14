using Microsoft.VisualStudio.TestTools.UnitTesting;
using tsuro;
using System.Collections.Generic;
namespace UnitTests
{
    [TestClass]
    public class TileTests
    {
        [TestMethod]
        public void TileActuallyRotates()
        {
            Path first = new Path(0, 1);
            Path second = new Path(2, 4);
            Path third = new Path(3, 6);
            Path fourth = new Path(5,7);
 
            List<Path> paths = new List<Path>();

            paths.Add(first);
            paths.Add(second);
            paths.Add(third);
            paths.Add(fourth);

            Tile t = new Tile(paths);

            t.rotate();

            first = new Path(2, 3);
            second = new Path(4, 6);
            third = new Path(5, 0);
            fourth = new Path(7, 1);

            List<Path> checkp = new List<Path>();

            checkp.Add(first);
            checkp.Add(second);
            checkp.Add(third);
            checkp.Add(fourth);

            Tile tcheck = new Tile(checkp);
            //check tile rotates once 
            Assert.IsTrue(tcheck.isEqual(t));

            t.rotate();
            t.rotate();
            t.rotate();

            Tile t2 = new Tile(paths);

            Assert.IsTrue(t2.isEqual(t));

        }
    }
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void TileNotInEmptyHand()
        {
            Admin a = new Admin();
            Board b = new Board();
            SPlayer player1 = new SPlayer("blue",null);

            Path first = new Path(0, 1);
            Path second = new Path(2, 4);
            Path third = new Path(3, 6);
            Path fourth = new Path(5, 7);

            List<Path> paths = new List<Path>();

            paths.Add(first);
            paths.Add(second);
            paths.Add(third);
            paths.Add(fourth);

            Tile t1 = new Tile(paths);

            Assert.IsFalse(a.legalPlay(player1, b, t1));
        }

        [TestMethod]
        public void TileInHandNotRotated()
        {
            Admin a = new Admin();
            Board b = new Board();

            Path first = new Path(0, 1);
            Path second = new Path(2, 4);
            Path third = new Path(3, 6);
            Path fourth = new Path(5, 7);

            List<Path> paths = new List<Path>();

            paths.Add(first);
            paths.Add(second);
            paths.Add(third);
            paths.Add(fourth);

            Tile t1 = new Tile(paths);
            first = new Path(0, 6);
            second = new Path(1, 5);
            third = new Path(2, 4);
            fourth = new Path(3, 7);

            paths = new List<Path>();

            paths.Add(first);
            paths.Add(second);
            paths.Add(third);
            paths.Add(fourth);

            Tile t2 = new Tile(paths);

            first = new Path(0, 5);
            second = new Path(1, 4);
            third = new Path(2, 7);
            fourth = new Path(3, 6);

            paths = new List<Path>();

            paths.Add(first);
            paths.Add(second);
            paths.Add(third);
            paths.Add(fourth);

            Tile t3 = new Tile(paths);

            List<Tile> tiles = new List<Tile>();

            tiles.Add(t1);
            tiles.Add(t2);
            tiles.Add(t3);

            SPlayer player1 = new SPlayer("blue", tiles);

        }
    }
}
