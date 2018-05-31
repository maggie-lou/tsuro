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
    public class DecoderTests
    {
        public DecoderTests()
        {
        }

		[TestMethod]
		public void XMLToBoard() {
			XElement boardXML = XElement.Parse("<board><map><ent><xy><x>0</x><y>0</y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>4</n></connect><connect><n>3</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile></ent></map><map><ent><color>red</color><pawn-loc><v></v><n>1</n><n>1</n></pawn-loc></ent></map></board>");

			Board expected = new Board();
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
			expected.grid[0, 0] = t1;

            SPlayer p1 = new SPlayer("red", new List<Tile>(), new RandomPlayer());
			p1.initialize(expected);
			test.setStartPos(expected, p1, new Posn(0, 0, 3));

			Board actual = XMLDecoder.xmlToBoard(boardXML);

            // Check boards are the same
			for (int i = 0; i < expected.grid.Length; i++) {
				for (int j = 0; j < expected.grid.Length; j++) {
					if (expected.grid[i, j] == null) {
						Assert.IsNull(actual.grid[i, j]);
					} else {
						Assert.IsTrue(expected.grid[i, j].isEqual(actual.grid[i, j]));
					}
				}
			}

			// Check players are the same
			Assert.AreEqual(expected.returnOnBoard().Count, actual.returnOnBoard().Count);
			for (int i = 0; i < expected.returnOnBoard().Count; i++) {
				SPlayer expectedPlayer = expected.returnOnBoard()[i];
				SPlayer actualPlayer = actual.returnOnBoard()[i];

				Assert.AreEqual(expectedPlayer.returnColor(), actualPlayer.returnColor());
			}
         
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
