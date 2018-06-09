using Microsoft.VisualStudio.TestTools.UnitTesting;
using tsuro;
using System.Collections.Generic;
namespace TsuroTests
{
    [TestClass]
    public class TileTests
    {


		[TestMethod]
        public void TileActuallyRotates()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Tile t1_test_rotated = t1.rotate();

            Tile t1_actual_rotated = test.makeTile(2, 3, 4, 6, 5, 0, 7, 1);

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
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Assert.IsTrue(t1.isEqual(t1.rotate()));
        }

		[TestMethod]
		public void IsEqualDifferentOrderPaths() {
			TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);
            Tile t1SameDiffOrdering = test.makeTile(1, 3, 4, 7, 2, 6, 0, 5);

			Assert.IsTrue(t1.isEqual(t1SameDiffOrdering));
		}

		[TestMethod]
        public void IsEqualRotatedDifferentOrderPaths()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);
			Tile t1RotatedDiffOrdering = test.makeTile(0, 4, 1, 6, 2, 7, 3, 5);

			Tile t1Rotated = t1.rotate();

			Assert.IsTrue(t1Rotated.isEqual(t1RotatedDiffOrdering));
        }

        [TestMethod]
        public void TilesIsNotEqual()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);

            Assert.IsFalse(t1.isEqual(t2));
        }

        [TestMethod]
        public void TestLocationEndMethod()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Assert.AreEqual(t1.getLocationEnd(0), 1);
        }

        [TestMethod]
        public void TileIs4WaySymmetric()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

            Assert.IsTrue(t1.isSymmetric(t1.rotate()));
            Assert.IsTrue(t1.howSymmetric() == 4);
        }

        [TestMethod]
        public void TileIs1WaySymmetric()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);
            
            Assert.IsFalse(t1.isSymmetric(t1.rotate()));
            Assert.IsTrue(t1.howSymmetric() == 1);
        }

        [TestMethod]
        public void TileIs2WaySymmetric()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);

            Assert.IsFalse(t1.isSymmetric(t1.rotate()));
            Assert.IsTrue(t1.howSymmetric() == 2);
        }
    }
    
}
