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
        public void RandomPlayerPlaysATurnAndRemovesTileFromHand()
        {
            RandomPlayer p1 = new RandomPlayer();
            Board b = new Board();

            int[] p1_initial_loc = p1.placePawn(b);
        }
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
            p1.currPlayer.addTileToHand(t1);
            p1.currPlayer.addTileToHand(t2);
            p1.currPlayer.addTileToHand(t3);

            Tile t = p1.playTurn(b, playerHand, 10);
            Assert.IsTrue(playerHand.Exists(x => x.isEqual(t)));
            Assert.AreEqual(p1.currPlayer.returnHand().Count, 2);
            Assert.IsFalse(p1.currPlayer.returnHand().Exists(x => x.isEqual(t)));
        }

        [TestMethod]
        public void LeastSymPlayerChoosesLeastSymTile()
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

            Tile mostSymTile = new Tile(path1);

            Path mfirst = new Path(0, 6);
            Path msecond = new Path(1, 5);
            Path mthird = new Path(2, 4);
            Path mfourth = new Path(3, 7);

            List<Path> mpath1 = new List<Path>()
            {
                mfirst,
                msecond,
                mthird,
                mfourth
            };

            Tile medSymTile = new Tile(mpath1);

            Path lfirst = new Path(0, 5);
            Path lsecond = new Path(1, 3);
            Path lthird = new Path(2, 6);
            Path lfourth = new Path(4, 7);

            List<Path> lpath1 = new List<Path>()
            {
                lfirst,
                lsecond,
                lthird,
                lfourth
            };

            Tile leastSymTile = new Tile(lpath1);
            //purposefully unordered
            List<Tile> playerTiles = new List<Tile> { medSymTile, mostSymTile, leastSymTile };

            LeastSymmetricPlayer p1 = new LeastSymmetricPlayer();

            p1.initialize("blue",new List<string>{"hotpink","green"});

            Board b = new Board();
            p1.placePawn(b);

            //playturn should return the least symmetric tile
            Assert.IsTrue(p1.playTurn(b, playerTiles, 0).isEqual(leastSymTile));
        }

        [TestMethod]
        public void LeastSymPlayerChoosesLeastSymTileRotated()
        {
            //the least symmetric tile is not valid unless rotated once
            //so the player will rotate the least symmetric tile and play that move 
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

            Tile mostSymTile = new Tile(path1);

            Path mfirst = new Path(0, 6);
            Path msecond = new Path(1, 5);
            Path mthird = new Path(2, 4);
            Path mfourth = new Path(3, 7);

            List<Path> mpath1 = new List<Path>()
            {
                mfirst,
                msecond,
                mthird,
                mfourth
            };

            Tile medSymTile = new Tile(mpath1);

            Path lfirst = new Path(0, 5);
            Path lsecond = new Path(1, 3);
            Path lthird = new Path(2, 6);
            Path lfourth = new Path(4, 7);

            List<Path> lpath1 = new List<Path>()
            {
                lfirst,
                lsecond,
                lthird,
                lfourth
            };

            Tile leastSymTile = new Tile(lpath1);
            //purposefully unordered
            List<Tile> playerTiles = new List<Tile> { medSymTile, mostSymTile, leastSymTile };

            LeastSymmetricPlayer p1 = new LeastSymmetricPlayer();

            p1.initialize("blue", new List<string> { "hotpink", "green" });

            Board b = new Board();
            p1.placePawn(b);
            p1.currPlayer.setPosn(1, 0, 0);

            Tile checkTile = p1.playTurn(b, playerTiles, 0);

            //playturn should return the least symmetric tile rotated once
            Assert.IsTrue(checkTile.isEqual(leastSymTile));

            Assert.IsTrue(checkTile.paths[0].isEqual(new Path(2, 7)));
            Assert.IsTrue(checkTile.paths[1].isEqual(new Path(3, 5)));
            Assert.IsTrue(checkTile.paths[2].isEqual(new Path(4, 0)));
            Assert.IsTrue(checkTile.paths[3].isEqual(new Path(6, 1)));
        }

        [TestMethod]
        public void MostSymPlayerChoosesMostSymTile()
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

            Tile mostSymTile = new Tile(path1);

            Path mfirst = new Path(0, 6);
            Path msecond = new Path(1, 5);
            Path mthird = new Path(2, 4);
            Path mfourth = new Path(3, 7);

            List<Path> mpath1 = new List<Path>()
            {
                mfirst,
                msecond,
                mthird,
                mfourth
            };

            Tile medSymTile = new Tile(mpath1);

            Path lfirst = new Path(0, 5);
            Path lsecond = new Path(1, 3);
            Path lthird = new Path(2, 6);
            Path lfourth = new Path(4, 7);

            List<Path> lpath1 = new List<Path>()
            {
                lfirst,
                lsecond,
                lthird,
                lfourth
            };

            Tile leastSymTile = new Tile(lpath1);
            //purposefully unordered
            List<Tile> playerTiles = new List<Tile> { medSymTile, mostSymTile, leastSymTile };

            MostSymmetricPlayer p1 = new MostSymmetricPlayer();

            p1.initialize("blue", new List<string> { "hotpink", "green" });

            Board b = new Board();
            p1.placePawn(b);
            p1.currPlayer.setPosn(2, 2, 2);
            Tile checkTile = p1.playTurn(b, playerTiles, 0);
            //playturn should return the most symmetric tile
            Assert.IsTrue(checkTile.isEqual(mostSymTile));
        }
        [TestMethod]
        public void MostSymPlayerChoosesMidSymTile()
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

            Tile mostSymTile = new Tile(path1);

            Path mfirst = new Path(0, 6);
            Path msecond = new Path(1, 5);
            Path mthird = new Path(2, 4);
            Path mfourth = new Path(3, 7);

            List<Path> mpath1 = new List<Path>()
            {
                mfirst,
                msecond,
                mthird,
                mfourth
            };

            Tile medSymTile = new Tile(mpath1);

            Path lfirst = new Path(0, 5);
            Path lsecond = new Path(1, 3);
            Path lthird = new Path(2, 6);
            Path lfourth = new Path(4, 7);

            List<Path> lpath1 = new List<Path>()
            {
                lfirst,
                lsecond,
                lthird,
                lfourth
            };

            Tile leastSymTile = new Tile(lpath1);
            //purposefully unordered
            List<Tile> playerTiles = new List<Tile> { medSymTile, mostSymTile, leastSymTile };

            MostSymmetricPlayer p1 = new MostSymmetricPlayer();

            p1.initialize("blue", new List<string> { "hotpink", "green" });

            Board b = new Board();
            // player starts at (0,0,0)
            p1.placePawn(b);
            Tile checkTile = p1.playTurn(b, playerTiles, 0);
            //playturn should return the mid symmetric tile (first valid move)
            Assert.IsTrue(checkTile.isEqual(medSymTile));

            Assert.IsTrue(checkTile.paths[0].isEqual(new Path(2, 0)));
            Assert.IsTrue(checkTile.paths[1].isEqual(new Path(3, 7)));
            Assert.IsTrue(checkTile.paths[2].isEqual(new Path(4, 6)));
            Assert.IsTrue(checkTile.paths[3].isEqual(new Path(5, 1)));
        }
    }
}
