using System;
//using System.box.Collections;
using System.Collections.Generic;


namespace tsuro
{
    interface IAdmin
    {
        bool legalPlay(SPlayer p, Board b, Tile t);
        TurnResult playATurn(List<Tile> pile, List<SPlayer> inGamePlayers,List<SPlayer> eliminatedPlayers,
            Board b, Tile t);
    }
    public class Admin:IAdmin
    {
        private bool tileInHand(SPlayer p, Tile t)
        {
            List<Tile> hand = p.returnHand();
            //if the tile is not (a possibly rotated version) of the tiles of the player
            if (hand == null)//if there are no tiles in the players hand
            {
                return false;
            }
            else//check all of the tiles in the players hand against t
            {
                foreach (Tile hTile in hand)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        hTile.rotate();
                        if (hTile.isEqual(t))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        private bool playerEliminated(SPlayer p, Board b, Tile t)
        {
            throw new NotImplementedException();
        }
        public bool legalPlay(SPlayer p, Board b, Tile t)
        {
            return (tileInHand(p, t) && playerEliminated(p, b, t));
        }

        public TurnResult playATurn(List<Tile> pile, List<SPlayer> inGamePlayers, List<SPlayer> eliminatedPlayers,
            Board b, Tile t)
        {
            throw new NotImplementedException();
        }




    }
}
