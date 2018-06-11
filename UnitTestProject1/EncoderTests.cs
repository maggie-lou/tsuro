using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tsuro;
using System.Xml.Linq;
using System.Linq;

namespace TsuroTests
{
	[TestClass]
    public class EncoderTests
    {
        [TestMethod]
		public void nameToXML() {
			string color = "green";
			XElement expected = new XElement("player-name", color);
			XElement actual = XMLEncoder.nameToXML(color);
			Assert.IsTrue(XNode.DeepEquals(expected, actual));
		}

        [TestMethod]
		public void encodeVoid() {
			XElement expected = new XElement("void", "");
            XElement actual = XMLEncoder.encodeVoid();
            Assert.IsTrue(XNode.DeepEquals(expected, actual));
		}
        
		[TestMethod]
        public void listOfColorsToXML()
        {
            XElement allColorsExpected = new XElement("list",
                                                      new XElement("color", "blue"),
                                                     new XElement("color", "hotpink"),
                                                      new XElement("color", "green"));
            List<string> allColors = new List<string> { "blue", "hotpink", "green" };

			XElement allColorsActual = XMLEncoder.listOfColorsToXML(allColors);

            Assert.IsTrue(XNode.DeepEquals(allColorsExpected, allColorsActual));
           
        }

		[TestMethod]
        public void TiletoXML()
        {
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);

            XElement t1XML = XMLEncoder.tileToXML(t1);
            XElement t1XMLExpected = new XElement("tile",
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
            Assert.IsTrue(XNode.DeepEquals(t1XML, t1XMLExpected));
        }

		[TestMethod]
        public void PosnToPawnLocXML()
        {
            Posn testPosn1 = new Posn(1, 1, 0);
            Posn testPosn2 = new Posn(0, 1, 5);

            XElement pawnLocXML1 = XMLEncoder.posnToPawnLocXML(testPosn1);
            XElement pawnLocXML2 = XMLEncoder.posnToPawnLocXML(testPosn2);

            XElement pawnLocXMLExpected = new XElement("pawn-loc",
                                               new XElement("h", ""),
                                               new XElement("n", 1),
                                                new XElement("n", 2));
            Assert.IsTrue(XNode.DeepEquals(pawnLocXML1, pawnLocXMLExpected));
            Assert.IsTrue(XNode.DeepEquals(pawnLocXML2, pawnLocXMLExpected));
        }

        [TestMethod]
        public void pawnToXML()
        {
			TestScenerios test = new TestScenerios();
			Board board = new Board();
            SPlayer p1 = new SPlayer("blue", new List<Tile>());
			test.setStartPos(board, p1, new Posn(0, 0, 5));
            
            SPlayer p2 = new SPlayer("hotpink", new List<Tile>());
			test.setStartPos(board, p2, new Posn(4, 4, 5));
            
            SPlayer p3 = new SPlayer("green", new List<Tile>());
			test.setStartPos(board, p3, new Posn(3, 2, 6));

            List<SPlayer> players = new List<SPlayer> { p1, p2, p3 };

			XElement playersToXML = XMLEncoder.pawnsToXML(new List<string>{"blue", "hotpink", "green"}, board);

            XElement playersToXMLExpected = new XElement("map",
                                                        new XElement("ent",
                                                                    new XElement("color", "blue"),
                                                                     new XElement("pawn-loc",
                                                                                  new XElement("h", ""),
                                                                                  new XElement("n", 1),
                                                                                  new XElement("n", 0))),
                                                         new XElement("ent",
                                                                    new XElement("color", "hotpink"),
                                                                     new XElement("pawn-loc",
                                                                                  new XElement("h", ""),
                                                                                  new XElement("n", 5),
                                                                                  new XElement("n", 8))),
                                                         new XElement("ent",
                                                                    new XElement("color", "green"),
                                                                     new XElement("pawn-loc",
                                                                                  new XElement("v", ""),
                                                                                  new XElement("n", 2),
                                                                                  new XElement("n", 7))));
            Assert.IsTrue(XNode.DeepEquals(playersToXML, playersToXMLExpected));
        }
        [TestMethod]
        public void boardToXMLSimple()
        {
            Board board = new Board();
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
			board.placeTileAt(t1, 0, 0);

            SPlayer p1 = new SPlayer("red", new List<Tile>(), new RandomPlayer());
            p1.initialize(board);
            test.setStartPos(board, p1, new Posn(0, 0, 3));

            XElement boardToXML = XMLEncoder.boardToXML(board);
            XElement boardToXMLExpected = XElement.Parse("<board><map><ent><xy><x>0</x><y>0</y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>4</n></connect><connect><n>3</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile></ent></map><map><ent><color>red</color><pawn-loc><v></v><n>1</n><n>1</n></pawn-loc></ent></map></board>");

            Assert.IsTrue(XNode.DeepEquals(boardToXML, boardToXMLExpected));

        }

		[TestMethod]
		public void boardToXMLWithEliminatedPlayers()
		{
			Board board = new Board();
            TestScenerios test = new TestScenerios();

            // Board set up 
            Tile normalTile = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
			Tile eliminationTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
			board.placeTileAt(normalTile, 0, 0);
			board.placeTileAt(eliminationTile, 3, 0);
           

            // Player set up
			SPlayer activePlayer = new SPlayer("red", new List<Tile>(), new RandomPlayer());
			activePlayer.initialize(board);
			test.setStartPos(board, activePlayer, new Posn(0, 0, 3));
            
			SPlayer eliminatedPlayer = new SPlayer("blue", new List<Tile>(), new RandomPlayer());
			eliminatedPlayer.initialize(board);
			test.setStartPos(board, eliminatedPlayer, new Posn(3, 0, 7));

            // Test
			XElement boardToXMLActual = XMLEncoder.boardToXML(board);
			XElement boardToXMLExpected = XElement.Parse("<board><map><ent><xy><x>0</x><y>0</y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>4</n></connect><connect><n>3</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile></ent><ent><xy><x>0</x><y>3</y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent></map><map><ent><color>red</color><pawn-loc><v></v><n>1</n><n>1</n></pawn-loc></ent><ent><color>blue</color><pawn-loc><v></v><n>0</n><n>6</n></pawn-loc></ent></map></board>"); 
			Assert.IsTrue(XNode.DeepEquals(boardToXMLActual, boardToXMLExpected));

		}
        
