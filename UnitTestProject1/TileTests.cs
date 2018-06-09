using Microsoft.VisualStudio.TestTools.UnitTesting;
using tsuro;
using System.Collections.Generic;
using System;

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
            Assert.IsTrue(t1_test_rotated.isEqualOrRotation(t1_actual_rotated));

            //check that after tile is rotated 4 times, it is equivalent to the orig orientation
            for (int i = 0; i < 3; i++)
            {
                t1_test_rotated = t1_test_rotated.rotate();
            }

            Assert.IsTrue(t1_test_rotated.isEqualOrRotation(t1));

        }

        [TestMethod]
        public void RotatedTileIsEqualToOriginalTile()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Assert.IsTrue(t1.isEqualOrRotation(t1.rotate()));
        }

		[TestMethod]
		public void IsEqualDifferentOrderPaths() {
			TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);
            Tile t1SameDiffOrdering = test.makeTile(1, 3, 4, 7, 2, 6, 0, 5);

			Assert.IsTrue(t1.isEqualOrRotation(t1SameDiffOrdering));
		}

		[TestMethod]
        public void IsEqualRotatedDifferentOrderPaths()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);
			Tile t1RotatedDiffOrdering = test.makeTile(0, 4, 1, 6, 2, 7, 3, 5);

			Tile t1Rotated = t1.rotate();

			Assert.IsTrue(t1Rotated.isEqualOrRotation(t1RotatedDiffOrdering));
        }

        [TestMethod]
        public void TilesIsNotEqual()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);

            Assert.IsFalse(t1.isEqualOrRotation(t2));
        }

        [TestMethod]
        public void TestLocationEndMethod()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Assert.AreEqual(t1.getLocationEnd(0), 1);
        }

		[TestMethod]
		public void SortBySymmetricity() {
			TestScenerios test = new TestScenerios();
			Tile oneSymTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
			Tile twoSymTile = test.makeTile(0, 1, 2, 6, 4, 5, 3, 7);
			Tile fourSymTile = test.makeTile(0, 6, 1, 2, 3, 4, 5, 7);

			List<Tile> toSort = new List<Tile> { twoSymTile, fourSymTile, oneSymTile };
			Tile.sortBySymmetricity(toSort);

			Assert.AreEqual(3, toSort.Count);
			Assert.IsTrue(oneSymTile.isEqualOrRotation(toSort[0]));
			Assert.IsTrue(twoSymTile.isEqualOrRotation(toSort[1]));
			Assert.IsTrue(fourSymTile.isEqualOrRotation(toSort[2]));
		}

		[TestMethod]
		public void GetSymmetricityOneSym() {
			TestScenerios test = new TestScenerios();
            Tile oneSymTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
			Tile oneSymTileUnmodified = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

			Assert.AreEqual(1, oneSymTile.lazyGetSymmetricity());

			// Check original tile was not rotated
			Assert.IsTrue(oneSymTileUnmodified.isEqual(oneSymTile));
		}

		[TestMethod]
        public void GetSymmetricityTwoSym()
        {
			TestScenerios test = new TestScenerios();
			Tile twoSymTile = test.makeTile(0, 1, 2, 6, 4, 5, 3, 7);
			Tile twoSymTileUnmodified = test.makeTile(0, 1, 2, 6, 4, 5, 3, 7);

			Assert.AreEqual(2, twoSymTile.lazyGetSymmetricity());

            // Check original tile was not rotated
			Assert.IsTrue(twoSymTileUnmodified.isEqual(twoSymTile));
        }

		[TestMethod]
        public void GetSymmetricityFourSym()
        {
			TestScenerios test = new TestScenerios();
			Tile fourSymTile = test.makeTile(0, 6, 1, 2, 3, 4, 5, 7);
			Tile fourSymTileUnmodified = test.makeTile(0, 6, 1, 2, 3, 4, 5, 7);

			Assert.AreEqual(4, fourSymTile.lazyGetSymmetricity());

            // Check original tile was not rotated
			Assert.IsTrue(fourSymTileUnmodified.isEqual(fourSymTile));
        }
        
		[TestMethod]
		public void GetDifferentRotations() {
			TestScenerios test = new TestScenerios();

            // Test - only 1 rotation 
			Tile oneSymTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
			Tile oneSymTileUnmodified = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

			List<Tile> actualDiffRotations = oneSymTile.getDifferentRotations();
			Assert.AreEqual(1, actualDiffRotations.Count);
			Assert.IsTrue(oneSymTileUnmodified.isEqual(actualDiffRotations[0]));

			// Test - 4 different rotations
			Tile fourSymTile = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);
			Tile fourSymTileUnmodified = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);
			Tile fourSymRot1 = test.makeTile(0, 4, 1, 6, 2, 7, 3, 5);
			Tile fourSymRot2 = test.makeTile(0, 3, 1, 4, 2, 6, 5, 7);
			Tile fourSymRot3 = test.makeTile(0, 4, 1, 7, 2, 5, 3, 6);

			actualDiffRotations = fourSymTile.getDifferentRotations();
            Assert.AreEqual(4, actualDiffRotations.Count);
			Assert.IsTrue(fourSymTileUnmodified.isEqual(actualDiffRotations[0]));
			Assert.IsTrue(fourSymRot1.isEqual(actualDiffRotations[1]));
			Assert.IsTrue(fourSymRot2.isEqual(actualDiffRotations[2]));
			Assert.IsTrue(fourSymRot3.isEqual(actualDiffRotations[3]));

		}
    }
    
}
