using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;



namespace tsuro
{
	
	public class Admin
	{
		private List<SPlayer> activePlayers;
        private List<SPlayer> eliminatedPlayers;
        private SPlayer dragonTileHolder;
        private List<Tile> drawPile;



		/*************** CONSTRUCTORS ****************************/

		public Admin() {
			activePlayers = new List<SPlayer>();
            eliminatedPlayers = new List<SPlayer>();
            dragonTileHolder = null;
            drawPile = new List<Tile>();
		}

		public Admin(List<SPlayer> active, List<SPlayer> eliminated, SPlayer dragonHolder, List<Tile> drawPile) {
			activePlayers = active;
			eliminatedPlayers = eliminated;
			dragonTileHolder = dragonHolder;
			this.drawPile = drawPile;
		}
      

		/*************** GETTERS ****************************/
      
		public List<string> getPlayerOrder()
        {
            List<string> listOfColors = new List<string>();
            foreach (SPlayer p in activePlayers)
            {
                listOfColors.Add(p.getColor());
            }
            return listOfColors;
        }

		public int getDrawPileSize()
        {
            return drawPile.Count;
        }
        
		public SPlayer getPlayer(string color) {
			// Look in active players
			SPlayer p = activePlayers.Find(x => color == x.getColor());

            // Look in eliminated players
			if (p == null) {
				p = eliminatedPlayers.Find(x => color == x.getColor());
			}
            
            // Could not find input color 
			if (p == null) {
				throw new TsuroException("Could not find player with color " + color);
			}
			return p;
		}

		// Returns a list of colors of all players, both eliminated and active
        public List<string> getAllPlayerColors()
        {
            List<string> listOfColors = new List<string>();
            foreach (SPlayer p in activePlayers)
            {
                listOfColors.Add(p.getColor());
            }
            foreach (SPlayer p in eliminatedPlayers)
            {
                listOfColors.Add(p.getColor());
            }
            return listOfColors;
        }

		// Returns a list of colors of active players
        public List<string> getActivePlayerColors()
        {
            List<string> listOfColors = new List<string>();
            foreach (SPlayer p in activePlayers)
            {
                listOfColors.Add(p.getColor());
            }
            return listOfColors;
        }

		public SPlayer getDragonTileHolder() {
			return dragonTileHolder;
		}

		public int numActive() {
			return activePlayers.Count;
		}

		public int numEliminated() {
			return eliminatedPlayers.Count;
		}

		public SPlayer getFirstActivePlayer() {
			if (activePlayers.Count == 0) {
				throw new TsuroException("Cannot get first active players - list is empty.");
			}
			return activePlayers[0];
		}


		/*************** SETTERS ****************************/
        public void addToActivePlayers(SPlayer p)
        {
            if (p != null)
            {
                activePlayers.Add(p);
            }
        }

		public void setDragonTileHolder(SPlayer p) {
			if (dragonTileHolder != null) {
				throw new TsuroException("Cannot set a new dragon tile holder - already one existing.");
			}
			dragonTileHolder = p;
		}


		/*************** PREDICATES ****************************/
		public bool isActivePlayer(string color) {
			return activePlayers.Find((x) => x.getColor() == color) != null;
		}

		public bool isEliminatedPlayer(string color)
        {
			return eliminatedPlayers.Find((x) => x.getColor() == color) != null;
        }

		public bool isDragonHolder(string color) {
			return dragonTileHolder.getColor() == color;
		}

		/*************** GAME PLAY FUNCTIONS ****************************/
        
        // Plays a game and returns a list of the winners
        //
        // Assumes all players have already been initialized
		public List<SPlayer> play(List<SPlayer> players) {
			initializeDrawPile("drawPilepaths.txt");
			Board b = new Board();
			activePlayers = players;

            // Set start positions
			foreach (SPlayer p in players) {
				p.placePawn(b);
			}

			dealTiles(players, b);

			// Continue game play until there are winners
            List<SPlayer> winners = null;
            while (winners == null)
            {
				SPlayer p = activePlayers[0];
				Tile tileToPlay = p.playTurn(b, drawPile.Count);

				// Check for cheating player
				bool cheating = isCheating(p, b, tileToPlay);
				if (cheating) {
					// Replace cheating player with random player
					Console.WriteLine(p.getColor() + " was cheating - being replaced with random player.");
					RandomPlayer replacementPlayer = new RandomPlayer();
					replacementPlayer.initialize(p.getColor(), getActivePlayerColors());
					p.setStrategy(replacementPlayer);

					tileToPlay = p.playTurn(b, drawPile.Count);
				}
                
                TurnResult tr = playATurn(b, tileToPlay);
    
                // Update status of game, based on turn
                winners = tr.playResult;
                b = tr.b;
            }

			return winners;
		}


