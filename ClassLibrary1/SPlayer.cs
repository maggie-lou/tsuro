using System;
using System.Collections.Generic;
using System.Text;

namespace tsuro
{
    interface ISPlayer
    {
        //returns the color of the player
        String returnColor();
        //returns the tiles in the Player's hand 
        List<Tile> returnHand();
    }

    public class SPlayer:ISPlayer
    {
        //the player's color
        String color;
        //the 3 tiles in the players hand
        List<Tile> hand;

        //the location of the player on the tile   
        int currLoc;

        //tells where the player is on the larger board
        int row;
        int col;

        //tells whether the player has ever been moved by it's own turn
        // or the move of another player
        public bool hasMoved = false;

        //returns locations of the tile the player is on 
        public int getboardLocationRow()
        {
            return row;
        }
        public int getboardLocationCol()
        {
            return col;
        }
        //returns location of player on a given tile
        public int getLocationOnTile()
        {
            return currLoc;
        }

        public List<Tile> returnHand()
        {
            return hand;
        }

        public void addTileToHand(Tile t)
        {
            hand.Add(t);
        }

        public void removeTileFromHand(Tile t)
        {
            hand.Remove(t);
        }

        public String returnColor()
        {
            return color;
        }

        public SPlayer(String c, List<Tile> lt, bool moved)
        {
            color = c;
            hand = lt;
            hasMoved = moved;
        }

        public void setPosn(int r, int c, int TilePosn)
        {
            row = r;
            col = c;
            currLoc = TilePosn;
        }
    }
}
