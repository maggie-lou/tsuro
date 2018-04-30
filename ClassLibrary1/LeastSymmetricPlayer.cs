using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tsuro
{
    public class LeastSymmetricPlayer: IPlayers
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

        public int[] placePawn(Board b)
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
                            return new int[] { edgeRows[i], j, loc };
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
                            return new int[] { j, edgeCols[i], loc };
                        }
                    }
                }
            }
            throw new Exception("Edges of Board are Full.");

        }
        public Tile playTurn(Board b, List<Tile> playerHand, int numTilesInDrawPile)
        {
            //ordered from least to most symmetric
            List<Tile> validMoves = new List<Tile>();
            if (playerHand.Count == 0)
            {
                throw new Exception("player hand is empty");
            }
            else
            {
                //order tiles from least to most symmetric
                List<Tile> orderedTiles = new List<Tile>();

                SortedDictionary<int, Tile> sortedTiles = new SortedDictionary<int, Tile>();

                foreach (Tile t in playerHand)
                {
                    sortedTiles.Add(t.howSymmetric(), t);
                }

                foreach (KeyValuePair<int,Tile> pair in sortedTiles)
                {
                    Tile checkTile = pair.Value;
                    int timesRotated = 0;
                    checkTile = checkTile.rotate();

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
                    }
                }

                //no valid moves, return the first tile
                if (validMoves.Count == 0)
                {
                    return playerHand[0];
                }

                return validMoves[0];
            }

        }
        public void endGame(Board b, List<string> allColors)
        {
            throw new NotImplementedException();

        }


    }
}
