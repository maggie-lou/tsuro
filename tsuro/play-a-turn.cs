using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace tsuro
{
    class playaturn
    {
		static void Main(string[] args)
		{
			while (true) {
    			List<XElement> inputXML = new List<XElement>();

    			string line;

    			for (int i = 0; i < 5; i++)
    			{
    				line = Console.ReadLine();

                    // End execution when game is over
					if (line == null) {
						return;
					}
    				inputXML.Add(XElement.Parse(line));
    			}         
    			XElement drawTilesXml = inputXML[0];
    			XElement onBoardPlayersXml = inputXML[1];
    			XElement eliminatedPlayersXml = inputXML[2];
    			XElement boardXml = inputXML[3];
    			XElement tileToPlayXml = inputXML[4];
                
    			List<Tile> drawPile = XMLDecoder.xmlToListOfTiles(drawTilesXml);
				Board b = XMLDecoder.xmlToBoard(boardXml);
    			Tile tileToPlay = XMLDecoder.xmlToTile(tileToPlayXml);
                
    			SPlayer dragonTileHolder = null;
    			List<SPlayer> activePlayers = new List<SPlayer>();
    			foreach (XElement splayerXml in onBoardPlayersXml.Elements())
    			{
					SPlayer tempPlayer = XMLDecoder.xmlToSplayer(splayerXml);
					tempPlayer.playerState = SPlayer.State.Playing;
                    
					if (tempPlayer.isDragonHolder())
    				{
						if (dragonTileHolder != null) {
							throw new TsuroException("Cannot set multiple dragon tile holders.");
						}
    					dragonTileHolder = tempPlayer;
    				}
    				activePlayers.Add(tempPlayer);
    			}

    			List<SPlayer> eliminatedPlayers = new List<SPlayer>();
    			foreach (XElement splayerXml in eliminatedPlayersXml.Elements())
    			{
					SPlayer tempPlayer = XMLDecoder.xmlToSplayer(splayerXml);
    				eliminatedPlayers.Add(tempPlayer);
    			}
                
    			// Run our version of play a turn
				Admin admin = new Admin(activePlayers, eliminatedPlayers, dragonTileHolder, drawPile);
    			TurnResult tr = admin.playATurn(b, tileToPlay);
                
    			//Convert our turn result into xml strings
    			string outDrawPileXml = XMLEncoder.listOfTilesToXML(tr.drawPile).ToString();
    			string outActivePlayersXml = XMLEncoder.listOfSPlayerToXML(tr.currentPlayers, admin).ToString();
    			string outEliminatedPlayersXml = XMLEncoder.listOfSPlayerToXML(tr.eliminatedPlayers, admin).ToString();
    			string outBoardXml = XMLEncoder.boardToXML(tr.b).ToString();
    			string outwinnersXML;

    			if (tr.playResult == null)
    			{
    				outwinnersXML = XMLEncoder.encodeFalse().ToString();
    			}
    			else
    			{
    				outwinnersXML = XMLEncoder.listOfSPlayerToXML(tr.playResult, admin).ToString();
    			}

    			// Print XML Strings out through stdout
    			Console.WriteLine(XMLEncoder.RemoveWhitespace(outDrawPileXml));
    			Console.WriteLine(XMLEncoder.RemoveWhitespace(outActivePlayersXml));
    			Console.WriteLine(XMLEncoder.RemoveWhitespace(outEliminatedPlayersXml));
    			Console.WriteLine(XMLEncoder.RemoveWhitespace(outBoardXml));
    			Console.WriteLine(XMLEncoder.RemoveWhitespace(outwinnersXML));

    			Console.Out.WriteLine();
		   }
        }
    }
}
