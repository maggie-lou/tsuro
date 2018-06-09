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
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            
            SPlayer p1 = new SPlayer();
            p1.addTileToHand(t1);
            Assert.IsTrue(p1.returnHand().Count == 1);
            Assert.IsTrue(p1.returnHand().Exists(x => x.isEqualOrRotation(t1)));

            p1.removeTileFromHand(t1);

            Assert.IsTrue(p1.returnHand().Count == 0);
            Assert.IsFalse(p1.returnHand().Exists(x => x.isEqualOrRotation(t1)));
        }

        [TestMethod]
        public void RemoveTileInHandTest()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer();

            p1.addTileToHand(t1);
			Assert.AreEqual(1, p1.returnHand().Count);
			Assert.IsTrue(p1.returnHand().Exists(x => x.isEqualOrRotation(t1)));

			p1.removeTileFromHand(t1);
			Assert.AreEqual(0, p1.returnHand().Count);
			Assert.IsFalse(p1.returnHand().Exists(x => x.isEqualOrRotation(t1)));
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
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Admin a = new Admin();
            Board b = new Board();
            SPlayer player1 = new SPlayer("blue", new List<Tile>());

            Assert.IsFalse(player1.tileInHand(t1));
        }

        [TestMethod]
        public void TileInHandNotRotated()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile t3 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);
            List<Tile> hand = test.makeHand(t1, t2, t3);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer player1 = new SPlayer("blue", hand);
            Assert.IsTrue(player1.tileInHand(t1));
        }

        [TestMethod]
        public void TileInHandRotated()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile t3 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);
            List<Tile> hand = test.makeHand(t1, t2, t3);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer player1 = new SPlayer("blue", hand);

            Tile t1_rotated = t1;
         
			Assert.IsTrue(player1.tileInHand(t1_rotated.rotate()));
        }

        [TestMethod]
		public void SPlayerPlayTurnChoosesValidMoveAndRemovesTileFromHand()
		{
			TestScenerios test = new TestScenerios();
			Tile t1 = test.makeTile(4, 3, 5, 2, 6, 7, 0, 1);
			Tile t2 = test.makeTile(4, 3, 5, 7, 6, 1, 0, 2);
			Tile t3 = test.makeTile(4, 5, 6, 0, 7, 1, 2, 3);
            List<Tile> hand = test.makeHand(t1, t2, t3);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer player1 = new SPlayer("blue", hand);
			player1.setPosn(new Posn(1, 0, 0));
		}
    }
}
