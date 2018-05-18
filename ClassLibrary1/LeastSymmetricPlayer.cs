using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tsuro
{
	[Serializable]
    public class LeastSymmetricPlayer: AutomatedPlayer, IPlayers
    {
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
                            if (b.isNotEliminationMove(currPlayer, checkTile))
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
    }
}
