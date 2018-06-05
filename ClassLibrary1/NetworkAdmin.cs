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
					XElement xmlBoard = queryXML.Element("board");
					Board b = XMLDecoder.xmlToBoard(xmlBoard);
					XElement xmlPlayerHand = queryXML.Element("set");
					List<Tile> playerHand = new List<Tile>();
					foreach (XElement tileXml in xmlPlayerHand.Descendants("tile"))
					{
						playerHand.Add(XMLDecoder.xmlToTile(tileXml));
					}
					int numTilesInDrawPile = int.Parse(queryXML.Element("n").Value);
					Tile tileToPlay = player.playTurn(b, playerHand, numTilesInDrawPile);
					response = XMLEncoder.RemoveWhitespace(XMLEncoder.tileToXML(tileToPlay).ToString());
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

		// Parses initialize XML and calls on player
		// Returns void XML response
		public String initializeHandler(XElement initXML)
		{
			List<string> expectedTags = new List<string> { "color", "list" };
			bool validTags = XMLDecoder.checkOrderOfTagsFromXML(expectedTags, initXML.Elements().ToList());
			if (!validTags)
			{
				throw new Exception("Invalid initialize XML query from network.");
			}

			// Parse color
			String color = initXML.Element("color").Value;

			// Parse list of colors
			List<string> playerOrder = new List<string>();
			XElement colorListTree = initXML.Element("list");
			IEnumerable<XElement> colorIterator = colorListTree.Descendants();
			foreach (XElement colorXML in colorIterator)
			{
				if (colorXML.Name != "color")
				{
					throw new Exception("Invalid initialize XML query from network.");
				}
				playerOrder.Add(colorXML.Value);
			}

			// Call initialize on player
			player.initialize(color, playerOrder);

			// Return void
			return XMLEncoder.encodeVoid().ToString();
		}


	}
}
