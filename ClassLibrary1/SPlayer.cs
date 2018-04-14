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

        public List<Tile> returnHand()
        {
            return hand;
        }

        public String returnColor()
        {
            return color;
        }

        public SPlayer(String c, List<Tile> lt)
        {
            color = c;
            hand = lt;
        }
    }
}
