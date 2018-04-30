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
        //returns the tile the player will play based on player strategy
        Tile playTurn(Board b, int dpc);
    }

    public class SPlayer:ISPlayer
    {
        //the player's color
        String color;
        //the 3 tiles in the players hand
        List<Tile> hand;

        IPlayers playerStrategy;
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
            if (lt.Count > 3)
            {
                throw new Exception("This is too many tiles to start with");
            }
            hand = lt;
            hasMoved = moved;
  
        }

        public SPlayer(String c, List<Tile> lt, bool moved, string strategyType, List<string> allPlayers)
        {
            color = c;
            if (lt.Count > 3)
            {
                throw new Exception("This is too many tiles to start with");
            }
            hand = lt;
            hasMoved = moved;
            if (strategyType == "Random")
            {
                playerStrategy = new RandomPlayer();
                playerStrategy.initialize(c, allPlayers);
            }
            else if (strategyType == "LeastSymmetric")
            {
                playerStrategy = new LeastSymmetricPlayer();
                playerStrategy.initialize(c, allPlayers);
            }
            else if (strategyType == "MostSymmetric")
            {
                playerStrategy = new MostSymmetricPlayer();
                playerStrategy.initialize(c, allPlayers);
            }
            else
            {
                throw new Exception("not a valid player strategy!");
            }
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
        // set the string of a player
        public void setColor(string col)
        {
            color = col;
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
            if (hand.Count > 3)
            {
                throw new Exception("Too many tiles in player"+ color +"'s hand");
            }
        }
        // remove a Tile from players hand
        //returns true if removing is successful, false otherwise
        public bool removeTileFromHand(Tile t)
        {
            if (hand.Exists(x => x.isEqual(t)))
            {
                Tile toBeRemoved = hand.Find(x => x.isEqual(t));
                hand.Remove(toBeRemoved);
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

        public Tile playTurn(Board b, int drawPileCount)
        {
            Tile tileToBePlayed = playerStrategy.playTurn(b, hand, drawPileCount);
            bool successful = removeTileFromHand(tileToBePlayed);
            return tileToBePlayed;
        }

        public Board placePawn(Board b)
        {
            int[] locArray = playerStrategy.placePawn(b);
            setPosn(locArray[0], locArray[1], locArray[2]);
            b.registerPlayer(this);
            return b;
        }
    }
}
