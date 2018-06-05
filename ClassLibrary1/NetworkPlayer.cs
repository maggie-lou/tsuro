using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Net.Sockets;

namespace tsuro
{
	// A player that is initialized from an external source over the network
    // and can play in a local tournament
	public class NetworkPlayer: IPlayer
    {
		protected string name = "";
        protected List<string> allPlayers = new List<string>();
        protected string[] validNames = new string[] {"blue","red","green","orange","sienna"
            ,"hotpink","darkgreen","purple"};
		Socket handler;
		
		public NetworkPlayer(Socket sock, string name) 
        {
			this.name = name;
			handler = sock;
        }


		public XElement sendQuery(XElement query)
		{
			// Takes in a URL
			// Posts query to that URLs
			string queryString = query.ToString();
			byte[] msg = System.Text.Encoding.ASCII.GetBytes(queryString);  
            int bytesSent = handler.Send(msg);  

			byte[] bytes = new byte[1024];
            int bytesRec = handler.Receive(bytes);
			string responseString = System.Text.Encoding.ASCII.GetString(bytes, 0, bytesRec);
			return XElement.Parse(responseString);
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
			XElement listOfColors = XMLEncoder.listOfColorsToXML(allColors);
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
			                                 XMLEncoder.boardToXML(b));
			XElement response = sendQuery(xmlQuery);
			List<Posn> possiblePosns = XMLDecoder.xmlToPosn(response);

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
			XElement xmlQuery = new XElement("play-turn",
			                                 XMLEncoder.boardToXML(b),
			                                 XMLEncoder.playerHandToXML(playerHand),
											 new XElement("n", numTilesInDrawPile));
			XElement response = sendQuery(xmlQuery);
			Tile returnedTile = XMLDecoder.xmlToTile(response);
			return returnedTile;
		}
        
		public void endGame(Board b, List<string> allColors)
		{
			XElement winners = XMLEncoder.listOfColorsToXML(allColors);
			XElement xmlQuery = new XElement("end-game",
											XMLEncoder.boardToXML(b),
											 winners);
			XElement response = sendQuery(xmlQuery);
            if (response.Name != "void")
			{
				throw new Exception("Expected void from network player end game call.");
			}         
		}      
    }
}
