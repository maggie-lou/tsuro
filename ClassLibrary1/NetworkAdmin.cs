using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Net;
using System.Net.Sockets;


namespace tsuro
{
	// Initializes an OutgoingCompetitor to compete in other tournaments over
	// the network, and responds to queries about that player
	public class NetworkAdmin
	{
		protected IPlayer player;

		public NetworkAdmin(IPlayer player)
		{
			this.player = player;
		}

		// Parses and responses to an XML query about the competitor
		// Returns XML string of response
		public String interpretQuery(String query)
		{
			XElement queryXML = XElement.Parse(query);
			String command = queryXML.Name.ToString();
			String response = null;

			switch (command)
			{
				case "get-name":
					String name = player.getName();
					response = XMLEncoder.nameToXML(name).ToString();
					break;
				case "initialize":
					response = initializeHandler(queryXML);
					break;
				case "place-pawn":
					XElement boardXML = queryXML.Element("board");
					Board board = XMLDecoder.xmlToBoard(boardXML);
					Posn posn = player.placePawn(board);
					response = XMLEncoder.RemoveWhitespace(XMLEncoder.posnToPawnLocXML(posn).ToString());
					break;
				case "play-turn":
					response = playTurnHandler(queryXML);
                    break;
				case "end-game":
					XElement xmlBoardEndGame = queryXML.Element("board");
					Board bEndGame = XMLDecoder.xmlToBoard(xmlBoardEndGame);
					XElement xmlPlayerColors = queryXML.Element("set");
					List<string> playerColors = new List<string>();
					foreach (XElement playerColorXml in xmlPlayerColors.Descendants("color"))
					{
						playerColors.Add(XMLDecoder.xmlToColor(playerColorXml));
					}
					response = XMLEncoder.encodeVoid().ToString();
					break;
				default:
					throw new Exception("Outgoing competitor command not understand " +
										"the command " + command);
			}

			return response;
		}

		// Parses initialize XML and calls initialize on player
		// Returns void XML response
		public String initializeHandler(XElement initXML)
		{
			XMLDecoder.checkOrderOfTagsFromXML(new List<string> { "color", "list" }, 
			                                   initXML.Elements().ToList());

			String color = XMLDecoder.xmlToColor(initXML.Element("color"));
			List<string> playerOrder = XMLDecoder.xmlToListOfColors(initXML.Element("list"));

			player.initialize(color, playerOrder);

			// Return void
			return XMLEncoder.toString(XMLEncoder.encodeVoid());
		}

		// Parses play turn XML and calls play turn on player
        // Returns tile XML response
		public String playTurnHandler(XElement playTurnXML) {
			XMLDecoder.checkOrderOfTagsFromXML(new List<string> { "board", "set", "n" },
			                                   playTurnXML.Elements().ToList());

			Board b = XMLDecoder.xmlToBoard(playTurnXML.Element("board"));
			List<Tile> hand = XMLDecoder.xmlToListOfTiles(playTurnXML.Element("set"));
			int numTilesInDrawPile = XMLDecoder.xmlToNumber(playTurnXML.Element("n"));

			Tile tileToPlay = player.playTurn(b, hand, numTilesInDrawPile);
			String responseTile = XMLEncoder.toString(XMLEncoder.tileToXML(tileToPlay));
			return responseTile;
		}


	}
}
