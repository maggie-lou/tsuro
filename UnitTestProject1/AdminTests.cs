﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tsuro;

namespace TsuroTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void drawATileDrawsTile()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Board b = new Board();
            Admin a = new Admin();

            b.addTileToDrawPile(t1);

            Tile tcheck = b.drawATile();

            Assert.IsTrue(tcheck.isEqual(t1));
            Assert.IsNull(b.drawATile());
        }

        [TestMethod]
        public void DrawATileFromEmptyDrawpile()
        {
            Board b = new Board();
            Admin a = new Admin();

            Assert.IsNull(b.drawATile());
        }

        [TestMethod]
		public void PlayerOrderUpdatesAfterEndOfTurn() {
			TestScenerios test = new TestScenerios();
			Board b = new Board();
			Admin a = new Admin();
            
			SPlayer p1 = test.createPlayerAtPos("blue", new List<Tile>(), new RandomPlayer(),
                                                new Posn(3, 3, 1), b);
            SPlayer p2 = test.createPlayerAtPos("hotpink", new List<Tile>(), new RandomPlayer(),
                                                new Posn(4, 3, 1), b);
			SPlayer p3 = test.createPlayerAtPos("green", new List<Tile>(), new RandomPlayer(),
			                                    new Posn(4, 2, 1), b);

			Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

			TurnResult tr = a.playATurn(b, t1);
            
			Assert.AreEqual(3, tr.currentPlayers.Count);
			Assert.AreEqual("hotpink", tr.currentPlayers[0].returnColor());
			Assert.AreEqual("green", tr.currentPlayers[1].returnColor());
			Assert.AreEqual("blue", tr.currentPlayers[2].returnColor());
	
		}

		[TestMethod]
        public void PlayerOrderUpdatesAfterEndOfTurnWithElimination()
        {
			TestScenerios test = new TestScenerios();
            Board b = new Board();
            Admin a = new Admin();
            
            SPlayer p1 = test.createPlayerAtPos("blue", new List<Tile>(), new RandomPlayer(),
                                                new Posn(-1, 0, 5), b);
            SPlayer p2 = test.createPlayerAtPos("hotpink", new List<Tile>(), new RandomPlayer(),
                                                new Posn(0, -1, 3), b);
            SPlayer p3 = test.createPlayerAtPos("green", new List<Tile>(), new RandomPlayer(),
                                                new Posn(4, 2, 1), b);

            Tile t1 = test.makeTile(0, 3, 6, 1, 2, 5, 4, 7);

            TurnResult tr = a.playATurn(b, t1);
            
            Assert.AreEqual(2, tr.currentPlayers.Count);
			Assert.AreEqual(1, tr.eliminatedPlayers.Count);
			Assert.AreEqual("hotpink", tr.eliminatedPlayers[0].returnColor());
            Assert.AreEqual("green", tr.currentPlayers[0].returnColor());
            Assert.AreEqual("blue", tr.currentPlayers[1].returnColor());
        }

		[TestMethod]
		public void DragonHolderDoesntDrawEmptyDrawPile() {
			TestScenerios test = new TestScenerios();
            Board b = new Board();
            Admin a = new Admin();

            SPlayer p1 = test.createPlayerAtPos("blue", new List<Tile>(), new RandomPlayer(),
                                                new Posn(-1, 0, 5), b);
            SPlayer p2 = test.createPlayerAtPos("hotpink", new List<Tile>(), new RandomPlayer(),
                                                new Posn(0, -1, 3), b);
            SPlayer p3 = test.createPlayerAtPos("green", new List<Tile>(), new RandomPlayer(),
                                                new Posn(4, 2, 1), b);

			b.setDragonTileHolder(p2);

            Tile t1 = test.makeTile(0, 3, 6, 1, 2, 5, 4, 7);

            TurnResult tr = a.playATurn(b, t1);

			Assert.AreEqual(0, p2.returnHand().Count);
		}

        [TestMethod]
        public void PlayAValidTurnRemovesTileFromDrawPile()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile t3 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);

            Admin a = new Admin();
			List<Tile> drawpile = test.makeDrawPile(t2, t3);         
			Board b = test.createBoardWithDrawPile(drawpile);

            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            Posn p1Pos = new Posn(0, 0, 3);
            p1.setPosn(p1Pos);

            SPlayer p2 = new SPlayer("p2", new List<Tile>());
            Posn p2Pos = new Posn(4, 4, 0);
            p2.setPosn(p2Pos);

            b.registerPlayer(p1);
            b.registerPlayer(p2);

            List<SPlayer> l1 = new List<SPlayer>()
            {
                p1,p2
            };

            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult tmpturn = a.playATurn(b, t1);

            Assert.AreEqual(1, tmpturn.drawPile.Count);
            Assert.IsFalse(tmpturn.drawPile.Exists(x => x.isEqual(t2)));

            List<Tile> hand = tmpturn.currentPlayers[1].returnHand();

            Assert.IsTrue(hand.Exists(x => x.isEqual(t2)));
        }

        [TestMethod]
        public void PlayAValidTurnChangesOrderOfInGamePlayers()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile t3 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);

            Admin a = new Admin();
			List<Tile> drawpile = test.makeDrawPile(t2, t3);
            Board b = test.createBoardWithDrawPile(drawpile);
        
            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            Posn p1Pos = new Posn(0, 0, 3);
            p1.setPosn(p1Pos);
            SPlayer p2 = new SPlayer("p2", new List<Tile>());
            Posn p2Pos = new Posn(4, 4, 0);
            p2.setPosn(p2Pos);

            b.registerPlayer(p1);
            b.registerPlayer(p2);

            List<SPlayer> l1 = new List<SPlayer>()
            {
                p1,p2
            };

            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult tmpturn = a.playATurn(b, t1);

            Assert.IsTrue(tmpturn.currentPlayers[0].returnColor() == "p2");
            Assert.IsTrue(tmpturn.currentPlayers[1].returnColor() == "p1");
            Assert.IsTrue(tmpturn.currentPlayers.Count == 2);
        }

        [TestMethod]
        public void ValidTurnCausePlayerToBeEliminated()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile t3 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);

            Admin a = new Admin();
			List<Tile> drawpile = test.makeDrawPile(t2, t3);
            Board b = test.createBoardWithDrawPile(drawpile);
         
            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            Posn p1Pos = new Posn(0, 1, 6);
			test.setStartPos(b, p1, p1Pos);
            SPlayer p2 = new SPlayer("p2", new List<Tile>());
            Posn p2Pos = new Posn(4,4,0);
			test.setStartPos(b, p2, p2Pos);
                     
            TurnResult tmpturn = a.playATurn(b, t1);

			Assert.IsTrue(b.returnEliminated().Count == 1, "count of eliminated players has not increased to 1");
            Assert.IsTrue(tmpturn.eliminatedPlayers.Count == 1, "count of eliminated players has not increased to 1");
            Assert.IsTrue(tmpturn.eliminatedPlayers.Exists(x => x.returnColor() == "p1"), "p1 has not been moved to eliminated players");
            Assert.IsFalse(tmpturn.currentPlayers.Exists(x => x.returnColor() == "p1"), "p1 has not been removed from current players");
            Assert.IsTrue(tmpturn.currentPlayers.Count == 1, "count of current players has not decreased to 1");
        }

        [TestMethod]
        public void MakingAMoveFromTheEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(new Posn(0, -1, 3));

            b.registerPlayer(p1);

            TurnResult tr = a.playATurn(b, t1);
            Posn playerPosn = tr.currentPlayers[0].getPlayerPosn();
            Assert.IsTrue(playerPosn.returnLocationOnTile() == 3);
            Assert.IsTrue(playerPosn.returnRow() == 0);
            Assert.IsTrue(playerPosn.returnCol() == 0);
        }

        [TestMethod]
        public void MakeAMoveCauseTokenToCrossMultipleTiles()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile t3 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);
            Tile t4 = test.makeTile(0, 1, 2, 7, 4, 5, 3, 6);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(new Posn(1, 1, 3));

            b.registerPlayer(p1);

            b.grid[1, 1] = t2;
            b.grid[1, 3] = t1;
            b.grid[1, 4] = t3;

            TurnResult tr = a.playATurn(b, t4);
            Posn playerPosn = tr.currentPlayers[0].getPlayerPosn();
            Assert.IsTrue(playerPosn.returnRow() == 1);
            Assert.IsTrue(playerPosn.returnCol() == 4);
            Assert.IsTrue(playerPosn.returnLocationOnTile() == 3);
        }

		[TestMethod]
		public void Player1MoveCausesPlayer2MovementBeforeFirstTurn() {
			TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 2, 1, 5, 3, 7, 4, 6);
			Board board = new Board();
			Admin admin = new Admin();
            
			SPlayer p1 = new SPlayer("blue", new List<Tile>(), new RandomPlayer());
			p1.initialize(board);
			test.setStartPos(board, p1, new Posn(5, 6, 7));

			SPlayer p2 = new SPlayer("green", new List<Tile>(), new RandomPlayer());
			p2.initialize(board);
            test.setStartPos(board, p2, new Posn(5, 6, 6));

			TurnResult tr = admin.playATurn(board, t1);
            
			Posn p1EndPosExpected = new Posn(5, 5, 0);
			Posn p2EndPosExpected = new Posn(5, 5, 7);

			Posn p1EndPosActual = tr.currentPlayers[1].getPlayerPosn();
			Posn p2EndPosActual = tr.currentPlayers[0].getPlayerPosn();


			Assert.IsTrue(p1EndPosExpected.isEqual(p1EndPosActual));
			Assert.IsTrue(p2EndPosExpected.isEqual(p2EndPosActual));

		}

		[TestMethod]
        public void Player1MoveCausesPlayer2EliminationBeforeFirstTurn()
        {
			TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 2, 1, 5, 3, 4, 6, 7);
            Board board = new Board();
            Admin admin = new Admin();

            SPlayer p1 = new SPlayer("blue", new List<Tile>(), new RandomPlayer());
            p1.initialize(board);
            test.setStartPos(board, p1, new Posn(5, 6, 7));

            SPlayer p2 = new SPlayer("green", new List<Tile>(), new RandomPlayer());
            p2.initialize(board);
            test.setStartPos(board, p2, new Posn(5, 6, 6));

            TurnResult tr = admin.playATurn(board, t1);

            Posn p1EndPosExpected = new Posn(5, 5, 0);
            
            Posn p1EndPosActual = tr.currentPlayers[0].getPlayerPosn();     

            Assert.IsTrue(p1EndPosExpected.isEqual(p1EndPosActual));
			Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.returnColor() == "green"));
			Assert.IsTrue(tr.playResult.Exists(x => x.returnColor() == "blue"));
        }
        
        [TestMethod]
        public void MakeAMoveCauseMultiplePlayersToMove()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 2, 1, 6, 3, 5, 4, 7);
            Tile t2 = test.makeTile(0, 1, 2, 7, 3, 4, 5, 6);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(new Posn(1, 0, 2));

            SPlayer p2 = new SPlayer("p2", new List<Tile>());
            p2.setPosn(new Posn(0, 1, 5));

            SPlayer p3 = new SPlayer("p3", new List<Tile>());
            p3.setPosn(new Posn(1, 2, 6));

            b.registerPlayer(p1);
            b.registerPlayer(p2);
            b.registerPlayer(p3);

            b.grid[1, 2] = t2;

            TurnResult tr = a.playATurn(b, t1);
            Posn playerPosn0 = tr.currentPlayers[0].getPlayerPosn();
            Posn playerPosn1 = tr.currentPlayers[1].getPlayerPosn();
            Posn playerPosn2 = tr.currentPlayers[2].getPlayerPosn();

            Assert.IsTrue(playerPosn0.returnRow() == 1);
            Assert.IsTrue(playerPosn0.returnCol() == 2);
            Assert.IsTrue(playerPosn0.returnLocationOnTile() == 2);

            Assert.IsTrue(playerPosn1.returnRow() == 1);
            Assert.IsTrue(playerPosn1.returnCol() == 1);
            Assert.IsTrue(playerPosn1.returnLocationOnTile() == 5);

            Assert.IsTrue(playerPosn2.returnRow() == 1);
            Assert.IsTrue(playerPosn2.returnCol() == 1);
            Assert.IsTrue(playerPosn2.returnLocationOnTile() == 4);

            Assert.IsNull(tr.playResult);
        }


        [TestMethod]
        public void MakeAMoveWhenTileIsRotated()
        {
            Admin a = new Admin();
            Board b = new Board();
            List<SPlayer> inGame = new List<SPlayer>();

            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(new Posn(1, 1, 3));
            b.registerPlayer(p1);
            inGame.Add(p1);

            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 3, 6, 4, 7, 2, 1, 5);

            Tile rotatedTile = t1.rotate();

            TurnResult tr = a.playATurn(b, rotatedTile);
            Posn playerPosn = tr.currentPlayers[0].getPlayerPosn();
            Assert.IsTrue(playerPosn.returnCol() == 2,"p1 not at correct col");
            Assert.IsTrue(playerPosn.returnRow() == 1,"p1 not at correct row");
            Assert.IsTrue(playerPosn.returnLocationOnTile() == 0,"p1 not at correct location on tile");
            Assert.IsTrue(tr.currentPlayers.Exists(x => x.returnColor() == "p1"),"p1 not in winning players");
        }

        [TestMethod]
        public void MakeMoveWhereMultiplePlayersEliminated()
        {
            Admin a = new Admin();
            Board b = new Board();

            TestScenerios test = new TestScenerios();

            //tile to be placed
            Tile t1 = test.makeTile(7, 0, 6, 1, 5, 4, 2, 3);

            //tile the player is on
            Tile t2 = test.makeTile(1, 3, 0, 5, 2, 7, 4, 6);

			test.putTileOnBoard(t2, b, 1, 1);
         
            //players to be eliminated
            SPlayer elim1 = new SPlayer("elim1", new List<Tile>());
            elim1.setPosn(new Posn(0, 0, 2));
            elim1.playerState = SPlayer.State.Playing;
            SPlayer elim2 = new SPlayer("elim2", new List<Tile>());
            elim2.setPosn(new Posn(0, 0, 3));
            elim2.playerState = SPlayer.State.Playing;
            //player left over
            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(new Posn(1, 1, 0));
            p1.playerState = SPlayer.State.Playing;

            b.registerPlayer(p1);
            b.registerPlayer(elim1);
            b.registerPlayer(elim2);

            TurnResult tr = a.playATurn(b, t1);
            Posn playerPosn = tr.currentPlayers[0].getPlayerPosn();

            Assert.AreEqual(playerPosn.returnLocationOnTile(), 3,"Remaining player not at location 3 on tile");
            Assert.IsTrue(playerPosn.returnCol() == 1);
            Assert.IsTrue(playerPosn.returnRow() == 1);
            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.returnColor() == "elim1"),"eliminated player is in eliminated list");
            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.returnColor() == "elim2"), "eliminated player is in eliminated list");

            Assert.IsNotNull(tr.playResult);
            Assert.IsTrue(tr.playResult.Exists(x => x.returnColor() == "p1"), "p1 not in the winning list of players");
            Assert.AreEqual(tr.playResult.Count, 1);
        }

        [TestMethod]
        public void PlayerTakesDragonTile()
        {
			TestScenerios test = new TestScenerios();
            Admin a = new Admin();
            Board b = new Board();

			SPlayer p1 = test.createPlayerAtPos("p1", new List<Tile>(), new RandomPlayer(),
												new Posn(3, 3, 1), b);
			SPlayer p2 = test.createPlayerAtPos("p2", new List<Tile>(), new RandomPlayer(),
                                                new Posn(4, 3, 1), b);
                                                
            //tile to be placed
            Tile t1 = test.makeTile(7, 0, 6, 1, 5, 4, 2, 3);

            TurnResult tr = a.playATurn(b, t1);

			Assert.IsTrue(tr.b.isDragonTileHolder("p1"));
        }

        [TestMethod]
        public void DragonTileBeforeTurnStillNoNewTiles()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 2, 1, 6, 3, 5, 4, 7);
            Tile t2 = test.makeTile(0, 1, 2, 7, 3, 4, 5, 6);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(new Posn(1, 0, 2));

            SPlayer p2 = new SPlayer("p2", new List<Tile>());
            p2.setPosn(new Posn(0, 1, 5));

            SPlayer p3 = new SPlayer("p3", new List<Tile>());
            p3.setPosn(new Posn(1, 2, 6));

            b.registerPlayer(p1);
            b.registerPlayer(p2);
            b.registerPlayer(p3);

            b.grid[1, 2] = t2;
            
            TurnResult tr = a.playATurn(b, t1);

			Assert.IsTrue(tr.b.isDragonTileHolder("p1"));
            Assert.IsTrue(tr.currentPlayers[0].returnHand().Count == 0);
            Assert.IsTrue(tr.currentPlayers[1].returnHand().Count == 0);
            Assert.IsTrue(tr.currentPlayers[2].returnHand().Count == 0);
        }

        [TestMethod]
        public void PlayerHasDragonTileCausesPlayerToBeEliminated()
        {
            Admin a = new Admin();
            Board b = new Board();
            TestScenerios test = new TestScenerios();

            //tile to be placed
            Tile t1 = test.makeTile(7, 0, 6, 1, 5, 4, 2, 3);

            //tile the player is on
            Tile t2 = test.makeTile(1, 3, 0, 5, 2, 7, 4, 6);

            Tile t3 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile t4 = test.makeTile(1, 2, 3, 4, 5, 6, 7, 0);

            b.grid[1, 1] = t2;

            //players to be eliminated
            List<Tile> elim1Tiles = new List<Tile>() { t3, t4 };
            SPlayer elim1 = new SPlayer("elim1", elim1Tiles);
            elim1.setPosn(new Posn(0, 0, 2));
            elim1.playerState = SPlayer.State.Playing;

            //players left over
            SPlayer p2 = new SPlayer("p2", new List<Tile>());
            p2.setPosn(new Posn(4, 4, 3));
            p2.playerState = SPlayer.State.Playing;

            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(new Posn(1, 1, 0));
            p1.playerState = SPlayer.State.Playing;

            b.registerPlayer(p1);
            b.registerPlayer(elim1);
            b.registerPlayer(p2);

            b.setDragonTileHolder(p1);

            TurnResult tr = a.playATurn( b, t1);

			Assert.IsTrue(tr.b.isDragonTileHolder("p1"));
            Assert.AreEqual(1, tr.eliminatedPlayers.Count);
            Assert.AreEqual(2, tr.currentPlayers.Count);
            Assert.AreEqual(1, b.returnEliminated().Count);
			Assert.AreEqual(2, b.getNumActive());
            Assert.AreEqual(1, p2.returnHand().Count);
            Assert.AreEqual(1, p1.returnHand().Count);
            Assert.IsTrue(p1.returnHand().Contains(t3));
            Assert.IsTrue(p2.returnHand().Contains(t4));
			Assert.AreEqual("p2", b.getFirstActivePlayer().returnColor());
        }

        [TestMethod]
        public void PlayerWithoutDragonTileCausesDragonTileHolderToBeEliminated()
        {
            Admin a = new Admin();
            Board b = new Board();
            TestScenerios test = new TestScenerios();

            //tile to be placed
            Tile t1 = test.makeTile(7, 0, 6, 1, 5, 4, 2, 3);

            //tile the player is on
            Tile t2 = test.makeTile(1, 3, 0, 5, 2, 7, 4, 6);

            Tile t3 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile t4 = test.makeTile(1, 2, 3, 4, 5, 6, 7, 0);

            b.grid[1, 1] = t2;

            //players to be eliminated
            List<Tile> elim1Tiles = new List<Tile>() { t3, t4 };
			SPlayer elim1 = test.createPlayerAtPos("elim1", elim1Tiles, new RandomPlayer(),
												   new Posn(0, 0, 2), b);

			//players left over
			SPlayer p2 = test.createPlayerAtPos("p2", new List<Tile>(), new RandomPlayer(),
												new Posn(4, 4, 3), b);
			SPlayer p1 = test.createPlayerAtPos("p1", new List<Tile>(), new RandomPlayer(),
			                                    new Posn(1, 1, 0), b);

            b.setDragonTileHolder(elim1);

            TurnResult tr = a.playATurn( b, t1);

			Assert.IsTrue(tr.b.isDragonTileHolder("p2"));
			Assert.AreEqual(1, tr.eliminatedPlayers.Count);
			Assert.AreEqual(2, tr.currentPlayers.Count);
			Assert.AreEqual(1, b.returnEliminated().Count);
			Assert.AreEqual(2, b.getNumActive());
			Assert.AreEqual(1, p2.returnHand().Count);
			Assert.AreEqual(1, p1.returnHand().Count);
			Assert.IsTrue(p2.returnHand().Contains(t3));
			Assert.IsTrue(p1.returnHand().Contains(t4));         
        }

        
        [TestMethod]
        [DeploymentItem("drawPilepaths.txt")]
        public void InitializeDrawPile()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

            Admin a = new Admin();

            List<Tile> drawPile = a.initializeDrawPile("drawPilepaths.txt");

            Assert.IsTrue(drawPile[0].isEqual(t1));
        }
        [TestMethod]
        [DeploymentItem("drawPilepaths.txt")]
        public void DealTilesAtTheBeginningOfAGame()
        {
            Admin a = new Admin();
            List<Tile> drawPile = a.initializeDrawPile("drawPilepaths.txt");
            Board b = new Board();
            b.drawPile = drawPile;

            SPlayer rp1 = new SPlayer("blue", new List<Tile>(), new RandomPlayer());
            rp1.initialize(b);
            rp1.placePawn(b);

            SPlayer rp2 = new SPlayer("hotpink", new List<Tile>(), new RandomPlayer());
            rp2.initialize(b);
            rp2.placePawn(b);

			a.dealTiles(new List<SPlayer>{rp1, rp2}, b);

            Assert.AreEqual(b.drawPile.Count, 29);
        }

        [TestMethod]
        public void TwoPlayerGameWherePlayerEliminatesHimselfAndOtherPlayerWins()
        {
            TestScenerios test = new TestScenerios();

            Admin a = new Admin();
            Board b = new Board();
            //tile to be placed
            Tile t1 = test.makeTile(7, 0, 6, 1, 5, 4, 2, 3);
            //tile the player is on
            Tile t2 = test.makeTile(1, 3, 0, 5, 2, 7, 4, 6);

            b.grid[1, 1] = t2;

            //players to be eliminated
            SPlayer elim1 = new SPlayer("elim1", new List<Tile>());
            elim1.setPosn(new Posn(0, 0, 2));
            elim1.playerState = SPlayer.State.Playing;

            ////player left over
            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            p1.setPosn(new Posn(1, 1, 0));
            p1.playerState = SPlayer.State.Playing;

            b.registerPlayer(elim1);
            b.registerPlayer(p1);

            TurnResult tr = a.playATurn(b, t1);

            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.returnColor() == "elim1"), "eliminated player is in eliminated list");
            Assert.IsTrue(tr.currentPlayers.Exists(x => x.returnColor() == "p1"), "p1 is not inGamePlayers list");


            Assert.IsNotNull(tr.playResult);
            Assert.IsTrue(tr.playResult.Exists(x => x.returnColor() == "p1"), "p1 not in the winning list of players");
            Assert.AreEqual(tr.playResult.Count, 1);
        }

        [TestMethod]
        public void TwoPlayerGameWhereBothTie()
        {
            TestScenerios test = new TestScenerios();

            Admin a = new Admin();
            Board b = new Board();
            //tile to be placed
            Tile t1 = test.makeTile(7, 0, 6, 1, 5, 4, 2, 3);
            //tile the player is on
            Tile t2 = test.makeTile(1, 3, 0, 5, 2, 7, 4, 6);

            b.grid[1, 1] = t2;

            //players to be eliminated
            SPlayer elim1 = new SPlayer("elim1", new List<Tile>());
            elim1.setPosn(new Posn(0, 0, 2));
            elim1.playerState = SPlayer.State.Playing;
            SPlayer elim2 = new SPlayer("elim2", new List<Tile>());
            elim2.setPosn(new Posn(0, 0, 3));
            elim2.playerState = SPlayer.State.Playing;

            b.registerPlayer(elim1);
            b.registerPlayer(elim2);

            TurnResult tr = a.playATurn(b, t1);

            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.returnColor() == "elim1"), "eliminated player is in eliminated list");
            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.returnColor() == "elim2"), "eliminated player is in eliminated list");

            Assert.IsNotNull(tr.playResult);
            Assert.IsTrue(tr.playResult.Exists(x => x.returnColor() == "elim1"), "elim1 not in the winning list of players");
            Assert.IsTrue(tr.playResult.Exists(x => x.returnColor() == "elim2"), "elim2 not in the winning list of players");
            Assert.AreEqual(tr.playResult.Count, 2);
        }



    }
}
