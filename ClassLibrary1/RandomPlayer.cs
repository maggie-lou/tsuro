using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tsuro
{
    public class RandomPlayer : IPlayers
    {
        string name = "";
        List<string> allPlayers = new List<string>();
        string[] validNames = new string[] {"blue","red","green","orange","sienna"
            ,"hotpink","darkgreen","purple"};

        public string getName()
        {
            return name;
        }

        public void initialize(string playerColor, List<string> allColors)
        {
            allPlayers = allColors;
            if (validNames.Contains(playerColor))
            {
                name = playerColor;
            }
            else
            {
                throw new Exception("not a valid color!");
            }
        }

        public Posn placePawn(Board b)
        {
            // row is either 0(0 and 1) or 5(4 and 5)
            // col is either 0(6 and 7) or 5(2 and 3)
            int[] edgeRows = new int[] { 0, 5 };
            int[] edgeCols = new int[] { 0, 5 };
            Dictionary<int, int[]> edgeRowLoc = new Dictionary<int, int[]>();
            edgeRowLoc.Add(0, new int[] { 0, 1 });
            edgeRowLoc.Add(5, new int[] { 4, 5 });
            Dictionary<int, int[]> edgeColLoc = new Dictionary<int, int[]>();
            edgeColLoc.Add(0, new int[] { 6, 7 });
            edgeColLoc.Add(5, new int[] { 2, 3 });

            for (int i = 0; i < edgeRows.Length; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    foreach (int loc in edgeRowLoc[edgeRows[i]])
                    {
                        if (!b.locationOccupied(edgeRows[i], j, loc))
                        {
                            return new Posn( edgeRows[i], j, loc );
                        }
                    }
                }
            }

            for (int i = 0; i < edgeCols.Length; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    foreach (int loc in edgeColLoc[edgeCols[i]])
                    {
                        if (!b.locationOccupied(j, edgeCols[i], loc))
                        {
                            return new Posn(j, edgeCols[i], loc );
                        }
                    }
                }
            }
            throw new Exception("Edges of Board are Full.");
        }

        public Tile playTurn(Board b, List<Tile> playerHand, int numTilesInDrawPile)
        {

            List<Tile> validMoves = new List<Tile>();
            List<Tile> allMoves = new List<Tile>();
            
            Tile toPlayTile;
            if (playerHand.Count == 0)
            {
                throw new Exception("player hand is empty");
            }
            else
            {
                foreach (Tile t in playerHand)
                {
                    int timesRotated = 0;
                    Tile checkTile = t.rotate();
                    while (timesRotated < 4)
                    {
                        SPlayer currPlayer = b.returnOnBoard().Find(x => x.returnColor() == name);
                        if (currPlayer.returnColor() == null)
                        {
                            throw new Exception("Player not found on board!");
                        }
                        if (b.checkPlaceTile(currPlayer, checkTile))
                        {
                            validMoves.Add(checkTile);
                            break;
                        }
                        else
                        {
                            checkTile = checkTile.rotate();
                            timesRotated = timesRotated + 1;
                        }
                        allMoves.Add(checkTile);
                    }
                }

                if (validMoves.Count == 0)
                {
                    Random rAll = new Random();
                    int rIntAll = rAll.Next(0, allMoves.Count);
                    toPlayTile = playerHand.Find(x => x.isEqual(allMoves[rIntAll]));
                    return allMoves[rIntAll];
                }

                Random r = new Random();
                int rInt = r.Next(0, validMoves.Count);
                toPlayTile = playerHand.Find(x => x.isEqual(validMoves[rInt]));
                return validMoves[rInt];
            }
        }

        public void endGame(Board b, List<string> allColors)
        {
            throw new NotImplementedException();
        }
    }
}
