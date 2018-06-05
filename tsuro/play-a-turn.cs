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
    			Tile[,] boardGrid = XMLDecoder.xmlBoardToGrid(boardXml);
    			Dictionary<string, Posn> colorToPosnMap = XMLDecoder.xmlBoardToPlayerPosns(boardXml);
    			Tile tileToPlay = XMLDecoder.xmlToTile(tileToPlayXml);
    			SPlayer dragonTileHolder = null;

    			// Create list of active players from onBoardXml
    			List<SPlayer> activePlayers = new List<SPlayer>();
    			foreach (XElement splayerXml in onBoardPlayersXml.Elements())
    			{
    				string color = XMLDecoder.xmlSPlayerToColor(splayerXml);

    				List<Tile> playerHand = XMLDecoder.xmlSPlayerToHand(splayerXml);
    				SPlayer tempPlayer = new SPlayer(color, playerHand);
					tempPlayer.playerState = SPlayer.State.Playing;

    				if (colorToPosnMap[color] == null)
    				{
    					throw new Exception("Active player color was not found on Board player colors");

    				}
    				tempPlayer.setPosn(colorToPosnMap[color]);

    				if (XMLDecoder.SPlayerXmlIsDragonTileHolder(splayerXml))
    				{
    					dragonTileHolder = tempPlayer;
    				}

    				activePlayers.Add(tempPlayer);
    			}

				// Create list of eliminated players from onBoardXml
    			List<SPlayer> eliminatedPlayers = new List<SPlayer>();
    			foreach (XElement splayerXml in eliminatedPlayersXml.Elements())
    			{
    				string color = XMLDecoder.xmlSPlayerToColor(splayerXml);

    				List<Tile> playerHand = XMLDecoder.xmlSPlayerToHand(splayerXml);
    				SPlayer tempPlayer = new SPlayer(color, playerHand);
    				if (colorToPosnMap[color] == null)
    				{
    					throw new Exception("Eliminated player color was not found on Board player colors");

    				}
    				tempPlayer.setPosn(colorToPosnMap[color]);

    				eliminatedPlayers.Add(tempPlayer);
    			}

				Board boardWithAllInfo = new Board(drawPile, activePlayers, eliminatedPlayers, dragonTileHolder, boardGrid);

    			// Run our version of play a turn
    			Admin admin = new Admin();
    			TurnResult tr = admin.playATurn(boardWithAllInfo, tileToPlay);

    			//Convert our turn result into xml strings
    			string outDrawPileXml = XMLEncoder.listOfTilesToXML(tr.drawPile).ToString();
    			string outActivePlayersXml = XMLEncoder.listOfSPlayerToXML(tr.currentPlayers, tr.b).ToString();
    			string outEliminatedPlayersXml = XMLEncoder.listOfSPlayerToXML(tr.eliminatedPlayers, tr.b).ToString();
    			string outBoardXml = XMLEncoder.boardToXML(tr.b).ToString();
    			string outwinnersXML = "";

    			if (tr.playResult == null)
    			{
    				outwinnersXML = XMLEncoder.encodeFalse().ToString();
    			}
    			else
    			{
    				outwinnersXML = XMLEncoder.listOfSPlayerToXML(tr.playResult, tr.b).ToString();
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
