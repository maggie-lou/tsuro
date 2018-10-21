using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace tsuro
{
	// Converts XML to objects 
    public static class XMLDecoder
    {
		public static string xmlSPlayerToColor(XElement splayerXml)
        {
            return splayerXml.Element("color").Value;         
        }
        
		public static List<Tile> xmlSPlayerToHand(XElement splayerXml){
			return xmlToListOfTiles(splayerXml.Element("set"));         
		}
      

		public static SPlayer xmlToSplayer(XElement SPlayerXml){
			bool isDragon;

			// Must be either a splayer-dragon or splayer-nodragon
			try {
				checkOrderOfTagsFromXML(new List<string> { "splayer-dragon" }, new List<XElement> { SPlayerXml });
				isDragon = true;
			} catch(XMLTagOrderException) {
				checkOrderOfTagsFromXML(new List<string> { "splayer-nodragon" }, new List<XElement> { SPlayerXml });
				isDragon = false;
			}
            checkOrderOfTagsFromXML(new List<string> { "color", "set" },
			                        SPlayerXml.Elements().ToList());
			
			string color = SPlayerXml.Element("color").Value;
			List<Tile> playerHand = xmlToListOfTiles(SPlayerXml.Element("set"));
			return new SPlayer(color, playerHand, null, isDragon);

		}

		public static List<Tile> xmlToListOfTiles(XElement listOfTilesXml)
		{
			// Must be either a list or set of tiles
			try {
				checkOrderOfTagsFromXML(new List<string> { "set" }, new List<XElement> { listOfTilesXml });
			} catch (XMLTagOrderException) {
				checkOrderOfTagsFromXML(new List<string> { "list" }, new List<XElement> { listOfTilesXml });
			}

			List<Tile> listOfTiles = new List<Tile>();
            foreach (XElement tileXml in listOfTilesXml.Elements("tile"))
			{
				listOfTiles.Add(xmlToTile(tileXml));
			}
			return listOfTiles;
		}
		public static Tile xmlToTile(XElement tileXml)
        {
			// Check outer tag
			checkOrderOfTagsFromXML(new List<string> { "tile" }, new List<XElement> { tileXml });

            List<Path> paths = new List<Path>();

            // Check inner tag order 
            List<XElement> tileXMLTree = tileXml.Descendants().ToList();
            checkOrderOfTagsFromXML(new List<string> { "connect", "n", "n",
                "connect", "n", "n",
                "connect", "n", "n",
                "connect", "n", "n"}, tileXMLTree);

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

		public static int xmlToNumber(XElement numberXML) {
			checkOrderOfTagsFromXML(new List<string> { "n" }, new List<XElement> { numberXML });
			return int.Parse(numberXML.Value);
		}

        // Throws an exception, if tags do not match what is expected, in the given order
        // Does nothing if tags match
        public static void checkOrderOfTagsFromXML(List<string> expectedTags, List<XElement> actualTags)
        {
			if (expectedTags.Count != actualTags.Count)
            {
				throw new XMLTagOrderException("Expected " + expectedTags.Count + " tags, received " + actualTags.Count + ".");
            }

			for (int i = 0; i < expectedTags.Count; i++)
            {
				if (expectedTags[i] != actualTags[i].Name)
                {
					throw new XMLTagOrderException("Expected tag " + expectedTags[i] + ",received tag " + actualTags[i] + ".");
                }
            }
        }

        public static List<Posn> xmlToPosn(XElement posnXML)
        {
            List<XElement> posnXMLTree = posnXML.Descendants().ToList();

			// Check XML tags
			// If not a horizontal pawn-loc, must be a vertical pawn-loc
			bool horizontalPosn = false;
			try {
				checkOrderOfTagsFromXML(new List<string> { "h", "n", "n" }, posnXMLTree);
				horizontalPosn = true;
			} catch(XMLTagOrderException) {
				checkOrderOfTagsFromXML(new List<string> { "v", "n", "n" }, posnXMLTree);
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

			if (horizontalPosn)
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
			checkOrderOfTagsFromXML(new List<string> { "color" }, new List<XElement> { colorXML });
			return colorXML.Value;
		}

        public static List<string>xmlToListOfColors(XElement listOfColorsXML)
		{
			checkOrderOfTagsFromXML(new List<string> { "list" }, new List<XElement> { listOfColorsXML });

			List<string> listOfColors = new List<string>();
			foreach (XElement colorXML in listOfColorsXML.Elements())
			{
				listOfColors.Add(xmlToColor(colorXML));
			}
			return listOfColors;
		}


        public static Board xmlToBoard(XElement boardXML)
		{
			checkOrderOfTagsFromXML(new List<string> { "board" },
			                        new List<XElement> { boardXML });
			checkOrderOfTagsFromXML(new List<string> { "map", "map" },
			                        boardXML.Elements().ToList());
			

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
				board.placeTileAt(tile, row, col);
			}
			// create pawns (aka onboard players)
			foreach (XElement ent in pawnsXML.Elements("ent"))
			{
				checkOrderOfTagsFromXML(new List<string> { "color", "pawn-loc" },
				                                        ent.Elements().ToList());

				string color = ent.Element("color").Value;
				List<Posn> possiblePosns = xmlToPosn(ent.Element("pawn-loc"));
				Posn startPos = pawnLocToPosn(possiblePosns, board);

				board.addPlayerToBoard(color, startPos);
			}
			return board;
		}
              

        // Returns a position on the board from the 2 options, after parsing pawn loc XML
        // Needed because board has 2 positions per location
		public static Posn pawnLocToPosn(List<Posn> possiblePosns, Board board) {
			if (possiblePosns.Count != 2) {
				throw new Exception("There should only be two possible pawn locations.");
			}
			// if on edge
			// start game or eliminated
			// check if there is a tile in the onedge position
			//not on edge
			// check if there is a tile on a posn and choose that one

			Posn phantomPosn = null;
			Posn edgePosn = null;
			if (board.isElimPosn(possiblePosns[0]))
			{
				phantomPosn = possiblePosns[1];
				edgePosn = possiblePosns[0];
			}else if (board.isElimPosn(possiblePosns[1])){
				phantomPosn = possiblePosns[0];
                edgePosn = possiblePosns[1];
			}
			if (phantomPosn != null)
			{
				
				if (board.getTileAt(edgePosn.returnRow(), edgePosn.returnCol()) != null)
				{
					return edgePosn;
				}
				return phantomPosn;

			}         

			// Player is always on a tile, about to move to an empty space
            // Valid positions must be on a tile
			Posn posn1 = possiblePosns[0];
			Posn posn2 = possiblePosns[1];
			if (board.getTileAt(posn1.returnRow(), posn1.returnCol()) != null) {
				return posn1;
			} else if (board.getTileAt(posn2.returnRow(), posn2.returnCol()) != null)
			{
				return posn2;
			}
			else
			{
				throw new Exception("Invalid posn of player (not on edge and don't have tiles anywhere around it).");
			}


		}
	}
}
