using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace tsuro
{
	// A player that can compete in tournaments over the network
    // Responds to queries about next moves
    public class OutgoingCompetitor
    {
		protected IPlayer player;

		public OutgoingCompetitor(IPlayer player)
        {
			this.player = player;
        }

        // Parses and responses to an XML query about the competitor
        // Returns XML string of response
		public String interpretQuery(String query) {
			XElement queryXML = XElement.Parse(query);
			String command = queryXML.Name.ToString();
			String response = null;

			switch(command) {
				case "get-name":
					String name = player.getName();
					response = XMLEncoder.nameToXML(name);
					break;
				case "initialize":
					response = initializeHandler(queryXML);
					break;
				case "place-pawn":
					
					break;
				case "play-turn":
					break;
				case "end-game":
					break;
				default:
					throw new Exception("Outgoing competitor command not understand " +
										"the command " + command);
			}

			return response;
		}

        // Parses initialize XML and calls on player
		// Returns void XML response
		public String initializeHandler(XElement initXML) {
			List<string> expectedTags = new List<string> { "color", "list" };
			bool validTags = XMLDecoder.checkOrderOfTagsFromXML(expectedTags, initXML.Descendants().ToList());
			if (!validTags) {
				throw new Exception("Invalid initialize XML query from network.");
			}

			// Parse color
			String color = initXML.Element("color").ToString();

			// Parse list of colors
			List<string> playerOrder = new List<string>();
			XElement colorListTree = initXML.Element("list");
			IEnumerable<XElement> colorIterator = colorListTree.Descendants();
			foreach (XElement colorXML in colorIterator) {
				if (colorXML.Name != "color") {
					throw new Exception("Invalid initialize XML query from network.");
				}
				playerOrder.Add(colorXML.Value);
			}

			// Call initialize on player
			player.initialize(color, playerOrder);

			// Return void
			return XMLEncoder.encodeVoid();
		}
    }
}
