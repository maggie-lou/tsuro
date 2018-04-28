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
        // return grid location of the tile the player is on
        int getboardLocationRow();
        int getboardLocationCol();
        // return location on tile player is on
        int getLocationOnTile();
        // add a tile to the players hand
        void addTileToHand(Tile t);
        // remove a tile from players hand
        bool removeTileFromHand(Tile t);
        // set the position and location of a player on the board
        void setPosn(int r, int c, int TilePosn);
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
        //tells wheher the player has drawn the Dragon Tile
        public bool hasDragonTile = false;
        public SPlayer(String c, List<Tile> lt, bool moved)
        {
            color = c;
            hand = lt;
            hasMoved = moved;
        }

        public SPlayer()
        {
            color = null;
            hand = new List<Tile>();
            hasMoved = false;
        }

        // returns a string of the player
        public String returnColor()
        {
            return color;
        }
        // returns list of tiles in players hand
        public List<Tile> returnHand()
        {
            return hand;
        }

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

        // add a Tile to players hand
        public void addTileToHand(Tile t)
        {
            hand.Add(t);
        }
        // remove a Tile from players hand
        //returns true if removing is successful, false otherwise
        public bool removeTileFromHand(Tile t)
        {
            if (hand.Exists(x => x.isEqual(t)))
            {
                hand.Remove(t);
                return true;
            }
            return false;
        }
        
        // set the position and location of a player
        public void setPosn(int r, int c, int TilePosn)
        {
            row = r;
            col = c;
            currLoc = TilePosn;
        }
    }
}
