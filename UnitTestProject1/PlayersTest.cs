using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tsuro;

namespace TsuroTests
{
    [TestClass]
    public class PlayersTest
    {
		[TestMethod]
        public void GenerateRandomStartTileLocation()
        {
            int row = -1;
            int col = 3;
            int generatedTileLoc = AutomatedPlayer.getRandomStartTilePosition(row, col);
            Assert.IsTrue(generatedTileLoc == 4 || generatedTileLoc == 5);
        }

        [TestMethod]
        public void GenerateRandomStartPosition()
        {
            Posn generatedPosn = AutomatedPlayer.generateRandomStartPosn();
            // One position coordinate must be a phantom coordinate
            Boolean rowIsPhantom = (generatedPosn.returnRow() == -1 ||
                                    generatedPosn.returnRow() == 6);
            Boolean colIsPhantom = (generatedPosn.returnCol() == -1 ||
                                    generatedPosn.returnCol() == 6);
            Assert.IsTrue(rowIsPhantom ^ colIsPhantom);

            // Other position coordinate must be valid board coordinate
            if (rowIsPhantom)
            {
                Assert.IsTrue(generatedPosn.returnCol() >= 0 && generatedPosn.returnCol() <= 5);
            }
            else
            {
                Assert.IsTrue(generatedPosn.returnRow() >= 0 && generatedPosn.returnRow() <= 5);
            }
        }

        [TestMethod]
        public void RandomPlayerPlacesPawnOnEdgeWithNoOtherPlayers()
        {
            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true, "Random");
            Board b = new Board();
            
            p1.initialize(b);
			Posn checkPosn = p1.playerStrategy.placePawn(b);
            Assert.IsFalse(b.locationOccupied(checkPosn));
        }
      
