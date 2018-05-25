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
							Assert.IsTrue(board.onEdge(checkPosn));
						}
						else
						{
							Assert.IsFalse(board.onEdge(checkPosn));
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

			b1.grid[0, 0] = t1;

			Assert.IsNull(b2.grid[0, 0]);
		}

        [TestMethod]
        public void PlayerGetsEliminated()
        {
            SPlayer p1 = new SPlayer("blue", new List<Tile>());
            SPlayer p2 = new SPlayer("red", new List<Tile>());

            Board b = new Board();
            b.registerPlayer(p1);
            p1.playerState = SPlayer.State.Playing;
            b.registerPlayer(p2);

            b.eliminatePlayer(p1);
            Assert.IsFalse(b.returnOnBoard().Contains(p1));
            Assert.IsTrue(b.returnEliminated().Contains(p1));
        }
        [TestMethod]
		public void EliminatedPlayerReturnsHandToDrawPile()
		{
			TestScenerios test = new TestScenerios();
			Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
			Tile t2 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
			Tile t3 = test.makeTile(0, 5, 2, 3, 4, 7, 1, 6);

			List<Tile> hand = test.makeHand(t1, t2);
			SPlayer p1 = new SPlayer("blue", hand, "LeastSymmetric");
			p1.playerState = SPlayer.State.Placed;

			Board board = new Board();
			Assert.AreEqual(0, board.drawPile.Count);
			board.eliminatePlayer(p1);
			Assert.AreEqual(2, board.drawPile.Count);         
		}
        
        [TestMethod]
        public void PlaceTileFirstTurnLeadsToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>());
            Board b = new Board();

            p1.setPosn(new Posn(-1, 0, 4));
            Assert.IsFalse(b.isNotEliminationMove(p1, t1));
        }

        [TestMethod]
        public void PlaceTileLeadsToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>());
            Board b = new Board();

            p1.setPosn(new Posn(1, 0, 0));
            Assert.IsFalse(b.isNotEliminationMove(p1, t1));
        }

        [TestMethod]
        public void CanPlaceTileLeadsToEmptySpace()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>());
            Board b = new Board();

            p1.setPosn(new Posn(0, 0, 3));
            Assert.IsTrue(b.isNotEliminationMove(p1, t1));
        }
        [TestMethod]
        public void PlaceTileInTheMiddleOfBoardLeadsPlayerToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 2, 1, 5, 3, 7, 4, 6);
            Tile t2 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>());
            Board b = new Board();

            p1.setPosn(new Posn(0, 1, 3));
            b.grid[0, 1] = t1;
            Assert.IsFalse(b.isNotEliminationMove(p1, t2));
        }

        [TestMethod]
        public void PlaceTilePlacesTile()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), "Random");
            Board b = new Board();         
            p1.setPosn(new Posn(0, 0, 3));

            Posn endPos = b.placeTile(p1, t1);

			Assert.IsTrue(endPos.returnCol() == 1);
			Assert.IsTrue(endPos.returnRow() == 0);
			Assert.IsTrue(endPos.returnLocationOnTile() == 3);
            Assert.IsTrue(b.occupied(0, 1));
        }

        [TestMethod]
        public void EmptyBoardNotOccupied()
        {
            Board b = new Board();

            Assert.IsFalse(b.occupied(3, 3));
        }

        [TestMethod]
        public void LocationOnBoardIsOccupied()
        {
            SPlayer p1 = new SPlayer("blue", new List<Tile>());
            Board b = new Board();
            b.registerPlayer(p1);

			Posn newPosn = new Posn(0, 0, 3);
            p1.setPosn(newPosn);

            Assert.IsTrue(b.locationOccupied(newPosn));
        }

        [TestMethod]
        public void LocationOnBoardIsNotOccupied()
        {
            SPlayer p1 = new SPlayer("blue", new List<Tile>());
            Board b = new Board();
            b.registerPlayer(p1);
            
			Posn newPosn = new Posn(0, 0, 3);

            p1.setPosn(newPosn);

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
			board.grid[4,0] = onBoard;

			SPlayer p1 = new SPlayer("blue", new List<Tile>());
			board.registerPlayer(p1);
			p1.setPosn(new Posn(6, 0, 1));

			Assert.IsFalse(board.isNotEliminationMove(p1, toPlace));
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
            
            SPlayer p1 = new SPlayer("blue", hand, "Random");
            SPlayer p2 = new SPlayer("green", new List<Tile>(), "Random");
            SPlayer p3 = new SPlayer("hotpink", new List<Tile>(), "Random");

            p1.initialize(board);
            p2.initialize(board);
            p3.initialize(board);
            test.setStartPos00(board, p1);
            test.setStartPos(board, p2, new Posn(3, 3, 3));
            test.setStartPos(board, p3, new Posn(4, 3, 3));

            board.setDragonTileHolder(p2);

            Assert.AreEqual(0, board.drawPile.Count);

            TurnResult tr = admin.playATurn(board.drawPile, board.returnOnBoard(), board.returnEliminated(), board, t1);

            // Green and hotpink both drew a tile 
            // Green has t2
            // Hot pink has t3
            // No dragon tile holder 
            Assert.AreEqual(2, board.returnOnBoard().Count);
            SPlayer greenPlayer = board.returnOnBoard()[0];
            Assert.AreEqual("green", greenPlayer.returnColor());
            SPlayer hotpinkPlayer = board.returnOnBoard()[1];
            Assert.AreEqual("hotpink", hotpinkPlayer.returnColor());

            Assert.AreEqual(1, greenPlayer.returnHand().Count);
            Assert.AreEqual(1, hotpinkPlayer.returnHand().Count);
            Assert.IsTrue(greenPlayer.returnHand().Exists(x => x.isEqual(t2)));
            Assert.IsTrue(hotpinkPlayer.returnHand().Exists(x => x.isEqual(t3)));
            Assert.IsTrue(board.returnDragonTileHolder().returnColor() == "green");    
        }

        [TestMethod]
        public void DragonHolderEliminatedPassestoNextClockwisePlayer() {
            Admin a = new Admin();
            Board board = new Board();

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), "Random");
            SPlayer p2 = new SPlayer("green", new List<Tile>(), "Random");
            SPlayer p3 = new SPlayer("hotpink", new List<Tile>(), "Random");

            // Initialize start positions to satisfy contract - can't be
            //   eliminated before placing pawn
            TestScenerios test = new TestScenerios();
            test.setStartPos00(board, p1);
            test.setStartPos(board, p2, new Posn(3, 3, 3));
            test.setStartPos(board, p3, new Posn(4, 3, 3));
            
            board.setDragonTileHolder(p2);
			board.eliminatePlayer(p2);
         
            Assert.AreEqual("hotpink", board.returnDragonTileHolder().returnColor());
			Assert.IsTrue(board.returnDragonTileHolder().returnHand().Count < 3);
        }
    }
}
