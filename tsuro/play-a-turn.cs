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
		//public class PlayerFields
   //     {
			//Posn posn;
			//List<Tile> hand;
			//bool isDragonTileHolder;
			//bool isOnBoard;
        //}
        static void Main(string[] args)
        {
			XElement queryXml = XElement.Parse(args[0]);
			XElement drawTilesXml = queryXml.Elements("list").ElementAt(0);
			XElement onBoardPlayersXml = queryXml.Elements("list").ElementAt(1);
			XElement eliminatedPlayersXml = queryXml.Elements("list").ElementAt(2);
			XElement boardXml = queryXml.Element("board");
			XElement tileToPlayXml = queryXml.Element("tile");

			List<Tile> drawPile = XMLDecoder.xmlToListOfTiles(drawTilesXml);
			Tile[,] boardGrid = XMLDecoder.xmlBoardToGrid(boardXml);
			Dictionary<string, Posn> colorToPosnMap = XMLDecoder.xmlBoardToPlayerPosns(boardXml);
			Tile tileToPlay = XMLDecoder.xmlToTile(tileToPlayXml);

			SPlayer dragonTileHolder = null;
			// create list of active players from onBoardXml
			List<SPlayer> activePlayers = new List<SPlayer>();
			foreach (XElement splayerXml in onBoardPlayersXml.Elements())
			{
				string color = XMLDecoder.xmlSPlayerToColor(splayerXml);

				List<Tile> playerHand = XMLDecoder.xmlSPlayerToHand(splayerXml);
				SPlayer tempPlayer = new SPlayer(color, playerHand);
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

			Board boardWithAllInfo = new Board(drawPile, activePlayers, eliminatedPlayers, dragonTileHolder);
        }


    }
}
