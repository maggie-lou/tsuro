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
			//Console.WriteLine("Reading XMLs");
			List<XElement> inputXML = new List<XElement>();
            
            string line;

			for (int i = 0; i < 5; i++) {
				line = Console.ReadLine();
				inputXML.Add(XElement.Parse(line));
			}
			//while ((line = Console.ReadLine()) != null)
			//        {
				
			//    //Console.WriteLine(line + " " + count);
			//    inputXML.Add(XElement.Parse(line));
			//	count++;
			//}

			//inputXML.Add(XElement.Parse("<list><tile><connect><n>0</n><n>7</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>6</n></connect></tile><tile><connect><n>0</n><n>2</n></connect><connect><n>1</n><n>3</n></connect><connect><n>4</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>7</n></connect><connect><n>3</n><n>6</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>3</n></connect><connect><n>2</n><n>7</n></connect><connect><n>5</n><n>6</n></connect></tile><tile><connect><n>0</n><n>2</n></connect><connect><n>1</n><n>6</n></connect><connect><n>3</n><n>7</n></connect><connect><n>4</n><n>5</n></connect></tile><tile><connect><n>0</n><n>2</n></connect><connect><n>1</n><n>4</n></connect><connect><n>3</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>4</n></connect><connect><n>3</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile><tile><connect><n>0</n><n>2</n></connect><connect><n>1</n><n>7</n></connect><connect><n>3</n><n>4</n></connect><connect><n>5</n><n>6</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile><tile><connect><n>0</n><n>7</n></connect><connect><n>1</n><n>3</n></connect><connect><n>2</n><n>5</n></connect><connect><n>4</n><n>6</n></connect></tile><tile><connect><n>0</n><n>6</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>4</n></connect><connect><n>3</n><n>7</n></connect></tile><tile><connect><n>0</n><n>2</n></connect><connect><n>1</n><n>4</n></connect><connect><n>3</n><n>7</n></connect><connect><n>5</n><n>6</n></connect></tile><tile><connect><n>0</n><n>7</n></connect><connect><n>1</n><n>6</n></connect><connect><n>2</n><n>5</n></connect><connect><n>3</n><n>4</n></connect></tile><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect><connect><n>4</n><n>5</n></connect></tile><tile><connect><n>0</n><n>2</n></connect><connect><n>1</n><n>7</n></connect><connect><n>3</n><n>5</n></connect><connect><n>4</n><n>6</n></connect></tile><tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>7</n></connect><connect><n>4</n><n>6</n></connect></tile><tile><connect><n>0</n><n>7</n></connect><connect><n>1</n><n>2</n></connect><connect><n>3</n><n>4</n></connect><connect><n>5</n><n>6</n></connect></tile><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>5</n></connect><connect><n>3</n><n>6</n></connect><connect><n>4</n><n>7</n></connect></tile><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>4</n></connect><connect><n>3</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>7</n></connect><connect><n>3</n><n>4</n></connect><connect><n>5</n><n>6</n></connect></tile><tile><connect><n>0</n><n>5</n></connect><connect><n>1</n><n>6</n></connect><connect><n>2</n><n>7</n></connect><connect><n>3</n><n>4</n></connect></tile><tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>4</n><n>7</n></connect></tile><tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>2</n></connect><connect><n>4</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile><tile><connect><n>0</n><n>7</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>4</n></connect></tile><tile><connect><n>0</n><n>7</n></connect><connect><n>1</n><n>6</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect></tile></list>"));
			//inputXML.Add(XElement.Parse("<list><splayer-nodragon><color>blue</color><set><tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>4</n><n>7</n></connect></tile><tile><connect><n>0</n><n>6</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>4</n></connect><connect><n>3</n><n>7</n></connect></tile></set></splayer-nodragon><splayer-nodragon><color>red</color><set><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>4</n></connect><connect><n>3</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>6</n></connect><connect><n>2</n><n>5</n></connect><connect><n>4</n><n>7</n></connect></tile><tile><connect><n>0</n><n>6</n></connect><connect><n>1</n><n>3</n></connect><connect><n>2</n><n>5</n></connect><connect><n>4</n><n>7</n></connect></tile></set></splayer-nodragon><splayer-nodragon><color>green</color><set><tile><connect><n>0</n><n>2</n></connect><connect><n>1</n><n>3</n></connect><connect><n>4</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile><tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>7</n></connect><connect><n>4</n><n>6</n></connect></tile><tile><connect><n>0</n><n>7</n></connect><connect><n>1</n><n>2</n></connect><connect><n>3</n><n>4</n></connect><connect><n>5</n><n>6</n></connect></tile></set></splayer-nodragon><splayer-nodragon><color>orange</color><set><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>7</n></connect><connect><n>3</n><n>4</n></connect><connect><n>5</n><n>6</n></connect></tile><tile><connect><n>0</n><n>2</n></connect><connect><n>1</n><n>4</n></connect><connect><n>3</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>3</n></connect><connect><n>2</n><n>6</n></connect><connect><n>5</n><n>7</n></connect></tile></set></splayer-nodragon><splayer-nodragon><color>sienna</color><set><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>7</n></connect><connect><n>3</n><n>5</n></connect><connect><n>4</n><n>6</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>7</n></connect><connect><n>3</n><n>6</n></connect></tile><tile><connect><n>0</n><n>7</n></connect><connect><n>1</n><n>3</n></connect><connect><n>2</n><n>5</n></connect><connect><n>4</n><n>6</n></connect></tile></set></splayer-nodragon></list>"));
			//inputXML.Add(XElement.Parse("<list></list>"));
			//inputXML.Add(XElement.Parse("<board><map></map><map><ent><color>orange</color><pawn-loc><h></h><n>6</n><n>9</n></pawn-loc></ent><ent><color>red</color><pawn-loc><v></v><n>6</n><n>10</n></pawn-loc></ent><ent><color>sienna</color><pawn-loc><h></h><n>6</n><n>7</n></pawn-loc></ent><ent><color>blue</color><pawn-loc><v></v><n>0</n><n>1</n></pawn-loc></ent><ent><color>green</color><pawn-loc><h></h><n>6</n><n>1</n></pawn-loc></ent></map></board>"));
			//inputXML.Add(XElement.Parse("<tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>4</n></connect><connect><n>2</n><n>7</n></connect><connect><n>5</n><n>6</n></connect></tile>"));

			//Console.WriteLine("Reading XMLs");
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
			}else{
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
