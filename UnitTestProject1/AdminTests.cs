using System;
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

            a.addTileToDrawPile(t1);
            
            Tile tcheck = a.drawTile();

            Assert.IsTrue(tcheck.isEqualOrRotation(t1));
			Assert.AreEqual(0, a.getDrawPileSize());
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

			a.addToActivePlayers(p1);
			a.addToActivePlayers(p2);
			a.addToActivePlayers(p3);

			Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

			TurnResult tr = a.playATurn(b, t1);
            
			Assert.AreEqual(3, tr.currentPlayers.Count);
			Assert.AreEqual("hotpink", tr.currentPlayers[0].getColor());
			Assert.AreEqual("green", tr.currentPlayers[1].getColor());
			Assert.AreEqual("blue", tr.currentPlayers[2].getColor());
	
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
			a.addToActivePlayers(p1);
            a.addToActivePlayers(p2);
            a.addToActivePlayers(p3);

            Tile t1 = test.makeTile(0, 3, 6, 1, 2, 5, 4, 7);

            TurnResult tr = a.playATurn(b, t1);
            
            Assert.AreEqual(2, tr.currentPlayers.Count);
			Assert.AreEqual(1, tr.eliminatedPlayers.Count);
			Assert.AreEqual("hotpink", tr.eliminatedPlayers[0].getColor());
            Assert.AreEqual("green", tr.currentPlayers[0].getColor());
            Assert.AreEqual("blue", tr.currentPlayers[1].getColor());
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
			a.addToActivePlayers(p1);
            a.addToActivePlayers(p2);
            a.addToActivePlayers(p3);

			a.setDragonTileHolder(p2);

            Tile t1 = test.makeTile(0, 3, 6, 1, 2, 5, 4, 7);

            TurnResult tr = a.playATurn(b, t1);

			Assert.AreEqual(0, p2.getHand().Count);
		}

        [TestMethod]
        public void PlayAValidTurnRemovesTileFromDrawPile()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile t3 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);

			List<Tile> drawpile = test.makeDrawPile(t2, t3);         
			Admin a = test.createAdminWithDrawPile(drawpile);
			Board b = new Board();

            SPlayer p1 = new SPlayer("blue", new List<Tile>());
            Posn p1Pos = new Posn(0, 0, 3);
			b.addPlayerToBoard(p1.getColor(), p1Pos);

            SPlayer p2 = new SPlayer("hotpink", new List<Tile>());
            Posn p2Pos = new Posn(4, 4, 0);
            b.addPlayerToBoard(p2.getColor(), p2Pos);

            a.addToActivePlayers(p1);
            a.addToActivePlayers(p2);

            List<SPlayer> l1 = new List<SPlayer>()
            {
                p1,p2
            };

            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult tmpturn = a.playATurn(b, t1);

            Assert.AreEqual(1, tmpturn.drawPile.Count);
            Assert.IsFalse(tmpturn.drawPile.Exists(x => x.isEqualOrRotation(t2)));

            List<Tile> hand = tmpturn.currentPlayers[1].getHand();

            Assert.IsTrue(hand.Exists(x => x.isEqualOrRotation(t2)));
        }

        [TestMethod]
        public void PlayAValidTurnChangesOrderOfInGamePlayers()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile t3 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);

			List<Tile> drawpile = test.makeDrawPile(t2, t3);
			Admin a = test.createAdminWithDrawPile(drawpile);
			Board b = new Board();
        
            SPlayer p1 = new SPlayer("blue", new List<Tile>());
            Posn p1Pos = new Posn(0, 0, 3);
            SPlayer p2 = new SPlayer("green", new List<Tile>());
            Posn p2Pos = new Posn(4, 4, 0);

			a.addToActivePlayers(p1);
            a.addToActivePlayers(p2);

			b.addPlayerToBoard("blue", p1Pos);
			b.addPlayerToBoard("green", p2Pos);

            List<SPlayer> l1 = new List<SPlayer>()
            {
                p1,p2
            };

            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult tmpturn = a.playATurn(b, t1);

            Assert.IsTrue(tmpturn.currentPlayers[0].getColor() == "green");
            Assert.IsTrue(tmpturn.currentPlayers[1].getColor() == "blue");
            Assert.IsTrue(tmpturn.currentPlayers.Count == 2);
        }

        [TestMethod]
        public void ValidTurnCausePlayerToBeEliminated()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile t2 = test.makeTile(0, 6, 1, 5, 2, 4, 3, 7);
            Tile t3 = test.makeTile(0, 5, 1, 4, 2, 7, 3, 6);

			List<Tile> drawpile = test.makeDrawPile(t2, t3);
			Admin a = test.createAdminWithDrawPile(drawpile);
            Board b = new Board();
         
            SPlayer p1 = new SPlayer("p1", new List<Tile>());
            Posn p1Pos = new Posn(0, 1, 6);
			test.setStartPos(b, p1, p1Pos);
            SPlayer p2 = new SPlayer("p2", new List<Tile>());
            Posn p2Pos = new Posn(4,4,0);
			test.setStartPos(b, p2, p2Pos);

			a.addToActivePlayers(p1);
            a.addToActivePlayers(p2);
                     
            TurnResult tmpturn = a.playATurn(b, t1);

			Assert.AreEqual(1, a.numEliminated(), "count of eliminated players has not increased to 1");
            Assert.IsTrue(tmpturn.eliminatedPlayers.Count == 1, "count of eliminated players has not increased to 1");
            Assert.IsTrue(tmpturn.eliminatedPlayers.Exists(x => x.getColor() == "p1"), "p1 has not been moved to eliminated players");
            Assert.IsFalse(tmpturn.currentPlayers.Exists(x => x.getColor() == "p1"), "p1 has not been removed from current players");
            Assert.IsTrue(tmpturn.currentPlayers.Count == 1, "count of current players has not decreased to 1");
        }

        [TestMethod]
        public void MakingAMoveFromTheEdge()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);

            Admin a = new Admin();
            Board b = new Board();

			SPlayer p1 = new SPlayer("blue", new List<Tile>());
            Posn p1Pos = new Posn(0, -1, 3);
			b.addPlayerToBoard(p1.getColor(), p1Pos);
            
            a.addToActivePlayers(p1);

            TurnResult tr = a.playATurn(b, t1);

			Posn newp1Pos = b.getPlayerPosn(a.getFirstActivePlayer().getColor());
			Assert.IsTrue(new Posn(0, 0, 3).isEqual(newp1Pos));
            //Posn playerPosn = tr.currentPlayers[0].getPlayerPosn();
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

            SPlayer p1 = new SPlayer("blue", new List<Tile>());
			b.addPlayerToBoard(p1.getColor(), new Posn(1, 1, 3));

            a.addToActivePlayers(p1);

			b.placeTileAt(t2, 1, 1);
			b.placeTileAt(t1, 1, 3);
			b.placeTileAt(t3, 1, 4);
           
            TurnResult tr = a.playATurn(b, t4);
			Posn playerPosn = b.getPlayerPosn(tr.currentPlayers[0].getColor());
			Assert.IsTrue(new Posn(1, 4, 3).isEqual(playerPosn));
        }

		[TestMethod]
		public void Player1MoveCausesPlayer2MovementBeforeFirstTurn() {
			TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 2, 1, 5, 3, 7, 4, 6);
			Board board = new Board();
			Admin admin = new Admin();
            
			SPlayer p1 = new SPlayer("blue", new List<Tile>(), new RandomPlayer());
            board.addPlayerToBoard(p1.getColor(), new Posn(5, 6, 7));

			SPlayer p2 = new SPlayer("green", new List<Tile>(), new RandomPlayer());
			board.addPlayerToBoard(p2.getColor(), new Posn(5, 6, 6));

			admin.addToActivePlayers(p1);
            admin.addToActivePlayers(p2);

			TurnResult tr = admin.playATurn(board, t1);
            
			Posn p1EndPosExpected = new Posn(5, 5, 0);
			Posn p2EndPosExpected = new Posn(5, 5, 7);

			Posn p1EndPosActual = board.getPlayerPosn(tr.currentPlayers[1].getColor());
			Posn p2EndPosActual = board.getPlayerPosn(tr.currentPlayers[0].getColor());


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

            SPlayer p1 = test.createPlayerAtPos("blue", new List<Tile>(), new RandomPlayer(), new Posn(5, 6, 7), board);

			SPlayer p2 = test.createPlayerAtPos("green", new List<Tile>(), new RandomPlayer(), new Posn(5, 6, 6), board);

			admin.addToActivePlayers(p1);
			admin.addToActivePlayers(p2);
         
            TurnResult tr = admin.playATurn(board, t1);

            Posn p1EndPosExpected = new Posn(5, 5, 0);
            
			Posn p1EndPosActual = board.getPlayerPosn(tr.currentPlayers[0].getColor());     

			Assert.IsTrue(p1EndPosExpected.isEqual(p1EndPosActual));
			Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.getColor() == "green"));
			Assert.IsTrue(tr.playResult.Exists(x => x.getColor() == "blue"));
        }
        
        [TestMethod]
        public void MakeAMoveCauseMultiplePlayersToMove()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 2, 1, 6, 3, 5, 4, 7);
            Tile t2 = test.makeTile(0, 1, 2, 7, 3, 4, 5, 6);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("blue", new List<Tile>());
			b.addPlayerToBoard(p1.getColor(), new Posn(1, 0, 2));

            SPlayer p2 = new SPlayer("green", new List<Tile>());
			b.addPlayerToBoard(p2.getColor(), new Posn(0, 1, 5));

            SPlayer p3 = new SPlayer("hotpink", new List<Tile>());
			b.addPlayerToBoard(p3.getColor(), new Posn(1, 2, 6));

            a.addToActivePlayers(p1);
            a.addToActivePlayers(p2);
            a.addToActivePlayers(p3);

			b.placeTileAt(t2, 1, 2);

            TurnResult tr = a.playATurn(b, t1);
			Posn playerPosn0 = b.getPlayerPosn(tr.currentPlayers[0].getColor());
			Posn playerPosn1 = b.getPlayerPosn(tr.currentPlayers[1].getColor());
			Posn playerPosn2 = b.getPlayerPosn(tr.currentPlayers[2].getColor());

			Assert.IsTrue(playerPosn0.isEqual(new Posn(1, 2, 2)));
			Assert.IsTrue(playerPosn1.isEqual(new Posn(1, 1, 5)));
			Assert.IsTrue(playerPosn2.isEqual(new Posn(1, 1, 4)));

            Assert.IsNull(tr.playResult);
        }


        [TestMethod]
        public void MakeAMoveWhenTileIsRotated()
        {
            Admin a = new Admin();
            Board b = new Board();
            List<SPlayer> inGame = new List<SPlayer>();

            SPlayer p1 = new SPlayer("blue", new List<Tile>());
			b.addPlayerToBoard(p1.getColor(), new Posn(1, 1, 3));
            a.addToActivePlayers(p1);
            inGame.Add(p1);

            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 3, 6, 4, 7, 2, 1, 5);

            Tile rotatedTile = t1.rotate();

            TurnResult tr = a.playATurn(b, rotatedTile);
			Posn playerPosn = b.getPlayerPosn(tr.currentPlayers[0].getColor());
			Assert.IsTrue(playerPosn.isEqual(new Posn(1, 2, 0)));
            Assert.IsTrue(tr.currentPlayers.Exists(x => x.getColor() == "blue"),"p1 not in winning players");
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
            SPlayer elim1 = new SPlayer("blue", new List<Tile>());
			b.addPlayerToBoard(elim1.getColor(), new Posn(0, 0, 2));
            elim1.playerState = SPlayer.State.Playing;
            SPlayer elim2 = new SPlayer("green", new List<Tile>());
			b.addPlayerToBoard(elim2.getColor(), new Posn(0, 0, 3));
            elim2.playerState = SPlayer.State.Playing;
            //player left over
            SPlayer p1 = new SPlayer("hotpink", new List<Tile>());
			b.addPlayerToBoard(p1.getColor(), new Posn(1, 1, 0));
            p1.playerState = SPlayer.State.Playing;

            a.addToActivePlayers(elim1);
            a.addToActivePlayers(elim2);
			a.addToActivePlayers(p1);

            TurnResult tr = a.playATurn(b, t1);
			Posn playerPosn = b.getPlayerPosn(tr.currentPlayers[0].getColor());

			Assert.IsTrue(playerPosn.isEqual(new Posn(1, 1, 3)));
            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.getColor() == "blue"),"eliminated player is in eliminated list");
            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.getColor() == "green"), "eliminated player is in eliminated list");

            Assert.IsNotNull(tr.playResult);
            Assert.IsTrue(tr.playResult.Exists(x => x.getColor() == "hotpink"), "p1 not in the winning list of players");
            Assert.AreEqual(tr.playResult.Count, 1);
        }

        [TestMethod]
        public void PlayerTakesDragonTile()
        {
			TestScenerios test = new TestScenerios();
            Admin a = new Admin();
            Board b = new Board();

			SPlayer p1 = test.createPlayerAtPos("blue", new List<Tile>(), new RandomPlayer(),
												new Posn(3, 3, 1), b);
			SPlayer p2 = test.createPlayerAtPos("green", new List<Tile>(), new RandomPlayer(),
                                                new Posn(4, 3, 1), b);
			a.addToActivePlayers(p1);
			a.addToActivePlayers(p2);
                                                
            //tile to be placed
            Tile t1 = test.makeTile(7, 0, 6, 1, 5, 4, 2, 3);

            TurnResult tr = a.playATurn(b, t1);

			Assert.IsTrue(a.isDragonHolder("blue"));
        }

        [TestMethod]
        public void DragonTileBeforeTurnStillNoNewTiles()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 2, 1, 6, 3, 5, 4, 7);
            Tile t2 = test.makeTile(0, 1, 2, 7, 3, 4, 5, 6);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("blue", new List<Tile>());
			b.addPlayerToBoard(p1.getColor(), new Posn(1,0,2));


            SPlayer p2 = new SPlayer("green", new List<Tile>());
			b.addPlayerToBoard(p2.getColor(), new Posn(0,1,5));

            SPlayer p3 = new SPlayer("hotpink", new List<Tile>());
			b.addPlayerToBoard(p3.getColor(), new Posn(1,2,6));

            a.addToActivePlayers(p1);
            a.addToActivePlayers(p2);
            a.addToActivePlayers(p3);

			b.placeTileAt(t2, 1, 2);
            
            TurnResult tr = a.playATurn(b, t1);

			Assert.IsTrue(a.isDragonHolder("blue"));
            Assert.IsTrue(tr.currentPlayers[0].getHand().Count == 0);
            Assert.IsTrue(tr.currentPlayers[1].getHand().Count == 0);
            Assert.IsTrue(tr.currentPlayers[2].getHand().Count == 0);
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

			b.placeTileAt(t2, 1, 1);

            //players to be eliminated
            List<Tile> elim1Tiles = new List<Tile>() { t3, t4 };
            SPlayer elim1 = new SPlayer("blue", elim1Tiles);
			b.addPlayerToBoard(elim1.getColor(), new Posn(0, 0, 2));
            elim1.playerState = SPlayer.State.Playing;

            //players left over
            SPlayer p2 = new SPlayer("green", new List<Tile>());
			b.addPlayerToBoard(p2.getColor(), new Posn(4, 4, 3));
            p2.playerState = SPlayer.State.Playing;

            SPlayer p1 = new SPlayer("hotpink", new List<Tile>());
			b.addPlayerToBoard(p1.getColor(), new Posn(1, 1, 0));
            p1.playerState = SPlayer.State.Playing;

            a.addToActivePlayers(p1);
            a.addToActivePlayers(elim1);
            a.addToActivePlayers(p2);
            
            a.setDragonTileHolder(p1);

            TurnResult tr = a.playATurn( b, t1);

			Assert.IsTrue(a.isDragonHolder("hotpink"));
            Assert.AreEqual(1, tr.eliminatedPlayers.Count);
            Assert.AreEqual(2, tr.currentPlayers.Count);
            Assert.AreEqual(1, a.numEliminated());
			Assert.AreEqual(2, a.numActive());
            Assert.AreEqual(1, p2.getHand().Count);
            Assert.AreEqual(1, p1.getHand().Count);
            Assert.IsTrue(p1.getHand().Contains(t3));
            Assert.IsTrue(p2.getHand().Contains(t4));
			Assert.AreEqual("green", a.getFirstActivePlayer().getColor());
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

			b.placeTileAt(t2, 1, 1);

            //players to be eliminated
            List<Tile> elim1Tiles = new List<Tile>() { t3, t4 };
			SPlayer elim1 = test.createPlayerAtPos("elim1", elim1Tiles, new RandomPlayer(),
												   new Posn(0, 0, 2), b);

			//players left over
			SPlayer p2 = test.createPlayerAtPos("p2", new List<Tile>(), new RandomPlayer(),
												new Posn(4, 4, 3), b);
			SPlayer p1 = test.createPlayerAtPos("p1", new List<Tile>(), new RandomPlayer(),
			                                    new Posn(1, 1, 0), b);

			a.addToActivePlayers(elim1);
			a.addToActivePlayers(p2);
			a.addToActivePlayers(p1);

            a.setDragonTileHolder(elim1);

            TurnResult tr = a.playATurn( b, t1);

			Assert.IsTrue(a.isDragonHolder("p2"));
			Assert.AreEqual(1, tr.eliminatedPlayers.Count);
			Assert.AreEqual(2, tr.currentPlayers.Count);
			Assert.AreEqual(1, a.numEliminated());
			Assert.AreEqual(2, a.numActive());
			Assert.AreEqual(1, p2.getHand().Count);
			Assert.AreEqual(1, p1.getHand().Count);
			Assert.IsTrue(p2.getHand().Contains(t3));
			Assert.IsTrue(p1.getHand().Contains(t4));         
        }

        
        [TestMethod]
        [DeploymentItem("drawPilepaths.txt")]
        public void InitializeDrawPile()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

            Admin a = new Admin();

            List<Tile> drawPile = a.initializeDrawPile("drawPilepaths.txt");

            Assert.IsTrue(drawPile[0].isEqualOrRotation(t1));
        }
        [TestMethod]
        [DeploymentItem("drawPilepaths.txt")]
        public void DealTilesAtTheBeginningOfAGame()
        {
			TestScenerios test = new TestScenerios();
            Admin a = new Admin();
			Board b = new Board();
            List<Tile> drawPile = a.initializeDrawPile("drawPilepaths.txt");

            SPlayer rp1 = new SPlayer("blue", new List<Tile>(), new RandomPlayer());
            //rp1.initialize(b);
            //rp1.placePawn(b);

            SPlayer rp2 = new SPlayer("hotpink", new List<Tile>(), new RandomPlayer());
            //rp2.initialize(b);
            //rp2.placePawn(b);

			a.dealTiles(new List<SPlayer>{rp1, rp2}, b);
            
			Assert.AreEqual(29, a.getDrawPileSize());
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

			b.placeTileAt(t2, 1, 1);

            //players to be eliminated
            SPlayer elim1 = new SPlayer("elim1", new List<Tile>());
			b.addPlayerToBoard(elim1.getColor(), new Posn(0, 0, 2));
            elim1.playerState = SPlayer.State.Playing;

            ////player left over
            SPlayer p1 = new SPlayer("p1", new List<Tile>());
			b.addPlayerToBoard(p1.getColor(), new Posn(1, 1, 0));
            p1.playerState = SPlayer.State.Playing;

            a.addToActivePlayers(elim1);
            a.addToActivePlayers(p1);

            TurnResult tr = a.playATurn(b, t1);

            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.getColor() == "elim1"), "eliminated player is in eliminated list");
            Assert.IsTrue(tr.currentPlayers.Exists(x => x.getColor() == "p1"), "p1 is not inGamePlayers list");


            Assert.IsNotNull(tr.playResult);
            Assert.IsTrue(tr.playResult.Exists(x => x.getColor() == "p1"), "p1 not in the winning list of players");
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

			b.placeTileAt(t2, 1, 1);

            //players to be eliminated
            SPlayer elim1 = new SPlayer("elim1", new List<Tile>());
			b.addPlayerToBoard(elim1.getColor(), new Posn(0, 0, 2));
            elim1.playerState = SPlayer.State.Playing;
            SPlayer elim2 = new SPlayer("elim2", new List<Tile>());
			b.addPlayerToBoard(elim2.getColor(), new Posn(0, 0, 3));
            elim2.playerState = SPlayer.State.Playing;

            a.addToActivePlayers(elim1);
            a.addToActivePlayers(elim2);

            TurnResult tr = a.playATurn(b, t1);

            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.getColor() == "elim1"), "eliminated player is in eliminated list");
            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.getColor() == "elim2"), "eliminated player is in eliminated list");

            Assert.IsNotNull(tr.playResult);
            Assert.IsTrue(tr.playResult.Exists(x => x.getColor() == "elim1"), "elim1 not in the winning list of players");
            Assert.IsTrue(tr.playResult.Exists(x => x.getColor() == "elim2"), "elim2 not in the winning list of players");
            Assert.AreEqual(tr.playResult.Count, 2);
        }

        [TestMethod]
		public void DragonTileHolderDoesNotChangeAfterPlayATurn(){
			TestScenerios test = new TestScenerios();

			Admin a = test.createAdminWithDrawPile(new List<Tile> {});
            Board b = new Board();
            Tile t1 = test.makeTile(7, 0, 6, 1, 5, 4, 2, 3);
            Tile t2 = test.makeTile(1, 3, 0, 5, 2, 7, 4, 6);
			Tile t3 = test.makeTile(2, 4, 3, 6, 5, 1, 7, 0);
            
			SPlayer p1 = test.createPlayerAtPos("red", new List<Tile> { t1, t2 }, new RandomPlayer(), new Posn(3, 4, 3), b);
			SPlayer p2 = test.createPlayerAtPos("green", new List<Tile> { t1, t2, t3}, new RandomPlayer(), new Posn(3, 4, 3), b);
			SPlayer p3 = test.createPlayerAtPos("blue", new List<Tile> { t1, t2 }, new RandomPlayer(), new Posn(2, 4, 6), b);

			a.addToActivePlayers(p1);
			a.addToActivePlayers(p2);
			a.addToActivePlayers(p3);

            
			a.setDragonTileHolder(p3);
			a.playATurn(b, t3);

			Assert.AreEqual("blue", a.getDragonTileHolder().getColor());
			Console.WriteLine(XMLEncoder.splayerToXML(p3, a));
		}
        
    }
}
