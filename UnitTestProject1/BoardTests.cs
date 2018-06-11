using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tsuro;

namespace TsuroTests
{
    [TestClass]
    public class BoardTests
    {
		[TestMethod]
        public void PosnIsOnTheEdge()
		{
			Board board = new Board();
			List<int> edgeRow = new List<int> { 0, 5 };
			List<int> edgeCol = new List<int> { 0, 5 };
			List<Posn> edgePosn = new List<Posn>();
            
            foreach (int row in edgeRow)
			{
				if (row == 0)
                {
                    for (int col = 0; col < 6; col++)
                    {
                        edgePosn.Add(new Posn(row, col, 0));
                        edgePosn.Add(new Posn(row, col, 1));
                    }
                }
                
                if (row == 5)
                {
                    for (int col = 0; col < 6; col++)
                    {
                        edgePosn.Add(new Posn(row, col, 4));
                        edgePosn.Add(new Posn(row, col, 5));
                    }
                }
			}

			foreach (int col in edgeCol)
			{
				if (col == 0)
                {
                    for (int row = 0; row < 6; row++)
                    {
                        edgePosn.Add(new Posn(row, col, 6));
                        edgePosn.Add(new Posn(row, col, 7));
                    }
                }

                if (col == 5)
                {
                    for (int row = 0; row < 6; row++)
                    {
                        edgePosn.Add(new Posn(row, col, 2));
                        edgePosn.Add(new Posn(row, col, 3));
                    }
                }
			}

            for (int row = -1; row <= 6; row++)
			{
                for (int col = -1; col <= 6; col++)
				{
					for (int loc = 0; loc <= 8; loc++)
					{
						Posn checkPosn = new Posn(row,col,loc);
						if (edgePosn.Exists(x=>x.isEqual(checkPosn)))
						{
							Assert.IsTrue(board.isElimPosn(checkPosn));
						}
						else
						{
							Assert.IsFalse(board.isElimPosn(checkPosn));
						}
					}
				}
			}

		}
		[TestMethod]
		public void Clone() 
		{
			// Clone, change first board, check second board didn't change
			Board b1 = new Board();
			Board b2 = b1.clone();
			TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
			b1.placeTileAt(t1, 0, 0);
            
			Assert.IsNull(b2.getTileAt(0,0));
		}

        [TestMethod]
        public void PlayerGetsEliminated()
        {
            SPlayer p1 = new SPlayer("blue", new List<Tile>());
            SPlayer p2 = new SPlayer("red", new List<Tile>());
			p1.playerState = SPlayer.State.Playing;
            p2.playerState = SPlayer.State.Playing;

			Admin admin = new Admin(new List<SPlayer> { p1, p2 }, new List<SPlayer>(), null, null);
            
            admin.eliminatePlayer(p1);
			Assert.IsFalse(admin.isActivePlayer("blue"));
			Assert.IsTrue(admin.isEliminatedPlayer("blue"));
			Assert.IsTrue(admin.isActivePlayer("red"));
        }

        [TestMethod]
		public void EliminatedPlayerReturnsHandToDrawPile()
		{
			TestScenerios test = new TestScenerios();
			Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
			Tile t2 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
			Tile t3 = test.makeTile(0, 5, 2, 3, 4, 7, 1, 6);
            
			List<Tile> hand = test.makeHand(t1, t2);
			SPlayer p1 = new SPlayer("blue", hand, new LeastSymmetricPlayer());

			Admin admin = new Admin();
			Board board = new Board();
			test.setStartPos(board, p1, new Posn(-1, 0, 4));

           
			Assert.AreEqual(0, admin.getDrawPileSize());
			admin.eliminatePlayer(p1.getColor());
			Assert.AreEqual(2, admin.getDrawPileSize());         
		}