        [TestMethod]
        public void RandomPlayerChoosesTileWhenAllMovesAreValidAndRemovesTileFromHand()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile t3 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);

            List<Tile> playerHand = test.makeHand(t1, t2, t3);

            Board b = new Board();
            SPlayer p1 = new SPlayer("blue",playerHand,true,"Random");

            p1.initialize(b);
            p1.placePawn(b);
            p1.setPosn(new Posn(2, 2, 2));

            Tile t = p1.playTurn(b,0);
            Assert.AreEqual(2,p1.returnHand().Count);
            Assert.IsFalse(p1.returnHand().Exists(x => x.isEqual(t)));
        }

        [TestMethod]
        public void LeastSymPlayerChoosesLeastSymTile()
        {
            TestScenerios test = new TestScenerios();
            Tile mostSymTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile medSymTile = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile leastSymTile = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);

            //purposefully unordered
            List<Tile> playerTiles = test.makeHand(medSymTile, mostSymTile, leastSymTile);
            Board b = new Board();

            SPlayer lsp1 = new SPlayer("blue", playerTiles, true, "LeastSymmetric");
            lsp1.initialize(b);
            lsp1.placePawn(b);
            //playturn should return the least symmetric tile
            Assert.IsTrue(lsp1.playTurn(b, 0).isEqual(leastSymTile));
        }

        [TestMethod]
        public void LeastSymPlayerChoosesLeastSymTileRotated()
        {
            //the least symmetric tile is not valid unless rotated once
            //so the player will rotate the least symmetric tile and play that move 
            TestScenerios test = new TestScenerios();
            Tile mostSymTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile medSymTile = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile leastSymTile = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);

            //purposefully unordered
            List<Tile> playerTiles = test.makeHand(medSymTile, mostSymTile, leastSymTile);

            SPlayer p1 = new SPlayer("blue", playerTiles, true, "LeastSymmetric");
            //p1.initialize("blue", new List<string> { "hotpink", "green" });

            Board b = new Board();
            p1.initialize(b);
            p1.placePawn(b);
            p1.setPosn(new Posn(1, 0, 0));

            Tile checkTile = p1.playTurn(b, 0);

            //playturn should return the least symmetric tile rotated once
            Assert.IsTrue(checkTile.isEqual(leastSymTile));

            Assert.IsTrue(checkTile.paths[0].isEqual(new Path(2, 7)));
            Assert.IsTrue(checkTile.paths[1].isEqual(new Path(3, 5)));
            Assert.IsTrue(checkTile.paths[2].isEqual(new Path(4, 0)));
            Assert.IsTrue(checkTile.paths[3].isEqual(new Path(6, 1)));
        }
        [TestMethod]
        public void LeastSymPlayerChoosesFirstTileIfAllTilesAreSameSym()
        {
            TestScenerios test = new TestScenerios();
            //the least symmetric tile is not valid unless rotated once
            //so the player will rotate the least symmetric tile and play that move 
            Tile leastSymTile1 = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);
            Tile leastSymTile2 = test.makeTile(0, 3, 1, 4, 2, 7, 5, 6);
            Tile medSymTile = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);

            //purposefully unordered
            List<Tile> playerTiles = test.makeHand(medSymTile, leastSymTile1, leastSymTile2);

            Board b = new Board();
            
            SPlayer lsp1 = new SPlayer("blue", playerTiles, true, "LeastSymmetric");
            lsp1.initialize(b);
            lsp1.placePawn(b);

            //playturn should return the least symmetric tile
            Assert.IsTrue(lsp1.playTurn(b, 0).isEqual(leastSymTile1));
        }

        [TestMethod]
        public void MostSymPlayerChoosesMostSymTile()
        {
            TestScenerios test = new TestScenerios();
            Tile mostSymTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile medSymTile = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile leastSymTile = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);

            //purposefully unordered
            List<Tile> playerTiles = test.makeHand(medSymTile, mostSymTile, leastSymTile);

            SPlayer p1 = new SPlayer("blue", playerTiles, true, "MostSymmetric");

            Board b = new Board();
            p1.initialize(b);
            p1.placePawn(b);
            p1.setPosn(new Posn(2, 2, 2));
            Tile checkTile = p1.playTurn(b, 0);
            //playturn should return the most symmetric tile
            Assert.IsTrue(checkTile.isEqual(mostSymTile));
        }

        [TestMethod]
        public void MostSymPlayerChoosesOneOfMostSymTile()
        {
            TestScenerios test = new TestScenerios();
            Tile mostSymTile1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile medSymTile = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile mostSymTile2 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);

            //purposefully unordered
            List<Tile> playerTiles = test.makeHand(medSymTile, mostSymTile1, mostSymTile2);

            SPlayer p1 = new SPlayer("blue", playerTiles, true, "MostSymmetric");

            Board b = new Board();
            p1.initialize(b);
            p1.placePawn(b);
            p1.setPosn(new Posn(2, 2, 2));
            Tile checkTile = p1.playTurn(b, 0);
            //playturn should return the most symmetric tile
            Assert.IsTrue(checkTile.isEqual(mostSymTile1));
        }

        [TestMethod]
        public void MostSymPlayerChoosesMidSymTile()
        {
            TestScenerios test = new TestScenerios();
            Tile mostSymTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile medSymTile = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile leastSymTile = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);

            //purposefully unordered
            List<Tile> playerTiles = test.makeHand(medSymTile, mostSymTile, leastSymTile);

            SPlayer p1 = new SPlayer("hotpink", playerTiles, false, "MostSymmetric");

            Board b = new Board();
            // player starts at (0,0,0)
            p1.initialize(b);
            p1.placePawn(b);
            Tile checkTile = p1.playTurn(b, 0);
            //playturn should return the mid symmetric tile (first valid move)
            Assert.IsTrue(checkTile.isEqual(medSymTile));

            Assert.IsTrue(checkTile.paths[0].isEqual(new Path(2, 0)));
            Assert.IsTrue(checkTile.paths[1].isEqual(new Path(3, 7)));
            Assert.IsTrue(checkTile.paths[2].isEqual(new Path(4, 6)));
            Assert.IsTrue(checkTile.paths[3].isEqual(new Path(5, 1)));
        }

        [TestMethod]
        public void PlayerHandAlreadyOnBoard()
        {
            TestScenerios test = new TestScenerios();
            Tile mostSymTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile medSymTile = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile leastSymTile = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);

            Admin a = new Admin();
            Board b = new Board();
            b.grid[1, 1] = mostSymTile;
            b.onBoardTiles.Add(mostSymTile);

            SPlayer p1 = new SPlayer("blue", test.makeHand(mostSymTile, medSymTile), true, "MostSymmetric");
            p1.initialize(b);
            p1.placePawn(b);
            p1.playTurn(b, b.drawPile.Count);
            Assert.IsInstanceOfType(p1.playerStrategy, typeof(RandomPlayer));
        }

        [TestMethod]
        public void PlayerHasTooManyTilesInHand()
        {
            TestScenerios test = new TestScenerios();
            Tile mostSymTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile medSymTile = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile leastSymTile = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);
            Tile extraTile = test.makeTile(0, 4, 1, 7, 2, 6, 3, 5);

            Admin a = new Admin();
            Board b = new Board();
            List<Tile> playerHand = test.makeHand(mostSymTile, medSymTile, leastSymTile, extraTile);

            SPlayer p1 = new SPlayer("blue", playerHand, true, "MostSymmetric");
            p1.initialize(b);
            p1.placePawn(b);
            p1.playTurn(b, b.drawPile.Count);
            Assert.IsInstanceOfType(p1.playerStrategy, typeof(RandomPlayer));
        }
        [TestMethod]
        public void PlayerHasRotatedVersionOfSameTileInHand()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Admin a = new Admin();
            Board b = new Board();

            List<Tile> playerHand = test.makeHand(t1, t1.rotate());

            SPlayer p1 = new SPlayer("blue", playerHand, true, "MostSymmetric");
            p1.initialize(b);
            p1.placePawn(b);
            p1.playTurn(b, b.drawPile.Count);
            Assert.IsInstanceOfType(p1.playerStrategy, typeof(RandomPlayer));
        }
    }
}
