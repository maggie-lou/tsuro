using System;
using System.Xml.Linq;
using System.Collections.Generic;

namespace tsuro
{
	// Initializes an OutgoingCompetitor to compete in other tournaments over
    // the network, and responds to queries about that player
    public class NetworkAdmin
    {
		IPlayer player;
        public NetworkAdmin()
        {
        }
        
		public string getMethodReturnValue(string incomingData)
		{
			XElement incomingDataXML = XElement.Parse(incomingData);
			string method = incomingDataXML.Name.ToString();
			string output;
			switch (method)
			{
				case "get-name":
					output = XMLEncoder.nameToXML(player.getName());
					break;
				case "initialize":
					int i = 0;
					int j = 0;
					string color = "";
					List<string> listOfColors = new List<string>();
					foreach (XElement colorXML in incomingDataXML.Elements("color"))
					{
						color = XMLDecoder.xmlToColor(colorXML);
						i++;
					}
                    if (i != 1)
					{
						throw new Exception("Invalid input argument from initialize call from network.");
					}
                    foreach (XElement listOfColorsXML  in incomingDataXML.Elements("list"))
					{
						listOfColors = XMLDecoder.xmlToListOfColors(listOfColorsXML);
						j++;
					}
					if (j != 1)
                    {
                        throw new Exception("Invalid input argument from initialize call from network.");
                    }

					player.initialize(color, listOfColors);
					output = XMLEncoder.encodeVoid();
					break;
				case "place-pawn":
					break;
				case "play-turn":
					break;
				case "end-game":
					break;
				default:
					throw new Exception("Invalid method call");
			}
			return null;

		}

        // Accept connection from client
        // parse the tag from it
        // call the function associated with the tag
        // return the appropriate xml with the return values
      
	}
    
}