		public bool legalPlay(SPlayer p, Board b, Tile t)
        {
            return (!b.isEliminationMove(p.getColor(), t)
				        || (b.isEliminationMove(p.getColor(), t)
                            && (p.allMovesEliminatePlayer(b, t))));
        }

        public TurnResult playATurn(Board b, Tile t)
        {
			if (activePlayers.Count == 0)
            {
                throw new TsuroException("Cannot play turn - No more active players on the board");
            }
            
			List<SPlayer> winners = null;
            SPlayer currentPlayer = activePlayers[0];         

            b.placeTile(currentPlayer.getColor(), t);

            // Move active players if newly placed tile affects them
			List<string> onEdgeColors = b.moveActivePlayers(getActivePlayerColors());

			// Eliminate on edge players
			bool isCurrentPlayerEliminated = false;
			List<SPlayer> onEdgePlayers = new List<SPlayer>();
			foreach (string playerColor in onEdgeColors) {
				eliminatePlayer(playerColor);
				onEdgePlayers.Add(getPlayer(playerColor));
				if (playerColor == currentPlayer.getColor()) {
					isCurrentPlayerEliminated = true;
				}
			}

            // Check if game is over
			bool gameOver = false;

			if (activePlayers.Count == 0)
            {
                // If all active players eliminated in the same turn, they all are winners
                winners = onEdgePlayers;
                gameOver = true;
			} 
			else if (b.getNumTilesOnBoard() == 35 || activePlayers.Count == 1)
			{
				// If all tiles played, all remaining players tie as winners
				// If only one active player left, he/she wins
				winners = activePlayers;
				gameOver = true;
			} 

            if (gameOver)
            {
				return new TurnResult(drawPile, activePlayers, eliminatedPlayers, b, winners);
            }
                     
            // Players draw
            if (dragonTileHolder != null)
            {
                drawTilesWithDragonHolder();
            }
            else
            {
                if (!isCurrentPlayerEliminated)
                {
					if (drawPile.Count == 0)
                    {
                        dragonTileHolder = currentPlayer;
                    }
                    else
                    {
                        currentPlayer.addTileToHand(drawTile());
                    }
                }
            }

            // Update game play order
            if (!isCurrentPlayerEliminated)
            {
				activePlayers.RemoveAt(0);
				activePlayers.Add(currentPlayer);
            }

            // Compute turn result
			return new TurnResult(drawPile, activePlayers, eliminatedPlayers, b, winners);
        }


        
		/*************** HELPER FUNCTIONS ****************************/