		[TestMethod]
		public void GetLegalMoves() {
			TestScenerios test = new TestScenerios();
			Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
			Tile t2 = test.makeTile(0, 1, 2, 6, 3, 7, 4, 5);
			Tile t3 = test.makeTile(0, 5, 1, 3, 2, 6, 4, 7);
			List<Tile> hand = new List<Tile> { t1, t2, t3 };
			Board b = new Board();
			SPlayer p1 = test.createPlayerAtPos("blue", hand, new RandomPlayer(),
												new Posn(-1, 0, 4), b);

            // Call test function
			List<Tile> actualLegalMoves = b.getLegalMoves(hand, "blue");
            
			// Construct expected result
			Tile t2Rot = test.makeTile(0, 4, 1, 5, 2, 3, 6, 7);
			Tile t3Rot2 = test.makeTile(0, 3, 1, 4, 2, 6, 5, 7);
			List<Tile> expected = new List<Tile> { t2Rot, t3, t3Rot2 };
            
			Assert.AreEqual(3, actualLegalMoves.Count);
			for (int i = 0; i < actualLegalMoves.Count; i++) {
				Assert.IsTrue(actualLegalMoves[i].isEqualOrRotation(expected[i]));
			}
		}

		[TestMethod]
		public void GetLegalMovesAllElimination() {
			TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            List<Tile> hand = new List<Tile> { t1 };
            Board b = new Board();
            SPlayer p1 = test.createPlayerAtPos("blue", hand, new RandomPlayer(),
                                                new Posn(-1, 0, 4), b);
            
            // Call test function
            List<Tile> actualLegalMoves = b.getLegalMoves(hand, "blue");

            // Construct expected result
            List<Tile> expected = new List<Tile> { t1 };

            Assert.AreEqual(1, actualLegalMoves.Count);
			Assert.IsTrue(actualLegalMoves[0].isEqualOrRotation(expected[0]));
		}
        
        [TestMethod]
        public void PlaceTileFirstTurnLeadsToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Board b = new Board();
			b.addPlayerToBoard("blue", new Posn(-1, 0, 4));

            Assert.IsTrue(b.isEliminationMove("blue", t1));
        }

        [TestMethod]
        public void PlaceTileLeadsToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            
            Board b = new Board();
			b.addPlayerToBoard("blue", new Posn(1, 0, 0));

