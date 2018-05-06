using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tsuro
{
    public class RandomPlayer : AutomatedPlayer, IPlayers
    {
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
    }
}
