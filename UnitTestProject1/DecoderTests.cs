﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tsuro;
using System.Xml.Linq;
using System.Linq;

namespace TsuroTests
{
	[TestClass]
    public class DecoderTests
    {
        public DecoderTests()
        {
        }
        [TestMethod]
		public void XMLBoardToGrid(){
			XElement boardXML = XElement.Parse("<board>" +
                                               "<map>" +
                                               "<ent><xy><x>0</x><y>0</y></xy>" +
                                               "<tile>" +
                                               "<connect><n>0</n><n>4</n></connect>" +
                                               "<connect><n>1</n><n>5</n></connect>" +
                                               "<connect><n>2</n><n>7</n></connect>" +
                                               "<connect><n>3</n><n>6</n></connect>" +
                                               "</tile>" +
                                               "</ent>" +
			                                   "<ent><xy><x>5</x><y>3</y></xy>" +
                                               "<tile>" +
                                               "<connect><n>0</n><n>1</n></connect>" +
                                               "<connect><n>2</n><n>6</n></connect>" +
                                               "<connect><n>3</n><n>7</n></connect>" +
                                               "<connect><n>4</n><n>5</n></connect>" +
                                               "</tile>" +
                                               "</ent>" +         
                                               "</map>" +
                                               "<map>" +
                                               "<ent><color>red</color><pawn-loc><h></h><n>0</n><n>0</n></pawn-loc>" +
                                               "</ent></map></board>");
			Tile[,] actualBoardGrid = XMLDecoder.xmlBoardToGrid(boardXML);
			Tile[,] expectedBoardGrid = new Tile[6, 6];
			TestScenerios test = new TestScenerios();
			Tile t1 = test.makeTile(0, 4, 1, 5, 2, 7, 3, 6);
			Tile t2 = test.makeTile(0, 1, 2, 6, 3, 7, 4, 5);
			expectedBoardGrid[0, 0] = t1;
			expectedBoardGrid[3, 5] = t2;

			// Check boards are the same
			for (int i = 0; i < expectedBoardGrid.GetLength(0); i++)
            {
				for (int j = 0; j < expectedBoardGrid.GetLength(0); j++)
                {
					if (expectedBoardGrid[i, j] == null)
                    {
						Assert.IsNull(actualBoardGrid[i, j]);
                    }
                    else
                    {
						Assert.IsTrue(expectedBoardGrid[i, j].isEqual(actualBoardGrid[i, j]));
                    }
                }
            }

		}
        [TestMethod]
		public void XMLPawnLocToPosnNormalPlay(){
			
		}
		[TestMethod]
        public void XMLPawnLocToPosnStartPlay()
        {

        }
		[TestMethod]
        public void XMLPawnLocToPosnEliminatedPlay()
        {

        }
		[TestMethod]
		public void XMLToBoardStartGame() {
			XElement boardXML = XElement.Parse("<board>" +
			                                   "<map>" +
			                                   "<ent><xy><x>0</x><y>0</y></xy>" +
			                                   "<tile>" +
			                                   "<connect><n>0</n><n>1</n></connect>" +
			                                   "<connect><n>2</n><n>4</n></connect>" +
			                                   "<connect><n>3</n><n>6</n></connect>" +
			                                   "<connect><n>5</n><n>7</n></connect>" +
			                                   "</tile>" +
			                                   "</ent>" +
			                                   "</map>" +
			                                   "<map>" +
			                                   "<ent><color>red</color><pawn-loc><h></h><n>0</n><n>0</n></pawn-loc>" +
			                                   "</ent></map></board>");

			Board expected = new Board();
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
			expected.grid[0, 0] = t1;

            SPlayer p1 = new SPlayer("red", new List<Tile>(), new RandomPlayer());
			p1.initialize(expected);
			test.setStartPos(expected, p1, new Posn(-1, 0, 5));

			Board actual = XMLDecoder.xmlToBoard(boardXML);

			// Check boards are the same
			for (int i = 0; i < expected.grid.GetLength(0); i++) {
				for (int j = 0; j < expected.grid.GetLength(0); j++) {
					if (expected.grid[i, j] == null) {
						Assert.IsNull(actual.grid[i, j]);
					} else {
						Assert.IsTrue(expected.grid[i, j].isEqual(actual.grid[i, j]));
					}
				}
			}

			// Check players are the same
			Assert.AreEqual(expected.getNumActive(), actual.getNumActive());
			List<string> expectedActivePlayers = expected.getPlayerOrder();
			foreach (string color in expectedActivePlayers) {
				Assert.IsTrue(actual.isOnBoard(color));
			}
         
		}
             
