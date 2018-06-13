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
            SPlayer p1 = new SPlayer("blue", new List<Tile>(), new RandomPlayer());
            Board b = new Board();
            
			p1.initialize("blue", new List<string>{"blue"});
			p1.placePawn(b);
			Assert.IsFalse(b.isElimPosn(b.getPlayerPosn(p1.getColor())));
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
			SPlayer p1 = new SPlayer("blue", playerHand, new RandomPlayer());

			p1.initialize("blue", new List<string>{"blue"});
			test.setStartPos(b, p1, new Posn(2, 2, 2));

            Tile t = p1.playTurn(b,0);
            Assert.AreEqual(2,p1.getHand().Count);
            Assert.IsFalse(p1.getHand().Exists(x => x.isEqualOrRotation(t)));
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

            SPlayer lsp1 = new SPlayer("blue", playerTiles, new LeastSymmetricPlayer());
			lsp1.initialize("blue", new List<string>{"blue"});
			test.setStartPos00(b, lsp1);
            //playturn should return the least symmetric tile
            Assert.IsTrue(lsp1.playTurn(b, 0).isEqualOrRotation(leastSymTile));
        }

        [TestMethod]
        public void LeastSymPlayerChoosesLeastSymTileRotated()
        {
            //the least symmetric tile is not valid unless rotated
            //so the player will rotate the least symmetric tile and play that move 
            TestScenerios test = new TestScenerios();
            Tile mostSymTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile medSymTile = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile leastSymTile = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);

            //purposefully unordered
            List<Tile> playerTiles = test.makeHand(medSymTile, mostSymTile, leastSymTile);

            SPlayer p1 = new SPlayer("blue", playerTiles, new LeastSymmetricPlayer());
            
            Board b = new Board();
			p1.initialize("blue", new List<string> { "blue" });
			test.setStartPos(b, p1, new Posn(0, 1, 7));

            Tile actualTile = p1.playTurn(b, 0);

            //playturn should return the least symmetric tile rotated once
			Assert.IsTrue(actualTile.isEqualOrRotation(leastSymTile));
			Tile leastSymRot3 = test.makeTile(0, 4, 1, 7, 2, 5, 3, 6);
			Assert.IsTrue(leastSymRot3.isEqual(actualTile));
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

            SPlayer p1 = new SPlayer("blue", playerTiles, new MostSymmetricPlayer());

            Board b = new Board();
			p1.initialize("blue", new List<string> { "blue" });
			test.setStartPos(b, p1, new Posn(2, 2, 2));
            Tile checkTile = p1.playTurn(b, 0);
            //playturn should return the most symmetric tile
            Assert.IsTrue(checkTile.isEqualOrRotation(mostSymTile));
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

            SPlayer p1 = new SPlayer("blue", playerTiles, new MostSymmetricPlayer());

            Board b = new Board();
			p1.initialize("blue", new List<string> { "blue" });
			test.setStartPos(b, p1, new Posn(2, 2, 2));
            Tile checkTile = p1.playTurn(b, 0);
            //playturn should return the most symmetric tile
            Assert.IsTrue(checkTile.isEqualOrRotation(mostSymTile1));
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

            SPlayer p1 = new SPlayer("hotpink", playerTiles, new MostSymmetricPlayer());

            Board b = new Board();
			p1.initialize("hotpink", new List<string>{"hotpink"});
			test.setStartPos(b, p1, new Posn(-1, 0, 5));
            
            Tile checkTile = p1.playTurn(b, 0);
            //playturn should return the mid symmetric tile (first valid move)
            Assert.IsTrue(checkTile.isEqualOrRotation(medSymTile));

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
            
			Admin a = test.createAdminWithDrawPile(new List<Tile>{});
            Board b = new Board();
			b.placeTileAt(mostSymTile, 1, 1);

            SPlayer p1 = new SPlayer("blue", test.makeHand(mostSymTile, medSymTile), new MostSymmetricPlayer());
			p1.initialize("blue", new List<string> { "blue" });
			test.setStartPos00(b, p1);
			Tile t = p1.playTurn(b, a.getDrawPileSize());
			Assert.IsTrue(a.isCheating(p1, b, t));
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

            SPlayer p1 = new SPlayer("blue", playerHand, new MostSymmetricPlayer());
			p1.initialize("blue", new List<string> { "blue" });
			test.setStartPos00(b, p1);
            Assert.IsTrue(a.isCheating(p1, b, mostSymTile));
        }
        [TestMethod]
        public void PlayerHasRotatedVersionOfSameTileInHand()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Admin a = new Admin();
            Board b = new Board();

            List<Tile> playerHand = test.makeHand(t1, t1.rotate());

            SPlayer p1 = new SPlayer("blue", playerHand, new MostSymmetricPlayer());
			p1.initialize("blue", new List<string> { "blue" });
			test.setStartPos00(b, p1);
			a.addToActivePlayers(p1);

			Tile t = p1.playTurn(b, a.getDrawPileSize());
            Assert.IsTrue(a.isCheating(p1, b, t));
        }
    }
}
