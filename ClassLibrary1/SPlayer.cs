using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace tsuro
{
	[Serializable]
    public class SPlayer
    {
		private string color;
		private List<Tile> hand;
        private IPlayer playerStrategy;
		public State playerState = State.UnInit;
        public bool hasDragonTile = false;
  
        public enum State
        {
            UnInit = 0,
            Init = 1,
            Placed = 2,
            Playing = 3,
            Eliminated = 4
        };
      
		/*************** CONSTRUCTORS ****************************/

        public SPlayer(String c, List<Tile> lt, IPlayer strategyType = null, bool hasDragon = false)
        {
            color = c;
            hand = lt;
			playerStrategy = strategyType;
			hasDragonTile = hasDragon;
        }

		public SPlayer(IPlayer strategyType = null)
        {
            color = null;
            hand = new List<Tile>();
			playerStrategy = strategyType;
        }

        

		/*************** GETTERS ****************************/
        public String getColor()
        {
            return color;
        }

        public List<Tile> getHand()
        {
            return hand;
        }

		public bool isDragonHolder() {
			return hasDragonTile;
		}
        
		/*************** SETTERS ****************************/
        public void addTileToHand(Tile t)
        {
			if (hand.Count >= 3) {
				throw new TsuroException("Cannot add tile to hand - Hand count cannot exceed 3.");
			}

            hand.Add(t);
        }

        public void removeTileFromHand(Tile t)
        {
            if (!hand.Exists(x => x.isEqualOrRotation(t)))
            {
                throw new ArgumentException("Cannot remove tile from player's hand - player does not have this tile.");
            }

            Tile toBeRemoved = hand.Find(x => x.isEqualOrRotation(t));
            hand.Remove(toBeRemoved);
        }

		public void setStrategy(IPlayer strategy) {
			playerStrategy = strategy;
		}



		/*************** GAME PLAY FUNCTIONS ****************************/
        public void initialize(string color, List<string> playerOrderColors)
        {
            if(playerState != State.UnInit)
            {
				throw new TsuroException("Initialize being called on a player that is not" +
                    "uninitialized");
            }

			this.color = color;
			playerStrategy.initialize(color, playerOrderColors);
            playerState = State.Init;
        }
       

        public Tile playTurn(Board b, int drawPileCount)
        {
            if((playerState != State.Placed) && (playerState != State.Playing))
            {
				throw new TsuroException("Player is playing turn but is not " +
                    "in placed or playing state");
            }
            playerState = State.Playing;
   
            Tile tileToBePlayed = playerStrategy.playTurn(b, hand, drawPileCount);
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
                    if (hTile.isEqualOrRotation(t))
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
                    if (b.isEliminationMove(color, t_rotate))
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
			Posn startPos = playerStrategy.placePawn(b);
			b.addPlayerToBoard(color, startPos);

            playerState = State.Placed;
            return b;
        }

		public void setHand(List<Tile> Hand){
			hand = Hand;
		}

		public void eliminate() {
			playerState = SPlayer.State.Eliminated;
		}

		public int getHandSize() {
			return hand.Count;
		}
    }
}
