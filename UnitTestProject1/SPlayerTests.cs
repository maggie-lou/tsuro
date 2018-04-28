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

        [TestMethod]
        public void TileInHandTest()
        {
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
            p1.setPosn(1, 2, 3);

            Assert.IsTrue(p1.getboardLocationRow() == 1);
            Assert.IsTrue(p1.getboardLocationCol() == 2);
            Assert.IsTrue(p1.getLocationOnTile() == 3);
        }
    }
}
