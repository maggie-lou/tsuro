using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace tsuro
{
	// Converts objects to XML to send over the network
    public static class XMLEncoder
    {
		public static XElement nameToXML(String name) {
		   	XElement nameXML = new XElement("player-name", name);
			return nameXML;
		}

		public static XElement encodeVoid() {
			XElement voidXML = new XElement("void", "");
			return voidXML;
		}
		public static XElement encodeFalse(){
			XElement falseXML = new XElement("false", "");
			return falseXML;
		}

		public static XElement listOfColorsToXML(List<string> allColors)
        {
            XElement listOfColors = new XElement("list");
            foreach (var color in allColors)
            {
                listOfColors.Add(new XElement("color", color));
            }
            return listOfColors;
        }
        
		public static XElement playerHandToXML(List<Tile> hand)
        {
            XElement handTileXML = new XElement("set");
            foreach (Tile t in hand)
            {
                handTileXML.Add(tileToXML(t));
            }
            return handTileXML;
        }

		public static XElement listOfTilesToXML(List<Tile> tiles)
        {
            XElement tilesXML = new XElement("list");
            foreach (Tile t in tiles)
            {
                tilesXML.Add(tileToXML(t));
            }
            return tilesXML;
        }

		public static XElement setOfTilesToXML(List<Tile> tiles)
        {
            XElement tilesXML = new XElement("list");
            foreach (Tile t in tiles)
            {
                tilesXML.Add(tileToXML(t));
            }
            return tilesXML;
        }

        public static XElement tileToXML(Tile t)
        {
            XElement xmlTile = new XElement("tile");
            foreach (var path in t.paths)
            {
                XElement xmlPaths = new XElement("connect",
                                                new XElement("n", path.loc1),
                                                 new XElement("n", path.loc2));
                xmlTile.Add(xmlPaths);
            }
            return xmlTile;
        }
        
        public static XElement posnToPawnLocXML(Posn p)
        {
            XElement hv;
            int loc = p.returnLocationOnTile();
            switch (loc)
            {
                case 0:
                case 1:
                case 4:
                case 5:
                    hv = new XElement("h", "");
                    break;
                case 2:
                case 3:
                case 6:
                case 7:
                    hv = new XElement("v", "");
                    break;
                default:
                    hv = null;
                    break;
            }

            XElement edge;
            switch (loc)
            {
                case 0:
                case 1:
                    edge = new XElement("n", p.returnRow());
                    break;
                case 5:
                case 4:
                    edge = new XElement("n", p.returnRow() + 1);
                    break;
                case 2:
                case 3:
                    edge = new XElement("n", p.returnCol() + 1);
                    break;
                case 6:
                case 7:
                    edge = new XElement("n", p.returnCol());
                    break;
                default:
                    edge = null;
                    break;
            }

            XElement locOnEdge;
            switch (loc)
            {
                case 0:
                case 5:
                    locOnEdge = new XElement("n", p.returnCol() * 2);
                    break;
                case 1:
                case 4:
                    locOnEdge = new XElement("n", p.returnCol() * 2 + 1);
                    break;
                case 2:
                case 7:
                    locOnEdge = new XElement("n", p.returnRow() * 2);
                    break;
                case 3:
                case 6:
                    locOnEdge = new XElement("n", p.returnRow() * 2 + 1);
                    break;
                default:
                    locOnEdge = null;
                    break;
            }

            if (hv == null || edge == null || locOnEdge == null)
            {
                throw new Exception("Invalid position input to posnToPawnLocXML!!!!!");
            }
            return new XElement("pawn-loc", hv, edge, locOnEdge);


        }

        public static XElement boardToXML(Board b)
        {
            XElement boardXML = new XElement("board");
            XElement listTilesXML = new XElement("map");

            // Includes positions of active and eliminated players 
			XElement listPawnsXML = pawnsToXML(b.getAllPlayerColors(), b);
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
					if (b.getTileAt(row,col) != null)
                    {
						XElement tile = tileToXML(b.getTileAt(row, col));
                        XElement xy = new XElement("xy",
                                                   new XElement("x", col),
                                                   new XElement("y", row));
                        XElement tileEntry = new XElement("ent", xy, tile);
                        listTilesXML.Add(tileEntry);
                    }
                }
            }
            boardXML.Add(listTilesXML, listPawnsXML);
            return boardXML;
        }
        public static XElement pawnsToXML(List<string> playerColorList, Board b)
        {
            XElement pawnXML = new XElement("map");
			foreach (string color in playerColorList)
            {
				Posn tempPosn = b.getPlayerPosn(color);
                pawnXML.Add(new XElement("ent",
                                         new XElement("color", color),
                                                     posnToPawnLocXML(tempPosn)));
            }
            return pawnXML;
        }
        
		public static XElement listOfSPlayerToXML(List<SPlayer> splayers, Admin a){
            // Empty list - avoid XElement shortcut 
			if (splayers.Count == 0) {
				return new XElement("list", "");
			}

			XElement splayersXML = new XElement("list");

			foreach (SPlayer player in splayers)
			{
				splayersXML.Add(splayerToXML(player, a));
			}
			return splayersXML;
		}
        public static XElement splayerToXML(SPlayer player, Admin a)
        {
            XElement splayerXML;
			if (a.getDragonTileHolder() != null &&
			    a.getDragonTileHolder().getColor() == player.getColor())
            {
                splayerXML = new XElement("splayer-dragon");
            }
            else
            {
                splayerXML = new XElement("splayer-nodragon");
            }


            XElement handTileXML = playerHandToXML(player.getHand());

			splayerXML.Add(new XElement("color", player.getColor()),
                           handTileXML);
            return splayerXML;
        }

		public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

		public static string toString(XElement xElement) {
			return RemoveWhitespace(xElement.ToString());
		}
    }


}
