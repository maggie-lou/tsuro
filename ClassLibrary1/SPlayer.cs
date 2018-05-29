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

	[Serializable]
    public class SPlayer:ISPlayer
    {
        //the player's color
        String color;
        //the 3 tiles in the players hand
        List<Tile> hand;

        public IPlayer playerStrategy;
        public List<string> listOfColors = new List<string>();

        //variable which holds the player's position
        Posn playerPosn;

        //variable telling the player's state
        public enum State
        {
            UnInit = 0,
            Init = 1,
            Placed = 2,
            Playing = 3,
            Eliminated = 4
        };

        public State playerState = State.UnInit;
        //tells wheher the player has drawn the Dragon Tile
        public bool hasDragonTile = false;
        public SPlayer(String c, List<Tile> lt)
        {
            color = c;
            hand = lt;  
        }

        public SPlayer(String c, List<Tile> lt, string strategyType)
        {
            color = c;
            hand = lt;
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
        }

        public void initialize(Board b)
        {
            //this means player has not yet been initialized
            if(playerState != State.UnInit)
            {
                throw new Exception("initialize being called on a player that is not" +
                    "uninitialized");
            }

            foreach (SPlayer p in b.returnOnBoard())
            {
                listOfColors.Add(p.returnColor());
            }
            playerStrategy.initialize(color, listOfColors);
            playerState = State.Init;
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
            if((playerState != State.Placed) && (playerState != State.Playing))
            {
                throw new Exception("player is playing turn but is not " +
                    "in placed or playing state");
            }
            playerState = State.Playing;

            List<SPlayer> currentPlayers = b.returnOnBoard();

            // Update player's list of active player colors to be consistent with board's
            listOfColors = new List<string>();
            foreach (SPlayer p in currentPlayers)
            {
                listOfColors.Add(p.returnColor());
            }

            //is below redundant?
            //CONTRACT: ensure list of colors is consistent with board's list of colors
            foreach (SPlayer p in currentPlayers)
            {
                if (!listOfColors.Contains(p.returnColor()))
                {
                    throw new Exception("list of colors supplied in the board is not consistent with" +
                        "initialized list of colors");
                }
            }
            bool tileInHandOnBoard = false;
            bool duplicatesInHand = false;
            bool tooManyTilesInHand = false;

            //CONTRACT: No tile in Player's hand is rotation of another
            for (int i = 0; i < hand.Count - 1; i++)
            {
                for (int j = i + 1; j < hand.Count; j++)
                {
                    // Use list[i] and list[j]
                    duplicatesInHand = hand[i].isEqual(hand[j]);
                    if (duplicatesInHand)
                    {
                        Console.WriteLine("Player has duplicate tiles in hand.");
                        break;
                    }
                }
            }

            //CONTRACT: Player's hand is greater than 3
            if (hand.Count > 3)
            {
                Console.WriteLine("Player has more than 3 tiles in hand.");
                tooManyTilesInHand = true;
            }

            //CONTRACT: Player's set of tiles is not already placed on Board
            foreach (Tile t in hand)
            {
                tileInHandOnBoard = b.onBoardTiles.Exists(x => x.isEqual(t));
                if (tileInHandOnBoard)
                {
                    Console.WriteLine("Player's set of tiles is already on the board.");
                    break;
                }
            }

            if (tileInHandOnBoard || tooManyTilesInHand || duplicatesInHand)
            {
                Console.WriteLine(color + " is KICKED OUT of the game!" );
                playerStrategy = new RandomPlayer();
                playerStrategy.initialize(color, listOfColors);
            }
            Tile tileToBePlayed = playerStrategy.playTurn(b, hand, drawPileCount);
            // Two Illegal Moves
            // First if tile that playerStrategy returns eliminates player, then check if all tiles in player's
            // hand eliminates them
            // Second if tile that player chooses is not in it's hand

            if ((!b.isNotEliminationMove(this, tileToBePlayed)) && (!allMovesEliminatePlayer(b, tileToBePlayed)))
            {
                Console.WriteLine("Player played an illegal move. (Had a legal move in its hand)");
                Console.WriteLine(color + " is KICKED OUT of the game!");
                playerStrategy = new RandomPlayer();
                playerStrategy.initialize(color, listOfColors);
            }

            if (!tileInHand(tileToBePlayed))
            {
                Console.WriteLine("Player played an illegal move. (Tile played is not in hand)");
                Console.WriteLine(color + " is KICKED OUT of the game!");
                playerStrategy = new RandomPlayer();
                playerStrategy.initialize(color, listOfColors);
            }

            // Let Player play a turn
            tileToBePlayed = playerStrategy.playTurn(b, hand, drawPileCount);
            removeTileFromHand(tileToBePlayed);
            return tileToBePlayed;
        }

        public bool tileInHand(Tile t)
        {
            // check if Tile t is in the hand of the player
            if (hand.Count == 0) //if there are no tiles in the players hand
            {
                return false;
            }
            else // if there are tiles in the players hand
            {
                // check all of the tiles in the players hand against t
                foreach (Tile hTile in hand)
                {
                    if (hTile.isEqual(t))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool allMovesEliminatePlayer(Board b, Tile tileToBePlayed)
        {
            int elimTiles = 0;
            foreach (Tile t in hand)
            {
                for (int i = 0; i < 4; i++)
                {
                    Tile t_rotate = t.rotate();
                    if (!b.isNotEliminationMove(this, t_rotate))
                    {
                        elimTiles++;
                    }
                }
            }
            if (elimTiles == hand.Count * 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Board placePawn(Board b)
        {
            if(playerState != State.Init)
            {
                throw new Exception("player pawn is being placed but" +
                    "player is not in initialized state");
            }
			playerPosn = playerStrategy.placePawn(b);
            b.registerPlayer(this);

            playerState = State.Placed;
            return b;
        }
    }
}