        // Returns whether a player is cheating
		public bool isCheating(SPlayer p, Board b, Tile t) {
			// Check legal play - t is in hand, and valid move
            // Valid moves cannot be elimination moves, unless there are no other options
			if (!legalPlay(p, b, t)) {
				Console.WriteLine("Cheating: non-legal play.");
				return true;
			}
             
			// Check that tile to be played is not a rotation of another tile in player's hand
			List<Tile> hand = p.getHand();
			for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].isEqualOrRotation(t))
                {
                    Console.WriteLine("Cheating: player has duplicate tiles in hand.");
                    return true;
                }
            }

			//for (int i = 0; i < hand.Count - 1; i++)
     //       {
     //           for (int j = i + 1; j < hand.Count; j++)
     //           {
					//if (hand[i].isEqualOrRotation(hand[j]))
      //              {
						//Console.WriteLine("Cheating: player has duplicate tiles in hand.");
						//return true;
            //        }
            //    }
            //}

            // Check number of tiles in hand
			if (hand.Count > 3)
            {
				Console.WriteLine("Cheating: player has more than 3 tiles in hand.");
				return true;
            }

            // Check player's tiles are not already on board 
			foreach (Tile tile in hand)
            {
                if (b.tileExistsOnBoard(tile))
                {
					Console.WriteLine("Cheating: player's set of tiles is already on the board.");
					return true;
                }
            }

			return false;
		}

		// Eliminates a player
        //
        // Removes from active player list, adds to eliminated player list,
        // returns  all tiles to the draw pile
        public void eliminatePlayer(string color)
        {
			SPlayer p = activePlayers.Find(x => color == x.getColor());
			if (p == null)
            {
                throw new TsuroException("Cannot find an active player with color " + color + " to eliminate.");
            }
            if ((p.playerState != SPlayer.State.Placed) && (p.playerState != SPlayer.State.Playing))
            {
                throw new TsuroException("Player is being eliminated before having placed a start pawn.");
            }


            // Add eliminated player's tiles to draw pile, and remove from his/her hand
            if (p.getHandSize() != 0)
            {
                List<Tile> hand = p.getHand();
                for (int i = 0; i < hand.Count; i++)
                {
                    Tile tempTile = hand[i];
                    p.removeTileFromHand(tempTile);
                    addTileToDrawPile(tempTile);
                    i--;
                }
            }


            // Eliminated player is dragon tile holder
            if (dragonTileHolder != null && dragonTileHolder.getColor() == p.getColor())
            {
                // Get index of eliminated player in active players list 
                int currIndex = activePlayers.FindIndex(x => p.getColor() == x.getColor()); ;

                // Pass dragon tile to next player with less than 3 tiles in hand
                SPlayer nextPlayer;
                do
                {
                    currIndex += 1;
                    nextPlayer = activePlayers[(currIndex) % activePlayers.Count];
                } while (nextPlayer.getHandSize() >= 3);

                if (nextPlayer.getColor() == p.getColor())
                {
                    // Cannot find player with fewer than 3 tiles in hand
                    dragonTileHolder = null;
                }
                else
                {
                    dragonTileHolder = nextPlayer;
                }
            }

            eliminatedPlayers.Add(p);
            activePlayers.Remove(p);
            p.eliminate();
        }


		public void drawTilesWithDragonHolder()
        {
            if (dragonTileHolder == null)
            {
                throw new Exception("There is not a dragon tile holder.");
            }


			if (drawPile.Count != 0)
            {
                int dragonHolderIndex = activePlayers.FindIndex(x =>
                                                          x.getColor()
                                                          == dragonTileHolder.getColor());
                int toDrawIndex = dragonHolderIndex;
                SPlayer nextPlayerToDraw = dragonTileHolder;
                do
                {
                    nextPlayerToDraw.addTileToHand(drawTile());
                    toDrawIndex++;
                    nextPlayerToDraw = activePlayers[(toDrawIndex)
					                                 % activePlayers.Count];
                } while (drawPile.Count != 0 &&
                         nextPlayerToDraw.getHandSize() < 3);

                // if nextPlayer has 3 tiles in its hand, set dragonTileHolder back to null
                if (nextPlayerToDraw.getHandSize() < 3)
                {
                    dragonTileHolder = nextPlayerToDraw;
                }
                else
                {
                    dragonTileHolder = null;
                }
            }
        }


        // Initializes full draw pile, and returns it 
		public List<Tile> initializeDrawPile(string filename)
		{
			List<Tile> drawPile = new List<Tile>();
			StreamReader reader = File.OpenText(filename);
			string tile;
			while ((tile = reader.ReadLine()) != null)
			{
				List<Path> tilePaths = new List<Path>();
				string[] paths = tile.Split(',');
				foreach (string path in paths)
				{
					string[] locs = path.Split(' ');
					int i = 0;
					int[] loc_arr = new int[2];
					foreach (string loc in locs)
					{
						loc_arr[i] = int.Parse(loc);
						i++;
					}
					Path newPath = new Path(loc_arr[0], loc_arr[1]);
					tilePaths.Add(newPath);
				}
				Tile newTile = new Tile(tilePaths);
				drawPile.Add(newTile);
			}

			this.drawPile = drawPile;
			return drawPile;
		}

		public void addTileToDrawPile(Tile t)
        {
            drawPile.Add(t);
        }

		// Returns first tile from draw pile
        public Tile drawTile()
        {
            if (drawPile.Count == 0)
            {
                throw new TsuroException("Cannot draw tile from an empty pile.");
            }

            Tile t = drawPile[0];
            drawPile.RemoveAt(0);
            return t;
        }

		public void dealTiles(List<SPlayer> activePlayers, Board b)
		{
			foreach (SPlayer p in activePlayers)
			{
				for (int i = 0; i < 3; i++)
				{
					Tile t = drawTile();
					p.addTileToHand(t);
				}
			}
		}


      
	}

}
