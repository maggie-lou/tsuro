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
                            return new Posn(edgeRows[i], j, loc);
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
                            return new Posn(j, edgeCols[i], loc);
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

                //list of all moves from least to most symmetric
                List<Tile> allMovesOrdered = new List<Tile>();

                SortedDictionary<int, List<Tile>> sortedTiles = new SortedDictionary<int, List<Tile>>();

                foreach (Tile t in playerHand)
                {
                    int symmetry = t.howSymmetric();
                    if (sortedTiles.ContainsKey(symmetry))
                    {
                        sortedTiles[symmetry].Add(t);
                    }
                    else
                    {
                        sortedTiles.Add(symmetry, new List<Tile> { t });
                    }
                }

                foreach (KeyValuePair<int,List<Tile>> pair in sortedTiles)
                {
                    foreach (Tile t in pair.Value)
                    {
                        Tile checkTile = t;
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
                            allMovesOrdered.Add(checkTile);
                        }
                    }
                }

                //no valid moves, return the first tile from all moves possible
                if (validMoves.Count == 0)
                {
                    return allMovesOrdered[0];
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
