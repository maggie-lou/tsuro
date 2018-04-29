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

        static List<Path> path1 = new List<Path>()
            {
                first,
                second,
                third,
                fourth
            };

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

        [TestMethod]
        public void TileActuallyRotates()
        {
            Tile t1 = new Tile(path1);
            //Tile t1_notrotated = t1;
            Tile t1_test_rotated = t1.rotate();

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

            Tile t1_actual_rotated = new Tile(rpath);
            int j = 0;
            foreach (Path p in t1_actual_rotated.paths)
            {
                Assert.IsTrue(p.isEqual(t1_test_rotated.paths[j]));
                j++;
            }
            //check tile rotates once 
            Assert.IsTrue(t1_test_rotated.isEqual(t1_actual_rotated));

            //check that after tile is rotated 4 times, it is equivalent to the orig orientation
            for (int i = 0; i < 3; i++)
            {
                t1_test_rotated = t1_test_rotated.rotate();
            }

            Assert.IsTrue(t1_test_rotated.isEqual(t1));

        }

        [TestMethod]
        public void RotatedTileIsEqualToOriginalTile()
        {
            Tile t1 = new Tile(path1);
            Assert.IsTrue(t1.isEqual(t1.rotate()));
        }

        [TestMethod]
        public void TilesIsNotEqual()
        {
            Tile t1 = new Tile(path1);
            Tile t2 = new Tile(path2);
            Assert.IsFalse(t1.isEqual(t2));
        }

        [TestMethod]
        public void TestLocationEndMethod()
        {
            Path first = new Path(0, 1);
            Path second = new Path(2, 4);
            Path third = new Path(3, 6);
            Path fourth = new Path(5, 7);

            List<Path> path1 = new List<Path>()
            {
                first,
                second,
                third,
                fourth
            };

            Tile t1 = new Tile(path1);
            Assert.AreEqual(t1.getLocationEnd(0), 1);
        }

        [TestMethod]
        public void TileIs4WaySymmetric()
        {
            Path first = new Path(0, 1);
            Path second = new Path(2, 3);
            Path third = new Path(4, 5);
            Path fourth = new Path(6, 7);

            List<Path> path1 = new List<Path>()
            {
                first,
                second,
                third,
                fourth
            };

            Tile t1 = new Tile(path1);

            Assert.IsTrue(t1.isSymmetric(t1.rotate()));
            Assert.IsTrue(t1.howSymmetric() == 4);
        }

        [TestMethod]
        public void TileIs1WaySymmetric()
        {
            Path first = new Path(0, 5);
            Path second = new Path(1, 3);
            Path third = new Path(2, 6);
            Path fourth = new Path(4, 7);

            List<Path> path1 = new List<Path>()
            {
                first,
                second,
                third,
                fourth
            };

            Tile t1 = new Tile(path1);

            Assert.IsFalse(t1.isSymmetric(t1.rotate()));
            Assert.IsTrue(t1.howSymmetric() == 1);
        }

        [TestMethod]
        public void TileIs2WaySymmetric()
        {
            Path first = new Path(0, 6);
            Path second = new Path(1, 5);
            Path third = new Path(2, 4);
            Path fourth = new Path(3, 7);

            List<Path> path1 = new List<Path>()
            {
                first,
                second,
                third,
                fourth
            };

            Tile t1 = new Tile(path1);

            Assert.IsFalse(t1.isSymmetric(t1.rotate()));
            Assert.IsTrue(t1.howSymmetric() == 2);
        }
    }
    
}
