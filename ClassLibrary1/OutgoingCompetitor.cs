using System;
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
    }
}