            Assert.IsTrue(b.isEliminationMove("blue", t1));
        }

        [TestMethod]
        public void CanPlaceTileLeadsToEmptySpace()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Board b = new Board();
			b.addPlayerToBoard("blue", new Posn(0,0,3));

            Assert.IsFalse(b.isEliminationMove("blue", t1));
        }
        [TestMethod]
        public void PlaceTileInTheMiddleOfBoardLeadsPlayerToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 2, 1, 5, 3, 7, 4, 6);
            Tile t2 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

			Board b = new Board();
			b.placeTileAt(t1, 0, 1);
            b.addPlayerToBoard("blue", new Posn(0, 1, 3));

            Assert.IsTrue(b.isEliminationMove("blue", t1));
        }

        [TestMethod]
        public void PlaceTilePlacesTile()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Board b = new Board();
			b.addPlayerToBoard("blue", new Posn(0, 0, 3));

            Posn endPos = b.placeTile("blue", t1);

			Assert.IsTrue(endPos.returnCol() == 1);
			Assert.IsTrue(endPos.returnRow() == 0);
			Assert.IsTrue(endPos.returnLocationOnTile() == 3);
			Assert.IsTrue(b.getTileAt(0, 1) != null);
        }

        [TestMethod]
        public void EmptyBoardNotOccupied()
        {
            Board b = new Board();

			Assert.IsTrue(b.getTileAt(3,3) == null);
        }

        [TestMethod]
        public void LocationOnBoardIsOccupied()
        {
            Board b = new Board();
			Posn newPosn = new Posn(0, 0, 3);
			b.addPlayerToBoard("blue", newPosn);

            Assert.IsTrue(b.locationOccupied(newPosn));
        }

        [TestMethod]
        public void LocationOnBoardIsNotOccupied()
        {
            Board b = new Board();            
			Posn newPosn = new Posn(0, 0, 3);
			b.addPlayerToBoard("blue", newPosn);

            Assert.IsFalse(b.locationOccupied(new Posn(0,0,0)));
        }
     

		[TestMethod]
		public void MovesOffAndBackOntoTileEliminatesSelf() {
			// Set up - player plays a tile that will cause them to move onto another tile, 
            // and then back onto the tile they just played, and eliminate themself 
			TestScenerios test = new TestScenerios();
            Tile onBoard = test.makeTile(0, 1, 2, 6, 3, 7, 5, 4);
			Tile toPlace = test.makeTile(0, 5, 1, 4, 2, 6, 3, 7);

			Board board = new Board();
			board.placeTileAt(onBoard, 4, 0);
            
			board.addPlayerToBoard("blue", new Posn(6, 0, 1));

			Assert.IsTrue(board.isEliminationMove("blue", toPlace));
		}

        [TestMethod]
        public void PlayerEliminatedOtherPlayersDrawRefilledDeck() {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile t2 = test.makeTile(0, 7, 2, 3, 4, 5, 6, 1);
            Tile t3 = test.makeTile(0, 3, 2, 1, 4, 5, 6, 7);

            List<Tile> hand = test.makeHand(t2, t3);

            Board board = new Board();
            Admin admin = new Admin();
            
            SPlayer p1 = new SPlayer(null, hand, new RandomPlayer());
            SPlayer p2 = new SPlayer(new RandomPlayer());
            SPlayer p3 = new SPlayer(new RandomPlayer());

			p1.initialize("blue", new List<string> {"blue", "green", "hotpink"});
			p2.initialize("green", new List<string> { "blue", "green", "hotpink" });
			p3.initialize("hotpink", new List<string> { "blue", "green", "hotpink" });
            test.setStartPos00(board, p1);
            test.setStartPos(board, p2, new Posn(3, 3, 3));
            test.setStartPos(board, p3, new Posn(4, 3, 3));

			admin.setDragonTileHolder(p2);

			Assert.AreEqual(0, admin.getDrawPileSize());

            TurnResult tr = admin.playATurn(board, t1);
            
            // Green and hotpink both drew a tile 
            // Green has t2
            // Hot pink has t3
            // No dragon tile holder 
			Assert.AreEqual(2, admin.numActive());
			SPlayer greenPlayer = admin.getFirstActivePlayer();
            Assert.AreEqual("green", greenPlayer.getColor());
			Assert.IsTrue(admin.isActivePlayer("hotpink"));

            Assert.AreEqual(1, greenPlayer.getHand().Count);
            Assert.AreEqual(1, p3.getHand().Count);
            Assert.IsTrue(greenPlayer.getHand().Exists(x => x.isEqualOrRotation(t2)));
            Assert.IsTrue(p3.getHand().Exists(x => x.isEqualOrRotation(t3)));
			Assert.IsTrue(admin.isDragonHolder("green"));
        }

        [TestMethod]
        public void DragonHolderEliminatedPassestoNextClockwisePlayer() {
            Admin a = new Admin();
            Board board = new Board();

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), new RandomPlayer());
            SPlayer p2 = new SPlayer("green", new List<Tile>(), new RandomPlayer());
            SPlayer p3 = new SPlayer("hotpink", new List<Tile>(), new RandomPlayer());

            // Initialize start positions to satisfy contract - can't be
            //   eliminated before placing pawn
            TestScenerios test = new TestScenerios();
            test.setStartPos00(board, p1);
            test.setStartPos(board, p2, new Posn(3, 3, 3));
            test.setStartPos(board, p3, new Posn(4, 3, 3));

			a.setDragonTileHolder(p2);
			a.eliminatePlayer(p2.getColor());
         
           
			Assert.IsTrue(a.isDragonHolder("hotpink"));
			Assert.IsTrue(p3.getHand().Count < 3);
        }

		[TestMethod]
        public void DragonTileHolderChangesFromPlayerToNullIfAfterDrawingAllPlayersHave3Tiles()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile t3 = test.makeTile(0, 3, 2, 4, 1, 6, 5, 7);
            Tile t4 = test.makeTile(0, 2, 4, 5, 1, 7, 3, 6);
            Tile t5 = test.makeTile(1, 2, 3, 4, 5, 6, 7, 0);
            Tile t6 = test.makeTile(1, 5, 2, 4, 3, 7, 0, 6);
            
			Admin admin = test.createAdminWithDrawPile(new List<Tile> { t1, t2 });
			Board board = new Board();
            SPlayer p1 = test.createPlayerAtPos("blue", new List<Tile> { t3, t4 }, new RandomPlayer(), new Posn(2, 2, 2), board);
			SPlayer p2 = test.createPlayerAtPos("red", new List<Tile> { t3, t4 }, new RandomPlayer(), new Posn(2, 2, 2), board);
			SPlayer p3 = test.createPlayerAtPos("sienna", new List<Tile> { t3, t4, t1 }, new RandomPlayer(), new Posn(2, 2, 2), board);

			admin.setDragonTileHolder(p1);
			admin.drawTilesWithDragonHolder();
            
			Assert.IsNull(admin.getDragonTileHolder());         
        }

        [TestMethod]
		public void EndOfGameFrom35TilesBeingPlaced()
		{
			TestScenerios test = new TestScenerios();
			Board board = new Board();
			Admin admin = new Admin();
			Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            for (int i = 0; i < 6; i++)
			{
                for (int j = 0; j < 6; j++)
				{
					board.placeTileAt(t1, i, j);
				}
			}

            // Clear one tile 
			board.placeTileAt(null, 1, 1);

			SPlayer p1 = test.createPlayerAtPos("green", new List<Tile> {}, new RandomPlayer(), new Posn(2, 2, 2), board);
            SPlayer p2 = test.createPlayerAtPos("sienna", new List<Tile> {}, new RandomPlayer(), new Posn(2, 2, 3), board);

			TurnResult tr = admin.playATurn(board, t1);
			Assert.IsTrue(tr.playResult != null);
		}

        [TestMethod]
		public void DragonTileHolderGetsEliminatedAndDragonTileHolderReturnsToNull()
		{
			TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile t3 = test.makeTile(0, 3, 2, 4, 1, 6, 5, 7);
            Tile t4 = test.makeTile(0, 2, 4, 5, 1, 7, 3, 6);
            Tile t5 = test.makeTile(1, 2, 3, 4, 5, 6, 7, 0);
            Tile t6 = test.makeTile(1, 5, 2, 4, 3, 7, 0, 6);

			Board board = new Board();
			Admin admin = new Admin();
            
			SPlayer p1 = test.createPlayerAtPos("red", new List<Tile> { t3, t4 }, new RandomPlayer(), new Posn(2, 2, 2), board);
			SPlayer p2 = test.createPlayerAtPos("green", new List<Tile> { t3, t4, t2 }, new RandomPlayer(), new Posn(2, 2, 2), board);
			SPlayer p3 = test.createPlayerAtPos("sienna", new List<Tile> { t3, t4, t1 }, new RandomPlayer(), new Posn(2, 2, 2), board);
            SPlayer p4 = test.createPlayerAtPos("blue", new List<Tile> { t3, t4 }, new RandomPlayer(), new Posn(2, 2, 2), board);

            admin.setDragonTileHolder(p4);
			admin.eliminatePlayer(p1.getColor());
			admin.eliminatePlayer(p4.getColor());

			Assert.AreEqual(4, admin.getDrawPileSize());
			Assert.IsNull(admin.getDragonTileHolder());
		}

        [TestMethod]
		public void DragonTileHolderGetsEliminatedAndReturnsTilesToDrawPile()
		{
			TestScenerios test = new TestScenerios();
			Tile toPlayTile = test.makeTile(0, 4, 1, 5, 2, 6, 3, 7);
            Tile redTile1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile redTile2 = test.makeTile(0, 3, 2, 4, 1, 6, 5, 7);
            Tile blueTile1 = test.makeTile(0, 2, 4, 5, 1, 7, 3, 6);
            Tile blueTile2 = test.makeTile(1, 2, 3, 4, 5, 6, 7, 0);
            Tile randTile = test.makeTile(1, 5, 2, 4, 3, 7, 0, 6);

			Board board = new Board();
			Admin admin = new Admin();
            
			SPlayer p1 = test.createPlayerAtPos("red", new List<Tile> { redTile1, redTile2 }, new RandomPlayer(), new Posn(1, 2, 1), board);
            SPlayer p2 = test.createPlayerAtPos("green", new List<Tile> { randTile, randTile, randTile }, new RandomPlayer(), new Posn(3, 3, 3), board);
			SPlayer p3 = test.createPlayerAtPos("sienna", new List<Tile> { randTile, randTile, randTile }, new RandomPlayer(), new Posn(1, 5, 5), board);
			SPlayer p4 = test.createPlayerAtPos("blue", new List<Tile> { blueTile1, blueTile2 }, new RandomPlayer(), new Posn(1, 2, 0), board);

			admin.setDragonTileHolder(p4);
			admin.playATurn(board, toPlayTile);

			Assert.AreEqual(4, admin.getDrawPileSize());
			Assert.IsNull(admin.getDragonTileHolder());
		}
    }
}