		[TestMethod]
        public void boardToXMLPlayerEliminatedCurrentTurn()
        {
			Admin a = new Admin();
            Board board = new Board();
            TestScenerios test = new TestScenerios();

            // Board set up 
            Tile normalTile = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            Tile eliminationTile = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
			Tile drawPileTile = test.makeTile(0, 3, 2, 1, 4, 7, 6, 5);
			board.placeTileAt(normalTile, 0, 0);
			board.addTileToDrawPile(drawPileTile);

            // Player set up
            SPlayer eliminatedPlayer = new SPlayer("blue", new List<Tile>(), new RandomPlayer());
            eliminatedPlayer.initialize(board);
            test.setStartPos(board, eliminatedPlayer, new Posn(3, -1, 3));

            SPlayer activePlayer = new SPlayer("red", new List<Tile>(), new RandomPlayer());
            activePlayer.initialize(board);
            test.setStartPos(board, activePlayer, new Posn(0, 0, 3));

			TurnResult tmpturn = a.playATurn(board, eliminationTile);

            // Test
			XElement boardToXMLActual = XMLEncoder.boardToXML(tmpturn.b);
            XElement boardToXMLExpected = XElement.Parse("<board><map><ent><xy><x>0</x><y>0</y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>4</n></connect><connect><n>3</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile></ent><ent><xy><x>0</x><y>3</y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent></map><map><ent><color>red</color><pawn-loc><v></v><n>1</n><n>1</n></pawn-loc></ent><ent><color>blue</color><pawn-loc><v></v><n>0</n><n>6</n></pawn-loc></ent></map></board>");

            Assert.IsTrue(XNode.DeepEquals(boardToXMLActual, boardToXMLExpected));

        }

        [TestMethod]
        public void splayerToXMLDragonHolder()
        {
            Board board = new Board();
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            List<Tile> hand = test.makeHand(t1);
            SPlayer p1 = new SPlayer("red", hand, new RandomPlayer());
            board.setDragonTileHolder(p1);

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

            XElement splayerXMLExpected = new XElement("splayer-dragon",
                                                       new XElement("color", "red"),
                                                       new XElement("set",
                                                                    t1XML));
            XElement splayerXMLActual = XMLEncoder.splayerToXML(p1, board);
            Console.WriteLine(splayerXMLActual);

            Assert.IsTrue(XNode.DeepEquals(splayerXMLExpected, splayerXMLActual));
        }

        [TestMethod]
        public void splayerToXMLNotDragonHolder()
        {
            Board board = new Board();
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            List<Tile> hand = test.makeHand(t1);
            SPlayer p1 = new SPlayer("red", hand, new RandomPlayer());

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

            XElement splayerXMLExpected = new XElement("splayer-nodragon",
                                                       new XElement("color", "red"),
                                                       new XElement("set",
                                                                    t1XML));
            XElement splayerXMLActual = XMLEncoder.splayerToXML(p1, board);
            Console.WriteLine(splayerXMLActual);

            Assert.IsTrue(XNode.DeepEquals(splayerXMLExpected, splayerXMLActual));
        }

        [TestMethod]
        public void xmlToPosn()
        {
            XElement xmlPosn = new XElement("pawn-loc",
                                               new XElement("h", ""),
                                               new XElement("n", 1),
                                                new XElement("n", 4));
            Posn posnExpected1 = new Posn(1, 2, 0);
            Posn posnExpected2 = new Posn(0, 2, 5);
            List<Posn> expected = new List<Posn> { posnExpected1, posnExpected2 };

			List<Posn> actual = XMLDecoder.xmlToPosn(xmlPosn);
            Assert.IsTrue(actual.Exists(x => x.isEqual(posnExpected1)));
            Assert.IsTrue(actual.Exists(x => x.isEqual(posnExpected2)));
            Assert.AreEqual(actual.Count, expected.Count);



            XElement xmlPosn2 = new XElement("pawn-loc",
                                               new XElement("v", ""),
                                               new XElement("n", 3),
                                                new XElement("n", 6));
            posnExpected1 = new Posn(3, 2, 2);
            posnExpected2 = new Posn(3, 3, 7);
            expected = new List<Posn> { posnExpected1, posnExpected2 };

			actual = XMLDecoder.xmlToPosn(xmlPosn2);
            Assert.IsTrue(actual.Exists(x => x.isEqual(posnExpected1)));
            Assert.IsTrue(actual.Exists(x => x.isEqual(posnExpected2)));
            Assert.AreEqual(actual.Count, expected.Count);


        }


		[TestMethod]
        [ExpectedException(typeof(XMLTagOrderException))]
        public void InvalidPosnXmlThrowsException()
        {
            XElement xmlPosnInvalid = new XElement("pawn-loc",
                                               new XElement("g", ""),
                                               new XElement("n", 3),
                                                new XElement("n", 6));

			List<Posn> actual = XMLDecoder.xmlToPosn(xmlPosnInvalid);
        }
    }
}
