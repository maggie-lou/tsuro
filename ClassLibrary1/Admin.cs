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
        List<Tile> drawPile = new List<Tile>();

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
        }/*
        private bool playerEliminated(SPlayer p, Board b, Tile t)
        {
            return true;//throw new NotImplementedException();
        }*/
        public bool legalPlay(SPlayer p, Board b, Tile t)
        {
            return (tileInHand(p, t) && b.checkEliminated(p));//playerEliminated(p, b, t));
        }

        public TurnResult playATurn(List<Tile> pile, List<SPlayer> inGamePlayers, List<SPlayer> eliminatedPlayers,
            Board b, Tile t)
        {
            /*pile = drawPile;
            //the tile that has been drawn from the deck
            Tile drawnTile = t;
            //find the tile within the deck
            Tile temp = pile.Find(t);
            */
            throw new NotImplementedException();
        }




    }
}
