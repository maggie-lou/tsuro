using Microsoft.VisualStudio.TestTools.UnitTesting;
using tsuro;
using System.Collections.Generic;
namespace TsuroTests
{
    [TestClass]
    public class TileTests
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

        [TestMethod]
        public void TileActuallyRotates()
        {
            Tile t1_notrotated = t1;
            t1.rotate();

            Path rfirst = new Path(2, 3);
            Path rsecond = new Path(4, 6);
            Path rthird = new Path(5, 0);
            Path rfourth = new Path(7, 1);

            List<Path> rpath = new List<Path>()
            {
                rfirst,
                rsecond,
                rthird,
                rfourth
            };

            Tile t1_rotated = new Tile(rpath);

            //check tile rotates once 
            Assert.IsTrue(t1_rotated.isEqual(t1));

            //check that after tile is rotated 4 times, it is equivalent to the orig orientation
            t1.rotate();
            t1.rotate();
            t1.rotate();

            Assert.IsTrue(t1_notrotated.isEqual(t1));

        }

        [TestMethod]
        public void RotatedTileIsEqualToOriginalTile()
        {
            Assert.IsTrue(t1.isEqual(t1.rotate()));
        }

        [TestMethod]
        public void TilesIsNotEqual()
        {
            Assert.IsFalse(t1.isEqual(t2));
        }

        [TestMethod]
        public void TestLocationEndMethod()
        {
            Assert.IsTrue(t1.getLocationEnd(0) == 1);
        }
    }
    
}
