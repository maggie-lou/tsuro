using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace tsuro
{
    interface IBoard
    {
        // returns list of SPlayers that are eliminated
        List<SPlayer> returnEliminated();
        
        // eliminate a player by removing from inGamePlayers and adding to eliminatedPlayers
        void eliminatePlayer(SPlayer p);
        //registers players in the beginning of the game
        void registerPlayer(SPlayer p);
        // returns an array of [row, col] of next grid location the player can place a tile
        int[] nextTileCoord(Posn p);
        // Takes in a SPlayer and a Tile and returns whether that player can place the tile
        // (this means that it does not lead a player to an edge if they have other tiles
        // and that when you first start, you are on the tile that you will place next)
        bool isNotEliminationMove(SPlayer p, Tile t);
        // returns whether SPlayer is on an edge
        bool onEdge(Posn p);
        // takes in a Tile, the current location of a player on its current tile
        // and a bool indicating if the player is starting on the edge
        // returns an int of new location on Tile t
        int getEndOfPathOnTile(Tile t, int currTilePosn);
        // Places a tile on the board and returns end position of player after following all paths,
          // but does not set it
        Posn placeTile(SPlayer p, Tile t);
        // returns whether a grid location already has a tile
        bool occupied(int row, int col);
        // Takes in a start position - returns end position after recursively 
        // moving across tiles
        // Does not modify any players
        Posn moveMockPlayer(Posn p);
        
    }

	[Serializable]
	public class Board : IBoard
	{
		public Tile[,] grid = new Tile[6, 6]; // grid of tiles placed on the board
		public List<SPlayer> onBoard = new List<SPlayer>(); // list of players on the board
		public List<SPlayer> eliminated = new List<SPlayer>(); // list of eliminated players
		public List<SPlayer> eliminatedButWinners = null; // when all players eliminated in one turn
														  // they all become winners
		SPlayer dragonTileHolder = null; //set to the player which is holding the dragon tile 
		public List<Tile> drawPile = new List<Tile>();
		public List<Tile> onBoardTiles = new List<Tile>(); // Redundant

		public Board(){
		}
		public Board(Tile[,] tempgrid){
			grid = tempgrid;
		}
		public Board(List<Tile> drawPile, List<SPlayer> activePlayers, List<SPlayer> eliminatedPlayers, SPlayer dragonHolder){
			this.drawPile = drawPile;
			this.onBoard = activePlayers;
			this.eliminated = eliminatedPlayers;
			this.dragonTileHolder = dragonHolder;
		}

		public Board(List<Tile> drawPile, List<SPlayer> activePlayers, List<SPlayer> eliminatedPlayers, SPlayer dragonHolder, Tile[,] grid)
        {
            this.drawPile = drawPile;
            this.onBoard = activePlayers;
            this.eliminated = eliminatedPlayers;
            this.dragonTileHolder = dragonHolder;
			this.grid = grid;
        }

			
        public void addTileToDrawPile(Tile t)
        {
            drawPile.Add(t);
        }

        public Tile drawATile()
        {
            Tile drawTile;
            // if the drawpile is not empty
            if (drawPile.Count != 0)
            {
                // get the first tile
                drawTile = drawPile[0];
                // remove this tile from the drawpile
                drawPile.Remove(drawTile);
                return drawTile;
            }
            // return null if drawpile is empty
            return null;
        }

        public List<SPlayer> returnEliminated()
        {
            return eliminated;
        }


		public SPlayer getActiveSPlayer(string color) {
			if (!isOnBoard(color)) {
				throw new Exception("There is not a player with that color, either active or eliminated.");
			}
			return onBoard.Find(x => x.returnColor() == color);
		}
        
		public List<string> getPlayerOrder() {
			List<string> listOfColors = new List<string>();
			foreach (SPlayer p in onBoard)
            {
                listOfColors.Add(p.returnColor());
            }
			return listOfColors;
		}

		public List<string> getAllColorsOnBoard()
        {
            List<string> listOfColors = new List<string>();
            foreach (SPlayer p in onBoard)
            {
                listOfColors.Add(p.returnColor());
            }
			foreach (SPlayer p in eliminated)
            {
                listOfColors.Add(p.returnColor());
            }
            return listOfColors;
        }

        // Eliminates a player by removing from active player list, adding to 
        // eliminated player list, and returning all tiles to the draw pile
		public void eliminatePlayer(SPlayer p)
        {
            if ((p.playerState != SPlayer.State.Placed) && (p.playerState != SPlayer.State.Playing))
            {
                throw new Exception("Player is being eliminated before having placed a start pawn.");
			} else if (!onBoard.Contains(p)) {
				throw new Exception("Trying to eliminate a non-active player.");
			}


			// Add eliminated player's tiles to draw pile, and remove from his/her hand
			if(p.getHandSize() != 0)
            {
				List<Tile> hand = p.returnHand();
				int handSize = hand.Count;
				for (int i = 0; i < hand.Count; i++) {
					Tile tempTile = hand[i];
					p.removeTileFromHand(tempTile);
                    addTileToDrawPile(tempTile);
					i--;
				}
            }

			int onBoardIndex = onBoard.FindIndex(x => p.returnColor() == x.returnColor());

			if(dragonTileHolder != null && dragonTileHolder.returnColor() == p.returnColor())
            {

				// Pass dragon tile to next player with less than 3 tiles in hand
				int currIndex = onBoardIndex;
				SPlayer nextPlayer;
				do
				{
					currIndex += 1;
					nextPlayer = onBoard[(currIndex) % onBoard.Count];
				} while (nextPlayer.getHandSize() >= 3);
            
                dragonTileHolder = nextPlayer;
            }
            
            eliminated.Add(p);
            onBoard.Remove(p);
			p.eliminate();
        }

        public void registerPlayer(SPlayer p)
        {
            if (p != null)
            {
                onBoard.Add(p);
            }
        }

        public bool existsDragonTileHolder()
        {
            return dragonTileHolder != null;
        }

        public void setDragonTileHolder(SPlayer p)
        {
            dragonTileHolder = p;
        }

        public int[] nextTileCoord(Posn p)
        {
			int currentRow = p.returnRow();
			int currentCol = p.returnCol();
			int currentTilePosn = p.returnLocationOnTile();

			int[] nextCoord = new int[] {currentRow, currentCol};

			switch(currentTilePosn) 
			{
				case 0: case 1:
					nextCoord[0] = currentRow - 1;
					break;
				case 2: case 3:
					nextCoord[1] = currentCol + 1;
					break;
				case 4: case 5:
					nextCoord[0] = currentRow + 1;
					break;
				case 6: case 7:
					nextCoord[1] = currentCol - 1;
					break;	
			}

			return nextCoord;
        }

        public bool isNotEliminationMove(SPlayer p, Tile t)
        {
            // Get next tile position
			Posn playerPosn = p.getPlayerPosn();
			int[] newGridLoc = nextTileCoord(playerPosn);

			// Put tile on mock board
			Board mockBoard = this.clone();
			mockBoard.grid[newGridLoc[0], newGridLoc[1]] = t;

			// Move player on fake board
			Posn endPos = mockBoard.moveMockPlayer(playerPosn);

			// See if elimination move
			return !onEdge(endPos);
        }

		public Board clone() {
			Board copy = DeepClone<Board>(this);
			return copy;
		}

		public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
        
        public bool onEdge(Posn p)
        {
			int row = p.returnRow();
			int col = p.returnCol();
			int tilePos = p.returnLocationOnTile();

			if (row == 0 && (col != -1 && col != 6))
            {
				if (tilePos == 0 || tilePos == 1) {
					return true;
				}
         
            }
			if (row == 5 && (col != -1 && col != 6))
            {
                if (tilePos == 4 || tilePos == 5)
                {
                    return true;
                }

            }

			if (col == 0 && ( row != -1 && row != 6))
            {
				if (tilePos == 6 || tilePos == 7)
                {
                    return true;
                }
            }
			if (col == 5 && (row != -1 && row != 6))
            {
				if (tilePos == 2 || tilePos == 3)
                {
                    return true;
                }
            }
            return false;
        }

		//maps player onto next tile's duplicate tile location and finds end of the path on the new tile
		public int getEndOfPathOnTile(Tile t, int currTilePosn)
		{
			int newTilePosn = 0;

			if (currTilePosn == 0)
			{
				newTilePosn = t.getLocationEnd(5);
			}
			else if (currTilePosn == 5)
			{
				newTilePosn = t.getLocationEnd(0);
			}
			else if (currTilePosn == 1)
			{
				newTilePosn = t.getLocationEnd(4);
			}
			else if (currTilePosn == 4)
			{
				newTilePosn = t.getLocationEnd(1);
			}
			else if (currTilePosn == 2)
			{
				newTilePosn = t.getLocationEnd(7);
			}
			else if (currTilePosn == 7)
			{
				newTilePosn = t.getLocationEnd(2);
			}
			else if (currTilePosn == 3)
			{
				newTilePosn = t.getLocationEnd(6);
			}
			else if (currTilePosn == 6)
			{
				newTilePosn = t.getLocationEnd(3);
			}
			return newTilePosn;
		}

        public Posn placeTile(SPlayer p, Tile t)
        {
            int[] newGridLoc = new int[2];
            Posn playerPosn = p.getPlayerPosn();

            // if player is not on the edge, if it is not the players first turn anymore
            // set new grid location to be the next location that player can place tile in
            newGridLoc = nextTileCoord(playerPosn);

            // get the current player location on their current tile
            int currentTilePosn = playerPosn.returnLocationOnTile();
            // get the new player location on the next tile
            int newTilePosn = getEndOfPathOnTile(t, currentTilePosn);

            int newRow = newGridLoc[0];
            int newCol = newGridLoc[1];
            // set the next grid location on the board to be the tile
            grid[newRow, newCol] = t;
            // add the tile to list of played tiles
            onBoardTiles.Add(t);

            // Calculate end position of player on new tile
			Posn endPos = new Posn(newRow, newCol, newTilePosn);
			// Calculate end position of player if additional tiles to move across
			endPos = moveMockPlayer(endPos);
			return endPos;
        }

        public bool occupied(int row, int col)
        {
            if (grid[row,col] != null)
            {
                return true;
            }
            return false;
        }
        
        public Posn moveMockPlayer(Posn startPos)
        {
            if (onEdge(startPos))
            {
                return startPos;
            }

            int[] nextCoord = nextTileCoord(startPos);
			int nextRow = nextCoord[0];
			int nextCol = nextCoord[1];
         
            // End recursion if no more tiles along path
			if (!occupied(nextRow, nextCol))
            {
                return startPos;
            }
            else // Recursively follow path
            {
				// set the current location of the player to be the the next grid location
				Tile nextTile = grid[nextRow, nextCol];
                int endPosn = getEndOfPathOnTile(nextTile, startPos.returnLocationOnTile());
				Posn newPosn = new Posn(nextRow, nextCol, endPosn);
                return moveMockPlayer(newPosn);
            }
        
        
        }
        
        public bool locationOccupied(Posn inputPosn)
        {
            foreach (SPlayer p in onBoard)
            {
                Posn playerPosn = p.getPlayerPosn();
                
				if (playerPosn.isEqual(inputPosn))
				{
					return true;
				}
            }
            return false;
        }

		public bool isOnBoard(String color) {
			if (onBoard.Find(x => x.returnColor() == color) != null) {
				return true;
			} else {
				return false;
			}
		}

		public bool isEliminated(String color)
        {
            if (eliminated.Find(x => x.returnColor() == color) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		public int getNumActive() {
			return onBoard.Count;
		}

		public int getNumEliminated() {
			return eliminated.Count;
		}

		public Posn getPlayerPosn(string color) {
			foreach (SPlayer p in onBoard) {
				if (p.returnColor() == color) {
					return p.getPlayerPosn();
				}
			}

			foreach (SPlayer p in eliminated) {
				if (p.returnColor() == color)
                {
                    return p.getPlayerPosn();
                }
			}

			throw new Exception("This color is not on the board - neither active nor eliminated.");
		}

        // Adds player to appropriate list
        // Eliminated if player is on edge, onBoard otherwise
		public void addPlayerToBoard(SPlayer player) {
			if (onEdge(player.getPlayerPosn())) {
				eliminated.Add(player);
			} else {
				onBoard.Add(player);
			}
		}

		public SPlayer getFirstActivePlayer() {
			if (onBoard.Count == 0) {
				throw new Exception("No more active players on the board");
			}
			return onBoard[0];
		}

        // Moves all players to the end of their path
        // Returns list of players who end up on the edge
		public void movePlayers() {         
			List<SPlayer> onEdgePlayers = new List<SPlayer>();
			for (int i = 0; i < getNumActive(); i++)
            {
				SPlayer player = onBoard[i];

                Posn endPos = moveMockPlayer(player.getPlayerPosn());
                player.setPosn(endPos);

                if (onEdge(endPos))
                {
                    onEdgePlayers.Add(player);
                    eliminatePlayer(player);   
                    i--;
                }
            }

			if (getNumActive() == 0) {
				eliminatedButWinners = onEdgePlayers;
			}
		}

		public bool isGameOver()
		{
			return onBoardTiles.Count == 35 || getNumActive() == 1 || eliminatedButWinners != null;
		}
        
		public TurnResult GetTurnResult()
		{
			List<SPlayer> winners;

			if (eliminatedButWinners != null) // Case where all active players eliminated on last turn, all become winners
			{
				winners = eliminatedButWinners;
			}
			else if (onBoardTiles.Count == 35 || getNumActive() == 1)// All active players are winners
			{
				winners = onBoard;
			} else {
				winners = null;
			}
			return new TurnResult(drawPile, onBoard, eliminated, this, winners);
		}

		public bool isDrawPileEmpty() {
			return drawPile.Count == 0;
		}
       
		public void drawTilesWithDragonHolder() {
			if (dragonTileHolder == null) {
				throw new Exception("There is not a dragon tile holder.");
			}

            
            if (!isDrawPileEmpty())
            {
				int dragonHolderIndex = onBoard.FindIndex(x =>
				                                          x.returnColor() 
				                                          == dragonTileHolder.returnColor());
                int toDrawIndex = dragonHolderIndex;
				SPlayer nextPlayerToDraw = dragonTileHolder;
                do
                {
                    nextPlayerToDraw.addTileToHand(drawATile());
                    toDrawIndex++;
					nextPlayerToDraw = onBoard[(toDrawIndex)
					                           % getNumActive()];
                } while (drawPile.Count != 0 &&
				         nextPlayerToDraw.getHandSize() < 3);

				dragonTileHolder = nextPlayerToDraw;
            }
		}

		public void moveCurrentPlayerToEndOfPlayOrder() {
			if (getNumActive() == 0) {
				throw new Exception("No active players left - can't change player order.");
			}
			SPlayer currentPlayer = onBoard[0];
			onBoard.RemoveAt(0);
            onBoard.Add(currentPlayer);
		}

		public bool isDragonTileHolder(string color) {
			if (dragonTileHolder == null) {
				return false;
			}
			return dragonTileHolder.returnColor() == color;
		}

		public void assignHandToPlayer(string color, List<Tile> hand)
        {
			SPlayer currPlayer = onBoard.Find(x => x.returnColor() == color);
			if (currPlayer != null)
			{
				currPlayer.setHand(hand);
			}else{
				throw new Exception("Player is not an Active Player (assigning hand to player).");
			}
            
        }
        
		public List<Tile> getLegalMoves(List<Tile> hand, string color) {
			throw new NotImplementedException();
			//List<Tile> nonElimMoves = new List<Tile>();
			//List<Tile> allMoves = new List<Tile>();

			//SPlayer currPlayer = getActiveSPlayer(color);

     //       // Add all rotations of hand to validMoves
     //       foreach (Tile t in hand)
     //       {
     //           Tile checkTile = t;
     //           int timesRotated = 0;
     //           checkTile = checkTile.rotate();
                
     //           while (timesRotated < 4)
     //           {
     //               if (isNotEliminationMove(currPlayer, checkTile))
     //               {
					//	nonElimMoves.Add(checkTile);
     //               }
					//allMoves.Add(checkTile);
                  
        //            checkTile = checkTile.rotate();
        //            timesRotated = timesRotated + 1;
                    
                    
        //        }
        //    }
        //}


            ////no valid moves, return the first tile
            //if (validMoves.Count == 0)
            //{
            //    return allMovesOrdered[0];
            //}

            //return validMoves[0];
        }

	}


}
