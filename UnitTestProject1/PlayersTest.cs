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
            Assert.IsTrue(b.locationOccupied(0, 0, 0));
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

        [TestMethod]
        public void RandomPlayerChoosesTileWhenAllMovesAreValid()
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
            Tile t1 = new Tile(path1);
            Tile t2 = new Tile(path2);
            Tile t3 = new Tile(path3);
            List<Tile> playerHand = new List<Tile>()
            {
                t1,t2,t3
            };

            Board b = new Board();
            RandomPlayer p1 = new RandomPlayer();
            p1.placePawn(b);
            p1.currPlayer.setPosn(2, 2, 2);


            Tile t = p1.playTurn(b, playerHand, 10);
            Assert.IsTrue(playerHand.Exists(x => x.isEqual(t)));
        }
    }
}
