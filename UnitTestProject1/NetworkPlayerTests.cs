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
	public class NetworkPlayerTests
	{
		[TestMethod]
		public void test()
		{
            Board board = new Board();
            TestScenerios test = new TestScenerios();
            Tile t1 = test.makeTile(0, 1, 2, 4, 3, 6, 5, 7);
            board.grid[0, 0] = t1;

            SPlayer p1 = new SPlayer("red", new List<Tile>(), "Random");
            p1.initialize(board);
            test.setStartPos(board, p1, new Posn(0, 0, 3));

            XElement boardToXML = XMLEncoder.boardToXML(board);
            XElement boardToXMLExpected = XElement.Parse("<board><map><ent><xy><x>0</x><y>0</y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>4</n></connect><connect><n>3</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile></ent></map><map><ent><color>red</color><pawn-loc><v></v><n>1</n><n>1</n></pawn-loc></ent></map></board>");

            Assert.IsTrue(XNode.DeepEquals(boardToXML, boardToXMLExpected));
            
			XElement tilesXML = boardToXMLExpected.Elements("map").ElementAt(0);
			Console.WriteLine(tilesXML);
			XElement pawnsXML = boardToXMLExpected.Elements("map").ElementAt(1);
			Console.WriteLine(pawnsXML);

			string xval = tilesXML.Descendants("x").ElementAt(0).Value;
			int x = int.Parse(xval);
			Console.WriteLine(x);
		}
        
        

        

        
        


	}   
}