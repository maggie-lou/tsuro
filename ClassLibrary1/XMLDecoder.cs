using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace tsuro
{
	// Converts XML to objects 
    public static class XMLDecoder
    {
		public static Tile xmlToTile(XElement tileXml)
        {
            List<Path> paths = new List<Path>();
            List<XElement> tileXMLTree = tileXml.Descendants().ToList();
            bool tileCheck = checkOrderOfTagsFromXML(new List<string> { "connect", "n", "n",
                "connect", "n", "n",
                "connect", "n", "n",
                "connect", "n", "n"}, tileXMLTree);
            if (!tileCheck)
            {
                throw new Exception("Invalid Tile XML Received from Network Player.");
            }
            IEnumerable<XElement> elements =
            from el in tileXml.Elements("connect")
            select el;
            foreach (XElement i in elements)
            {
                int start = (int)i.Elements("n").ElementAt(0);
                int end = (int)i.Elements("n").ElementAt(1);
                paths.Add(new Path(start, end));
            }
            Tile returnTile = new Tile(paths);
            return returnTile;

        }
        public static bool checkOrderOfTagsFromXML(List<string> expected, List<XElement> actual)
        {
            if (expected.Count != actual.Count)
            {
                return false;
            }

            for (int i = 0; i < expected.Count; i++)
            {
                if (expected[i] != actual[i].Name)
                {
                    return false;
                }
            }
            return true;
        }
        public static List<Posn> xmlToPosn(XElement posnXML)
        {
            List<XElement> posnXMLTree = posnXML.Descendants().ToList();
            bool hCheck = checkOrderOfTagsFromXML(new List<string> { "h", "n", "n" }, posnXMLTree);
            bool vCheck = checkOrderOfTagsFromXML(new List<string> { "v", "n", "n" }, posnXMLTree);
            if (!(hCheck || vCheck))
            {
                throw new Exception("Invalid Posn XML Received from Network Player.");
            }

            int row1;
            int row2;
            int col1;
            int col2;
            int tilePos;
            int tilePos2;

            XElement edgeXML = posnXML.Elements("n").ElementAt(0);
            int edge;
            int.TryParse(edgeXML.Value, out edge);

            XElement locOnEdgeXML = posnXML.Elements("n").ElementAt(1);
            int locOnEdge;
            int.TryParse(locOnEdgeXML.Value, out locOnEdge);

            if (hCheck)
            {
                row1 = edge;
                row2 = edge - 1;

                col1 = locOnEdge / 2;
                col2 = col1;

                tilePos = locOnEdge % 2;
                if (tilePos == 0)
                {
                    tilePos2 = 5;
                }
                else
                {
                    tilePos2 = 4;
                }
            }
            else
            {
                col1 = edge - 1;
                col2 = edge;

                row1 = locOnEdge / 2;
                row2 = row1;

                tilePos = locOnEdge % 2 + 2;
                if (tilePos == 2)
                {
                    tilePos2 = 7;
                }
                else
                {
                    tilePos2 = 6;
                }
            }

            Posn p1 = new Posn(row1, col1, tilePos);
            Posn p2 = new Posn(row2, col2, tilePos2);
            List<Posn> posnList = new List<Posn> { p1, p2 };

            return posnList;
        }
        public static string xmlToColor(XElement colorXML)
		{
			return colorXML.Value.ToString();
		}
        public static List<string> xmlToListOfColors(XElement listOfColorsXML)
		{
			List<string> listOfColors = new List<string>();
			foreach (XElement colorXML in listOfColorsXML.Elements("color"))
			{
				listOfColors.Add(colorXML.Value.ToString());
			}
			return listOfColors;
		}
        public static Board xmlToBoard(XElement boardXML, bool startGame)
		{
			Board board = new Board();
			int col = -1;
			int row = -1;
			XElement tilesXML = boardXML.Elements("map").ElementAt(0);
			XElement pawnsXML = boardXML.Elements("map").ElementAt(1);

            //create board with tiles placed in correct grid position
            foreach (XElement ent in tilesXML.Elements("ent"))
			{
				try
                {
                    col = Int32.Parse(ent.Descendants("x").ElementAt(0).Value);
                    row = Int32.Parse(ent.Descendants("y").ElementAt(0).Value);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
				Tile tile = xmlToTile(ent.Descendants("tile").ElementAt(0));
				board.grid[row, col] = tile;
			}
			// create pawns (aka onboard players)
			foreach (XElement ent in pawnsXML.Elements("ent"))
			{
				bool validXML = checkOrderOfTagsFromXML(new List<string> { "color", "pawn-loc" },
				                                        ent.Elements().ToList());
				if (!validXML) {
					throw new Exception("Invalid XML in xmlToBoard.");
				}
				string color = ent.Element("color").Value;
				List<Posn> possiblePosns = xmlToPosn(ent.Element("pawn-loc"));
				Posn startPos = pawnLocToPosn(startGame, possiblePosns, board);

				SPlayer tempPlayer = new SPlayer();
				tempPlayer.setColor(color);
				tempPlayer.setPosn(startPos);
				board.addPlayerToBoard(tempPlayer);
			}
			return board;
		}

        // Returns a position on the board from the 2 options, after parsing pawn loc XML
        // Needed because board has 2 positions per location
		public static Posn pawnLocToPosn(bool startGame, List<Posn> possiblePosns, Board board) {
			if (possiblePosns.Count != 2) {
				throw new Exception("There should only be two possible pawn locations.");
			}
         
			if (startGame)
			{
				Posn startPos;

                // Choose phantom position for start position
                if (!(board.onEdge(possiblePosns[0]) || board.onEdge(possiblePosns[1])))
                {
                    throw new Exception("Neither position is a start position.");
                }
                if (board.onEdge(possiblePosns[0]))
                { // onEdge returns true if posn is on edge and not a phantom position
                    startPos = possiblePosns[1];
                }
                else
                {
                    startPos = possiblePosns[0];
                }

				return startPos;
			} else {
				// Player is always on a tile, about to move to an empty space
                // Valid positions must be on a tile
				Posn posn = possiblePosns[0];
				if (board.grid[posn.returnRow(), posn.returnCol()] != null) {
					return posn;
				} else {
					return possiblePosns[1];
				}
			}
		}
	}
}
