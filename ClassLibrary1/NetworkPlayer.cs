using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace tsuro
{
	public class NetworkPlayer: IPlayers
    {
		protected string name = "";
        protected List<string> allPlayers = new List<string>();
        protected string[] validNames = new string[] {"blue","red","green","orange","sienna"
            ,"hotpink","darkgreen","purple"};
		
        public NetworkPlayer()
        {
        }

		public XElement sendQuery(XElement query)
		{
			throw new NotImplementedException();
		}
		public string getName()
		{
			XElement xmlQuery = new XElement("get-name");
			// Send this request out
			// wait to receive a <player-name>
			XElement response = sendQuery(xmlQuery);
            // get value within player-name
			return response.Value;
            
		}
		public void initialize(string playerColor, List<string> allColors)
		{
			XElement listOfColors = new XElement("list");
			foreach (var color in allColors)
			{
				listOfColors.Add(new XElement("color", color));
			}
			XElement xmlQuery = new XElement("initialize",
											new XElement("color", playerColor),
											 listOfColors);
			//Expect to receive a void
			XElement response = sendQuery(xmlQuery);

			//check if return value is void
            if (response.Name != "void")
			{
				throw new Exception("Expected void from network player initialize call.");
			}
		}

		public Posn placePawn(Board b)
		{
			XElement xmlQuery = new XElement("place-pawn",
											 boardToXML(b));
			XElement response = sendQuery(xmlQuery);
			//return null;
			List<Posn> possiblePosns = xmlToPosn(response);

            foreach (Posn pos in possiblePosns)
			{
				// on edge returns true if posn is on the edge and NOT a phantom posn
                if (!b.onEdge(pos))
				{
					return pos;
				}
			}
			throw new Exception("Did not get an phantom starting position for network player from placePawn.");
		}

        
		public Tile playTurn(Board b, List<Tile> playerHand, int numTilesInDrawPile)
		{
			throw new NotImplementedException();
            
		}
		public void endGame(Board b, List<string> allColors)
		{
			throw new NotImplementedException();

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
				case 0: case 1: case 4: case 5:
					hv = new XElement("h", "");
					break;
				case 2: case 3: case 6: case 7:
					hv = new XElement("v", "");
					break;
				default:
					hv = null;
					break;
			}

			XElement edge;
			switch(loc)
			{
				case 0: case 1:
					edge = new XElement("n", p.returnRow());
					break;
				case 5: case 4:
					edge = new XElement("n", p.returnRow() + 1);
					break;
				case 2: case 3:
					edge = new XElement("n", p.returnCol() + 1);
					break;
				case 6: case 7:
					edge = new XElement("n", p.returnCol());
					break;     
				default:
					edge = null;
					break;
			}

			XElement locOnEdge;
            switch(loc)
			{
				case 0: case 5:
					locOnEdge = new XElement("n", p.returnCol() * 2);
                    break;
				case 1: case 4:
					locOnEdge = new XElement("n", p.returnCol() * 2 + 1);
					break;
				case 2: case 7:
					locOnEdge = new XElement("n", p.returnRow() * 2);
                    break;
				case 3: case 6:
					locOnEdge = new XElement("n", p.returnRow() * 2 + 1);
                    break;
				default:
					locOnEdge = null;
					break;
			}

            if (hv == null ||edge == null || locOnEdge == null)
			{
				throw new Exception("Invalid position input to posnToPawnLocXML!!!!!");
			}
			return new XElement("pawn-loc", hv, edge, locOnEdge);


		}

        public static XElement boardToXML(Board b)
		{
			XElement boardXML = new XElement("board");
			XElement listTilesXML = new XElement("map");
			XElement listPawnsXML = pawnsToXML(b.returnOnBoard());
			for (int row = 0; row < 6; row++) {
				for (int col = 0; col < 6; col++) {
					if (b.occupied(row, col)) {
						XElement tile = tileToXML(b.grid[row, col]);
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
        public static XElement pawnsToXML(List<SPlayer> playerlist)
		{
			XElement pawnXML = new XElement("map");
            foreach (SPlayer p in playerlist)
			{
				pawnXML.Add(new XElement("ent",
				                         new XElement("color", p.returnColor()),
				                                     posnToPawnLocXML(p.getPlayerPosn())));
			}
			return pawnXML;             
		}

		public static XElement splayerToXML(SPlayer player, Board board) {
			XElement splayerXML;
			if (board.returnDragonTileHolder() != null && 
			    board.returnDragonTileHolder().returnColor().Equals(player.returnColor()))
			{
				splayerXML = new XElement("splayer-dragon");
			} else {
				splayerXML = new XElement("splayer-nodragon");
			}

			XElement handTileXML = new XElement("list");
			foreach (Tile t in player.returnHand()) {
				handTileXML.Add(tileToXML(t));
			}

			splayerXML.Add(new XElement("color", player.returnColor()),
			               handTileXML);
			return splayerXML;
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
		public static List<Posn> xmlToPosn(XElement posnXML) {
			List<XElement>posnXMLTree = posnXML.Descendants().ToList();
			bool hCheck = checkOrderOfTagsFromXML(new List<string> { "h", "n", "n" }, posnXMLTree);
			bool vCheck = checkOrderOfTagsFromXML(new List<string> { "v", "n", "n" }, posnXMLTree);
			if (!(hCheck || vCheck))
			{
				throw new Exception("Invalid tags from posn xml from network player.");
			}

			//bool isH = posnXML.Elements("h").Any();
			//bool isV = posnXML.Elements("v").Any();
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
            
			if (hCheck) {
				row1 = edge;
				row2 = edge-1;

				col1 = locOnEdge / 2;
				col2 = col1;

				tilePos = locOnEdge % 2;
				if (tilePos == 0) {
					tilePos2 = 5;
				} else {
					tilePos2 = 4;
				}
			} else {
				col1 = edge-1;
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

    }
}