		[TestMethod]
        public void XMLToBoardBothActiveAndEliminatedPlayers()
        {
            XElement boardXML = XElement.Parse("<board>" +
			                                   "<map>" +
			                                   "<ent>" +
			                                   "<xy><x>1</x><y>2</y></xy>" +
			                                   "<tile>" +
			                                   "<connect><n>0</n><n>1</n></connect>" +
			                                   "<connect><n>2</n><n>4</n></connect>" +
			                                   "<connect><n>3</n><n>6</n></connect>" +
			                                   "<connect><n>5</n><n>7</n></connect>" +
			                                   "</tile>" +
			                                   "</ent>" +
			                                   "<ent>" +
                                               "<xy><x>0</x><y>0</y></xy>" +
                                               "<tile>" +
                                               "<connect><n>0</n><n>1</n></connect>" +
                                               "<connect><n>2</n><n>3</n></connect>" +
                                               "<connect><n>4</n><n>5</n></connect>" +
                                               "<connect><n>6</n><n>7</n></connect>" +
                                               "</tile>" +
                                               "</ent>" +
			                                   "</map>" +
			                                   "<map>" +
			                                   "<ent><color>red</color><pawn-loc><h></h><n>0</n><n>0</n></pawn-loc></ent>" +
			                                   "<ent><color>blue</color><pawn-loc><h></h><n>3</n><n>3</n></pawn-loc></ent>" +
			                                   "</map>" +
			                                   "</board>");

            Board expected = new Board();
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
			Tile t2 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            expected.grid[2, 1] = t1;
			expected.grid[0, 0] = t2;

            SPlayer p1 = new SPlayer("red", new List<Tile>(), new RandomPlayer());
            p1.initialize(expected);
            test.setStartPos(expected, p1, new Posn(0,0,0));
            
			SPlayer p2 = new SPlayer("blue", new List<Tile>(), new RandomPlayer());
            p2.initialize(expected);
            test.setStartPos(expected, p2, new Posn(2,1,4));

            Board actual = XMLDecoder.xmlToBoard(boardXML);

            // Check boards are the same
            for (int i = 0; i < expected.grid.GetLength(0); i++)
            {
                for (int j = 0; j < expected.grid.GetLength(0); j++)
                {
                    if (expected.grid[i, j] == null)
                    {
                        Assert.IsNull(actual.grid[i, j]);
                    }
                    else
                    {
                        Assert.IsTrue(expected.grid[i, j].isEqual(actual.grid[i, j]));
                    }
                }
            }

			// Check players are the same
			Assert.IsTrue(actual.isOnBoard("blue"));
			Assert.IsFalse(actual.isOnBoard("red"));

			Assert.IsTrue(actual.isEliminated("red"));
			Assert.IsFalse(actual.isEliminated("blue"));

			Assert.AreEqual(1, actual.getNumActive());
			Assert.AreEqual(1, actual.getNumEliminated());

        }

		[TestMethod]
        public void XMLToTile()
        {
            XElement t1XML = new XElement("tile",
                                                 new XElement("connect",
                                                             new XElement("n", 0),
                                                              new XElement("n", 1)),
                                                  new XElement("connect",
                                                             new XElement("n", 2),
                                                              new XElement("n", 3)),

                                                  new XElement("connect",
                                                             new XElement("n", 4),
                                                              new XElement("n", 5)),

                                                  new XElement("connect",
                                                             new XElement("n", 6),
                                                               new XElement("n", 7)));
            TestScenerios test = new TestScenerios();
            Tile t1Expected = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile t1Actual = XMLDecoder.xmlToTile(t1XML);

            Assert.IsTrue(t1Expected.isEqual(t1Actual));

        }
    }
}
