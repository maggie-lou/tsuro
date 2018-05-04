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
        // return player's position object corresponding to location on board
        Posn getPlayerPosn();
        //add a tile to the player's hand
        void addTileToHand(Tile t);
        // remove a tile from players hand
        bool removeTileFromHand(Tile t);
        // set the position and location of a player on the board
        void setPosn(Posn p);
        //returns the tile the player will play based on player strategy
        Tile playTurn(Board b, int dpc);
    }

    public class SPlayer:ISPlayer
    {
        //the player's color
        String color;
        //the 3 tiles in the players hand
        List<Tile> hand;

        public IPlayers playerStrategy;
        public List<string> listOfColors = new List<string>();

        Posn playerPosn;

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

        public SPlayer(String c, List<Tile> lt, bool moved, string strategyType)
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
            }
            else if (strategyType == "LeastSymmetric")
            {
                playerStrategy = new LeastSymmetricPlayer();
            }
            else if (strategyType == "MostSymmetric")
            {
                playerStrategy = new MostSymmetricPlayer();
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

        public void initialize(Board b)
        {
            foreach (SPlayer p in b.returnOnBoard())
            {
                listOfColors.Add(p.returnColor());
            }
            playerStrategy.initialize(color, listOfColors);
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
        public Posn getPlayerPosn()
        {
            return playerPosn;
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
        public void setPosn(Posn p)
        {
            playerPosn = p;
        }

        public Tile playTurn(Board b, int drawPileCount)
        {
            Tile tileToBePlayed = playerStrategy.playTurn(b, hand, drawPileCount);
            bool successful = removeTileFromHand(tileToBePlayed);
            return tileToBePlayed;
        }

        public Board placePawn(Board b)
        {
            Posn toBePlaced = playerStrategy.placePawn(b);
            playerPosn = toBePlaced;
            b.registerPlayer(this);
            return b;
        }
    }
}
