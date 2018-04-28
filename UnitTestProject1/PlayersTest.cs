using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tsuro;

namespace RandomTest
{
    [TestClass]
    public class RandomTest
    {
        [TestMethod]
        public void RandomPlayerPlacesPawnOnEdgeWithNoOtherPlayers()
        {
            RandomPlayer p1 = new RandomPlayer();
            Board b = new Board();

            int[] p1_initial_loc = p1.placePawn(b);

            CollectionAssert.AreEqual(p1_initial_loc, new int[] { 0, 0, 0 });
            Assert.IsTrue(b.locationOccupied(0,0,0));
        }

        [TestMethod]
        public void RandomPlayerPlacesPawnOnEdgeWithOtherPlayers()
        {
            RandomPlayer p1 = new RandomPlayer();
            Board b = new Board();
            SPlayer p2 = new SPlayer();
            p2.setPosn(0, 0, 0);
            b.registerPlayer(p2);
            SPlayer p3 = new SPlayer();
            p3.setPosn(0, 0, 1);
            b.registerPlayer(p3);

            int[] p1_initial_loc = p1.placePawn(b);

            CollectionAssert.AreEqual(p1_initial_loc, new int[] { 0, 1, 0 });
        }
    }
}
