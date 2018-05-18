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
            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            SPlayer p2 = new SPlayer("red", new List<Tile>(), true);

            Board b = new Board();
            b.registerPlayer(p1);
            p1.playerState = SPlayer.State.Playing;
            b.registerPlayer(p2);

            b.eliminatePlayer(p1);
            Assert.IsFalse(b.returnOnBoard().Contains(p1));
            Assert.IsTrue(b.returnEliminated().Contains(p1));
        }

        [TestMethod]
        public void PlaceTileFirstTurnLeadsToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), false);
            Board b = new Board();

            p1.setPosn(new Posn(-1, 0, 4));
            Assert.IsFalse(b.isNotEliminationMove(p1, t1));
        }

        [TestMethod]
        public void PlaceTileLeadsToEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            Board b = new Board();

            p1.setPosn(new Posn(1, 0, 0));
            Assert.IsFalse(b.isNotEliminationMove(p1, t1));
        }

        [TestMethod]
        public void CanPlaceTileLeadsToEmptySpace()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
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

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
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

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            Board b = new Board();

            p1.setPosn(new Posn(0, 0, 3));

            SPlayer pcheck = b.placeTile(p1, t1);
            Posn playerPosn = pcheck.getPlayerPosn();
            Assert.IsTrue(playerPosn.returnCol() == 1);
            Assert.IsTrue(playerPosn.returnRow() == 0);
            Assert.IsTrue(playerPosn.returnLocationOnTile() == 3);
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
            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
            Board b = new Board();
            b.registerPlayer(p1);

			Posn newPosn = new Posn(0, 0, 3);
            p1.setPosn(newPosn);

            Assert.IsTrue(b.locationOccupied(newPosn));
        }

        [TestMethod]
        public void LocationOnBoardIsNotOccupied()
        {
            SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
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

			SPlayer p1 = new SPlayer("blue", new List<Tile>(), true);
			board.registerPlayer(p1);
			p1.setPosn(new Posn(6, 0, 1));

			Assert.IsFalse(board.isNotEliminationMove(p1, toPlace));
		}
    }
}
