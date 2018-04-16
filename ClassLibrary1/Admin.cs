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
        private List<Tile> drawPile = new List<Tile>();

        public void addTileToDrawPile(Tile t)
        {
            drawPile.Add(t);
        }

        public bool tileInHand(SPlayer p, Tile t)
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
        private bool playerOnEdge(SPlayer p, Board b, Tile t)
        {
            b.placeTile(t, p);
        }*/
        //checks if player collides into another
        //public bool playerCollides(SPlayer p, Board b, Tile t)

        public bool legalPlay(SPlayer p, Board b, Tile t)
        {
            return (tileInHand(p, t) && b.checkPlaceTile(p,t));
        }

        public Tile drawATile()
        {
            Tile drawTile;
            if(drawPile.Count != 0)
            {
                drawTile = drawPile[0];
                drawPile.Remove(drawTile);
                return drawTile;
            }
            return null;
        }
        public TurnResult playATurn(List<Tile> pile, List<SPlayer> inGamePlayers, List<SPlayer> eliminatedPlayers,
            Board b, Tile t)
        {
            drawPile = pile;
            //if there are no players in the game
            if(inGamePlayers.Count == 0)
            {
                TurnResult tr = new TurnResult(pile, inGamePlayers, eliminatedPlayers, b, null);
                return tr;
            }

            SPlayer tempPlayer = inGamePlayers[0];
            bool playWasLegal = b.checkPlaceTile(tempPlayer, t);
            if (playWasLegal)
            {
                SPlayer currentPlayer = b.placeTile(tempPlayer, t);
                //draw a tile
                //remove tile
                Tile drawnTile = drawATile();
                if(drawnTile != null)
                {
                    currentPlayer.addTileToHand(drawnTile);
                }
                //remove old player, add player at new location to end of list 
                inGamePlayers.Remove(tempPlayer);
                inGamePlayers.Add(currentPlayer);
                TurnResult tr = new TurnResult(pile, inGamePlayers, eliminatedPlayers, b, null);
                return tr;
            }
            else
            {
                if (tempPlayer.returnHand().Count == 0)
                {
                    SPlayer currentPlayer = b.placeTile(tempPlayer, t);
                    inGamePlayers.Remove(currentPlayer);
                    eliminatedPlayers.Add(currentPlayer);
                    TurnResult tr = new TurnResult(pile, inGamePlayers, eliminatedPlayers, b, null);
                    return tr;
                }
                
            }
            /*pile = drawPile;
            //the tile that has been drawn from the deck
            Tile drawnTile = t;
            //find the tile within the deck
            Tile temp = pile.Find(t);
            */
            return null;
        }




    }
}
