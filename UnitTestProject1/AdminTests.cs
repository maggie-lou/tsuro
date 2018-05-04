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
        static Path first1 = new Path(0, 1);
        static Path second1 = new Path(2, 4);
        static Path third1 = new Path(3, 6);
        static Path fourth1 = new Path(5, 7);

        static List<Path> path1 = new List<Path>()
        {
            first1,
            second1,
            third1,
            fourth1
        };

        static Path first2 = new Path(0, 6);
        static Path second2 = new Path(1, 5);
        static Path third2 = new Path(2, 4);
        static Path fourth2 = new Path(3, 7);

        static List<Path> path2 = new List<Path>()
        {
            first2,
            second2,
            third2,
            fourth2
        };

        static Path first3 = new Path(0, 5);
        static Path second3 = new Path(1, 4);
        static Path third3 = new Path(2, 7);
        static Path fourth3 = new Path(3, 6);

        static List<Path> path3 = new List<Path>()
        {
            first3,
            second3,
            third3,
            fourth3
        };

        static Path first4 = new Path(0, 1);
        static Path second4 = new Path(2, 7);
        static Path third4 = new Path(4, 5);
        static Path fourth4 = new Path(3, 6);

        static List<Path> path4 = new List<Path>()
        {
            first4,
            second4,
            third4,
            fourth4
        };

        [TestMethod]
        public void TileNotInEmptyHand()
        {
            Tile t1 = new Tile(path1);
            Admin a = new Admin();
            Board b = new Board();
            SPlayer player1 = new SPlayer("blue", new List<Tile>(), false);

            Assert.IsFalse(a.tileInHand(player1, t1));
        }

        [TestMethod]
        public void TileInHandNotRotated()
        {
            Tile t1 = new Tile(path1);
            Tile t2 = new Tile(path2);
            Tile t3 = new Tile(path3);
            Admin a = new Admin();
            Board b = new Board();

            List<Tile> hand = new List<Tile>()
            {
                t1,
                t2,
                t3
            };

            SPlayer player1 = new SPlayer("blue", hand, false);
            Assert.IsTrue(a.tileInHand(player1, t1));
        }

        [TestMethod]
        public void TileInHandRotated()
        {
            Tile t1 = new Tile(path1);
            Tile t2 = new Tile(path2);
            Tile t3 = new Tile(path3);
            Admin a = new Admin();
            Board b = new Board();

            List<Tile> hand = new List<Tile>()
            {
                t1,
                t2,
                t3
            };

            SPlayer player1 = new SPlayer("blue", hand, false);

            Tile t1_rotated = t1;
            t1_rotated.rotate();

            Assert.IsTrue(a.tileInHand(player1, t1_rotated));

        }

        [TestMethod]
        public void drawATileDrawsTile()
        {
            Tile t1 = new Tile(path1);
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
        public void PlayATurnWithNoPlayers()
        {
            Tile t1 = new Tile(path1);
            Admin a = new Admin();
            Board b = new Board();
            List<Tile> drawpile = new List<Tile>();
            List<SPlayer> l1 = new List<SPlayer>();
            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult noturnplayed = a.playATurn(drawpile, l1, l2, b, t1);
            Assert.IsTrue(noturnplayed.playResult == null);
        }

        [TestMethod]
        public void PlayAValidTurnRemovesTileFromDrawPile()
        {
            Tile t1 = new Tile(path1);
            Tile t2 = new Tile(path2);
            Tile t3 = new Tile(path3);

            Admin a = new Admin();
            Board b = new Board();
            List<Tile> drawpile = new List<Tile>()
            {
                t2,t3
            };

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            Posn p1Pos = new Posn(0, 0, 3);
            p1.setPosn(p1Pos);
            SPlayer p2 = new SPlayer("p2", new List<Tile>(), true);
            Posn p2Pos = new Posn(4, 4, 0);
            p2.setPosn(p2Pos);

            b.registerPlayer(p1);
            b.registerPlayer(p2);

            List<SPlayer> l1 = new List<SPlayer>()
            {
                p1,p2
            };

            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult tmpturn = a.playATurn(drawpile, l1, l2, b, t1);

            Assert.IsTrue(tmpturn.drawPile.Count == 1);
            Assert.IsFalse(tmpturn.drawPile.Exists(x => x.isEqual(t2)));

            List<Tile> hand = tmpturn.currentPlayers[1].returnHand();

            Assert.IsTrue(hand.Exists(x => x.isEqual(t2)));

        }

        [TestMethod]
        public void PlayAValidTurnChangesOrderOfInGamePlayers()
        {
            Tile t1 = new Tile(path1);
            Tile t2 = new Tile(path2);
            Tile t3 = new Tile(path3);

            Admin a = new Admin();
            Board b = new Board();
            List<Tile> drawpile = new List<Tile>()
            {
                t2,t3
            };

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            Posn p1Pos = new Posn(0, 0, 3);
            p1.setPosn(p1Pos);
            SPlayer p2 = new SPlayer("p2", new List<Tile>(), true);
            Posn p2Pos = new Posn(4, 4, 0);
            p2.setPosn(p2Pos);

            b.registerPlayer(p1);
            b.registerPlayer(p2);

            List<SPlayer> l1 = new List<SPlayer>()
            {
                p1,p2
            };

            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult tmpturn = a.playATurn(drawpile, l1, l2, b, t1);

            Assert.IsTrue(tmpturn.currentPlayers[0].returnColor() == "p2");
            Assert.IsTrue(tmpturn.currentPlayers[1].returnColor() == "p1");
            Assert.IsTrue(tmpturn.currentPlayers.Count == 2);
        }

        [TestMethod]
        public void ValidTurnCausePlayerToBeEliminated()
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

            Admin a = new Admin();
            Board b = new Board();
            List<Tile> drawpile = new List<Tile>()
            {
                t2,t3
            };

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            Posn p1Pos = new Posn(0, 1, 6);
            p1.setPosn(p1Pos);
            SPlayer p2 = new SPlayer("p2", new List<Tile>(), true);
            Posn p2Pos = new Posn(4,4,0);
            p2.setPosn(p2Pos);

            b.registerPlayer(p1);
            b.registerPlayer(p2);

            List<SPlayer> l1 = new List<SPlayer>()
            {
                p1,p2
            };

            List<SPlayer> l2 = new List<SPlayer>();

            TurnResult tmpturn = a.playATurn(drawpile, l1, l2, b, t1);

            Assert.IsTrue(tmpturn.eliminatedPlayers.Count == 1, "count of eliminated players has not increased to 1");
            Assert.IsTrue(tmpturn.eliminatedPlayers.Exists(x => x.returnColor() == "p1"), "p1 has not been moved to eliminated players");
            Assert.IsFalse(tmpturn.currentPlayers.Exists(x => x.returnColor() == "p1"), "p1 has not been removed from current players");
            Assert.IsTrue(tmpturn.currentPlayers.Count == 1, "count of current players has not decreased to 1");
        }

        [TestMethod]
        public void MakingAMoveFromTheEdge()
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

            Tile t1 = new Tile(path1);
            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), false);
            p1.setPosn(new Posn(0, 0, 6));

            b.registerPlayer(p1);

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t1);
            Posn playerPosn = tr.currentPlayers[0].getPlayerPosn();
            Assert.IsTrue(playerPosn.returnLocationOnTile() == 3);
            Assert.IsTrue(playerPosn.returnRow() == 0);
            Assert.IsTrue(playerPosn.returnCol() == 0);
        }

        [TestMethod]
        public void MakeAMoveCauseTokenToCrossMultipleTiles()
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
            Path first4 = new Path(0, 1);
            Path second4 = new Path(2, 7);
            Path third4 = new Path(4, 5);
            Path fourth4 = new Path(3, 6);

            List<Path> path4 = new List<Path>()
            {
                first4,
                second4,
                third4,
                fourth4
            };
            Tile t1 = new Tile(path1);
            Tile t2 = new Tile(path2);
            Tile t3 = new Tile(path3);
            Tile t4 = new Tile(path4);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            p1.setPosn(new Posn(1, 1, 3));

            b.registerPlayer(p1);

            b.grid[1, 1] = t2;
            b.grid[1, 3] = t1;
            b.grid[1, 4] = t3;

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t4);
            Posn playerPosn = tr.currentPlayers[0].getPlayerPosn();
            Assert.IsTrue(playerPosn.returnRow() == 1);
            Assert.IsTrue(playerPosn.returnCol() == 4);
            Assert.IsTrue(playerPosn.returnLocationOnTile() == 3);
        }

        [TestMethod]
        public void MakeAMoveCauseMultiplePlayersToMove()
        {
            Path first1 = new Path(0, 2);
            Path second1 = new Path(1, 6);
            Path third1 = new Path(3, 5);
            Path fourth1 = new Path(4, 7);
            List<Path> path1 = new List<Path>()
            {
                first1,
                second1,
                third1,
                fourth1
            };
            Tile t1 = new Tile(path1);

            Path first2 = new Path(0, 1);
            Path second2 = new Path(2, 7);
            Path third2 = new Path(3, 4);
            Path fourth2 = new Path(5, 6);
            List<Path> path2 = new List<Path>()
            {
                first2,
                second2,
                third2,
                fourth2
            };
            Tile t2 = new Tile(path2);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            p1.setPosn(new Posn(1, 0, 2));

            SPlayer p2 = new SPlayer("p2", new List<Tile>(), true);
            p2.setPosn(new Posn(0, 1, 5));

            SPlayer p3 = new SPlayer("p3", new List<Tile>(), true);
            p3.setPosn(new Posn(1, 2, 6));

            b.registerPlayer(p1);
            b.registerPlayer(p2);
            b.registerPlayer(p3);

            b.grid[1, 2] = t2;

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t1);
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

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            p1.setPosn(new Posn(1, 1, 3));
            b.registerPlayer(p1);
            inGame.Add(p1);

            Path first1 = new Path(0, 3);
            Path second1 = new Path(6, 4);
            Path third1 = new Path(7, 2);
            Path fourth1 = new Path(1, 5);
            List<Path> path1 = new List<Path>()
            {
                first1,
                second1,
                third1,
                fourth1
            };
            Tile t1 = new Tile(path1);

            Tile rotatedTile = t1.rotate();

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, rotatedTile);
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
            //tile to be placed
            Path first1 = new Path(7, 0);
            Path second1 = new Path(6, 1);
            Path third1 = new Path(5, 4);
            Path fourth1 = new Path(2, 3);
            List<Path> path1 = new List<Path>()
            {
                first1,
                second1,
                third1,
                fourth1
            };
            Tile t1 = new Tile(path1);
            //tile the player is on
            Path first2 = new Path(1, 3);
            Path second2 = new Path(0,5);
            Path third2 = new Path(2,7);
            Path fourth2 = new Path(4,6);
            List<Path> path2 = new List<Path>()
            {
                first2,
                second2,
                third2,
                fourth2
            };
            Tile t2 = new Tile(path2);

            b.grid[1, 1] = t2;

            //players to be eliminated
            SPlayer elim1 = new SPlayer("elim1", new List<Tile>(), true);
            elim1.setPosn(new Posn(0, 0, 2));
            SPlayer elim2 = new SPlayer("elim2", new List<Tile>(), true);
            elim2.setPosn(new Posn(0, 0, 3));
            //player left over
            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            p1.setPosn(new Posn(1, 1, 0));

            b.registerPlayer(p1);
            b.registerPlayer(elim1);
            b.registerPlayer(elim2);

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t1);
            Posn playerPosn = tr.currentPlayers[0].getPlayerPosn();

            Assert.AreEqual(playerPosn.returnLocationOnTile(), 3,"remaining player not at location 3 on tile");
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
            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            p1.setPosn(new Posn(3, 3, 1));

            b.registerPlayer(p1);

            //tile to be placed
            Path first1 = new Path(7, 0);
            Path second1 = new Path(6, 1);
            Path third1 = new Path(5, 4);
            Path fourth1 = new Path(2, 3);
            List<Path> path1 = new List<Path>()
            {
                first1,
                second1,
                third1,
                fourth1
            };
            Tile t1 = new Tile(path1);

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t1);

            Assert.IsTrue(tr.b.returnDragonTileHolder().returnColor() == "p1");
        }

        [TestMethod]
        public void DragonTileBeforeTurnStillNoNewTiles()
        {
            Path first1 = new Path(0, 2);
            Path second1 = new Path(1, 6);
            Path third1 = new Path(3, 5);
            Path fourth1 = new Path(4, 7);
            List<Path> path1 = new List<Path>()
            {
                first1,
                second1,
                third1,
                fourth1
            };
            Tile t1 = new Tile(path1);

            Path first2 = new Path(0, 1);
            Path second2 = new Path(2, 7);
            Path third2 = new Path(3, 4);
            Path fourth2 = new Path(5, 6);
            List<Path> path2 = new List<Path>()
            {
                first2,
                second2,
                third2,
                fourth2
            };
            Tile t2 = new Tile(path2);

            Admin a = new Admin();
            Board b = new Board();

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            p1.setPosn(new Posn(1, 0, 2));

            SPlayer p2 = new SPlayer("p2", new List<Tile>(), true);
            p2.setPosn(new Posn(0, 1, 5));

            SPlayer p3 = new SPlayer("p3", new List<Tile>(), true);
            p3.setPosn(new Posn(1, 2, 6));

            b.registerPlayer(p1);
            b.registerPlayer(p2);
            b.registerPlayer(p3);

            b.grid[1, 2] = t2;

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t1);

            Assert.IsTrue(tr.b.returnDragonTileHolder().returnColor() == "p1");
            Assert.IsTrue(tr.currentPlayers[0].returnHand().Count == 0);
            Assert.IsTrue(tr.currentPlayers[1].returnHand().Count == 0);
            Assert.IsTrue(tr.currentPlayers[2].returnHand().Count == 0);
        }

        [TestMethod]
        public void PlayerHasDragonTileCausesPlayerToBeEliminated()
        {
            Admin a = new Admin();
            Board b = new Board();
            //tile to be placed
            Path first1 = new Path(7, 0);
            Path second1 = new Path(6, 1);
            Path third1 = new Path(5, 4);
            Path fourth1 = new Path(2, 3);
            List<Path> path1 = new List<Path>()
            {
                first1,
                second1,
                third1,
                fourth1
            };
            Tile t1 = new Tile(path1);
            //tile the player is on
            Path first2 = new Path(1, 3);
            Path second2 = new Path(0, 5);
            Path third2 = new Path(2, 7);
            Path fourth2 = new Path(4, 6);
            List<Path> path2 = new List<Path>()
            {
                first2,
                second2,
                third2,
                fourth2
            };
            Tile t2 = new Tile(path2);

            Path first3 = new Path(0, 1);
            Path second3 = new Path(2, 3);
            Path third3 = new Path(4, 5);
            Path fourth3 = new Path(6, 7);
            List<Path> path3 = new List<Path>()
            {
                first3,
                second3,
                third3,
                fourth3
            };
            Tile t3 = new Tile(path3);

            Path first4 = new Path(1, 2);
            Path second4 = new Path(3, 4);
            Path third4 = new Path(5, 6);
            Path fourth4 = new Path(7, 0);
            List<Path> path4 = new List<Path>()
            {
                first4,
                second4,
                third4,
                fourth4
            };
            Tile t4 = new Tile(path4);

            b.grid[1, 1] = t2;

            //players to be eliminated
            List<Tile> elim1Tiles = new List<Tile>() { t3, t4 };
            SPlayer elim1 = new SPlayer("elim1", elim1Tiles, true);
            elim1.setPosn(new Posn(0, 0, 2));

            //players left over
            SPlayer p2 = new SPlayer("p2", new List<Tile>(), true);
            p2.setPosn(new Posn(4, 4, 3));
            //List<Tile> elim2Tiles = new List<Tile>();

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            p1.setPosn(new Posn(1, 1, 0));

            b.registerPlayer(p1);
            b.registerPlayer(elim1);
            b.registerPlayer(p2);

            b.setDragonTileHolder(p1);

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t1);

            Assert.IsTrue(tr.currentPlayers[0].returnHand()[0].isEqual(t4));
            Assert.IsTrue(tr.currentPlayers[1].returnHand()[0].isEqual(t3));

        }

        [TestMethod]
        public void PlayerWithoutDragonTileCausesDragonTileHolderToBeEliminated()
        {
            Admin a = new Admin();
            Board b = new Board();
            //tile to be placed
            Path first1 = new Path(7, 0);
            Path second1 = new Path(6, 1);
            Path third1 = new Path(5, 4);
            Path fourth1 = new Path(2, 3);
            List<Path> path1 = new List<Path>()
            {
                first1,
                second1,
                third1,
                fourth1
            };
            Tile t1 = new Tile(path1);
            //tile the player is on
            Path first2 = new Path(1, 3);
            Path second2 = new Path(0, 5);
            Path third2 = new Path(2, 7);
            Path fourth2 = new Path(4, 6);
            List<Path> path2 = new List<Path>()
            {
                first2,
                second2,
                third2,
                fourth2
            };
            Tile t2 = new Tile(path2);

            Path first3 = new Path(0, 1);
            Path second3 = new Path(2, 3);
            Path third3 = new Path(4, 5);
            Path fourth3 = new Path(6, 7);
            List<Path> path3 = new List<Path>()
            {
                first3,
                second3,
                third3,
                fourth3
            };
            Tile t3 = new Tile(path3);

            Path first4 = new Path(1, 2);
            Path second4 = new Path(3, 4);
            Path third4 = new Path(5, 6);
            Path fourth4 = new Path(7, 0);
            List<Path> path4 = new List<Path>()
            {
                first4,
                second4,
                third4,
                fourth4
            };
            Tile t4 = new Tile(path4);

            b.grid[1, 1] = t2;

            //players to be eliminated
            List<Tile> elim1Tiles = new List<Tile>() { t3, t4 };
            SPlayer elim1 = new SPlayer("elim1", elim1Tiles, true);
            elim1.setPosn(new Posn(0, 0, 2));

            //players left over
            SPlayer p2 = new SPlayer("p2", new List<Tile>(), true);
            p2.setPosn(new Posn(4, 4, 3));
            //List<Tile> elim2Tiles = new List<Tile>();

            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            p1.setPosn(new Posn(1, 1, 0));

            b.registerPlayer(p1);
            b.registerPlayer(elim1);
            b.registerPlayer(p2);

            b.setDragonTileHolder(elim1);

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t1);

            Assert.IsNull(tr.b.returnDragonTileHolder());
        }

        [TestMethod]
        public void DragonTileHolderEliminatesSelf()
        {
            Admin a = new Admin();
            Board b = new Board();
            //tile to be placed
            Path first1 = new Path(7, 0);
            Path second1 = new Path(6, 1);
            Path third1 = new Path(5, 4);
            Path fourth1 = new Path(2, 3);
            List<Path> path1 = new List<Path>()
            {
                first1,
                second1,
                third1,
                fourth1
            };
            Tile t1 = new Tile(path1);
            //tile the player is on
            Path first2 = new Path(1, 3);
            Path second2 = new Path(0, 5);
            Path third2 = new Path(2, 7);
            Path fourth2 = new Path(4, 6);
            List<Path> path2 = new List<Path>()
            {
                first2,
                second2,
                third2,
                fourth2
            };
            Tile t2 = new Tile(path2);

            Path first3 = new Path(0, 1);
            Path second3 = new Path(2, 3);
            Path third3 = new Path(4, 5);
            Path fourth3 = new Path(6, 7);
            List<Path> path3 = new List<Path>()
            {
                first3,
                second3,
                third3,
                fourth3
            };
            Tile t3 = new Tile(path3);

            Path first4 = new Path(1, 2);
            Path second4 = new Path(3, 4);
            Path third4 = new Path(5, 6);
            Path fourth4 = new Path(7, 0);
            List<Path> path4 = new List<Path>()
            {
                first4,
                second4,
                third4,
                fourth4
            };
            Tile t4 = new Tile(path4);

            b.grid[1, 1] = t2;

            //pnot being eliminated
            List<Tile> elim1Tiles = new List<Tile>() { t3, t4 };
            SPlayer elim1 = new SPlayer("elim1", elim1Tiles, true);
            elim1.setPosn(new Posn(1, 1, 0));

            //players left over
            SPlayer p2 = new SPlayer("p2", new List<Tile>(), true);
            p2.setPosn(new Posn(4, 4, 3));

            //getting eliminated
            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            p1.setPosn(new Posn(0, 0, 2));

            b.registerPlayer(p1);
            b.registerPlayer(elim1);
            b.registerPlayer(p2);

            b.setDragonTileHolder(p1);

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t1);

            Assert.IsNull(tr.b.returnDragonTileHolder());
        }
        [TestMethod]
        [DeploymentItem("drawPilepaths.txt")]
        public void InitializeDrawPile()
        {
            Path first3 = new Path(0, 1);
            Path second3 = new Path(2, 3);
            Path third3 = new Path(4, 5);
            Path fourth3 = new Path(6, 7);
            List<Path> path3 = new List<Path>()
            {
                first3,
                second3,
                third3,
                fourth3
            };
            Tile t3 = new Tile(path3);

            Admin a = new Admin();

            List<Tile> drawPile = a.initializeDrawPile("drawPilepaths.txt");

            Assert.IsTrue(drawPile[0].isEqual(t3));
        }
        [TestMethod]
        [DeploymentItem("drawPilepaths.txt")]
        public void DealTilesAtTheBeginningOfAGame()
        {
            Admin a = new Admin();
            List<Tile> drawPile = a.initializeDrawPile("drawPilepaths.txt");
            Board b = new Board();
            b.drawPile = drawPile;

            SPlayer rp1 = new SPlayer("blue", new List<Tile>(), true, "Random");
            rp1.placePawn(b);

            SPlayer rp2 = new SPlayer("hotpink", new List<Tile>(), true, "Random");
            rp2.placePawn(b);

            a.dealTiles(b);

            Assert.AreEqual(b.drawPile.Count, 29);
        }

        [TestMethod]
        public void TwoPlayerGameWherePlayerEliminatesHimselfAndOtherPlayerWins()
        {
            Admin a = new Admin();
            Board b = new Board();
            //tile to be placed
            Path first1 = new Path(7, 0);
            Path second1 = new Path(6, 1);
            Path third1 = new Path(5, 4);
            Path fourth1 = new Path(2, 3);
            List<Path> path1 = new List<Path>()
            {
                first1,
                second1,
                third1,
                fourth1
            };
            Tile t1 = new Tile(path1);
            //tile the player is on
            Path first2 = new Path(1, 3);
            Path second2 = new Path(0, 5);
            Path third2 = new Path(2, 7);
            Path fourth2 = new Path(4, 6);
            List<Path> path2 = new List<Path>()
            {
                first2,
                second2,
                third2,
                fourth2
            };
            Tile t2 = new Tile(path2);

            b.grid[1, 1] = t2;

            //players to be eliminated
            SPlayer elim1 = new SPlayer("elim1", new List<Tile>(), true);
            elim1.setPosn(new Posn(0, 0, 2));

            ////player left over
            SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            p1.setPosn(new Posn(1, 1, 0));

            b.registerPlayer(elim1);
            b.registerPlayer(p1);

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t1);

            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.returnColor() == "elim1"), "eliminated player is in eliminated list");
            Assert.IsTrue(tr.currentPlayers.Exists(x => x.returnColor() == "p1"), "p1 is not inGamePlayers list");


            Assert.IsNotNull(tr.playResult);
            Assert.IsTrue(tr.playResult.Exists(x => x.returnColor() == "p1"), "p1 not in the winning list of players");
            Assert.AreEqual(tr.playResult.Count, 1);
        }

        [TestMethod]
        public void TwoPlayerGameWhereBothTie()
        {
            Admin a = new Admin();
            Board b = new Board();
            //tile to be placed
            Path first1 = new Path(7, 0);
            Path second1 = new Path(6, 1);
            Path third1 = new Path(5, 4);
            Path fourth1 = new Path(2, 3);
            List<Path> path1 = new List<Path>()
            {
                first1,
                second1,
                third1,
                fourth1
            };
            Tile t1 = new Tile(path1);
            //tile the player is on
            Path first2 = new Path(1, 3);
            Path second2 = new Path(0, 5);
            Path third2 = new Path(2, 7);
            Path fourth2 = new Path(4, 6);
            List<Path> path2 = new List<Path>()
            {
                first2,
                second2,
                third2,
                fourth2
            };
            Tile t2 = new Tile(path2);

            b.grid[1, 1] = t2;

            //players to be eliminated
            SPlayer elim1 = new SPlayer("elim1", new List<Tile>(), true);
            elim1.setPosn(new Posn(0, 0, 2));
            SPlayer elim2 = new SPlayer("elim2", new List<Tile>(), true);
            elim2.setPosn(new Posn(0, 0, 3));
            ////player left over
            //SPlayer p1 = new SPlayer("p1", new List<Tile>(), true);
            //p1.setPosn(1, 1, 0);

            //b.registerPlayer(p1);
            b.registerPlayer(elim1);
            b.registerPlayer(elim2);

            TurnResult tr = a.playATurn(new List<Tile>(), b.returnOnBoard(), b.returnEliminated(), b, t1);

            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.returnColor() == "elim1"), "eliminated player is in eliminated list");
            Assert.IsTrue(tr.eliminatedPlayers.Exists(x => x.returnColor() == "elim2"), "eliminated player is in eliminated list");

            Assert.IsNotNull(tr.playResult);
            Assert.IsTrue(tr.playResult.Exists(x => x.returnColor() == "elim1"), "elim1 not in the winning list of players");
            Assert.IsTrue(tr.playResult.Exists(x => x.returnColor() == "elim2"), "elim2 not in the winning list of players");
            Assert.AreEqual(tr.playResult.Count, 2);
        }

    }
}
