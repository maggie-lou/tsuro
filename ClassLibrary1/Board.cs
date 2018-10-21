using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

namespace tsuro
{   
	[Serializable]
	public class Board 
	{
		private Tile[,] grid;
		private Dictionary<string, Posn> colorToPosnMap;



        /*************** CONSTRUCTORS ****************************/

		public Board(){
			grid = new Tile[6, 6];
			colorToPosnMap = new Dictionary<string, Posn>();
		}
        
		public Board(Tile[,] grid, Dictionary<string, Posn> colorToPosnMap){
			this.grid = grid;
			this.colorToPosnMap = colorToPosnMap;
		}

        
        
		/*************** GETTERS ****************************/
		public int getNumTilesOnBoard()
		{
			int count = 0;
			for (int i = 0; i < grid.GetLength(0); i++)
			{
				for (int j = 0; j < grid.GetLength(1); j++)
				{
                    if (grid[i,j] != null)
					{
						count++;
					}
				}
			}
			return count;
		}
      
		public Tile getTileAt(int row, int col) {
			return grid[row, col];
		}

		public int getBoardLength() {
			return grid.GetLength(0);
		}

		public List<string> getAllPlayerColors() {
			return colorToPosnMap.Keys.ToList();
		}


        public Posn getPlayerPosn(string color)
        {
            if (!colorToPosnMap.ContainsKey(color))
            {
                throw new TsuroException("Cannot get player position - there are no players with this color on the board.");
            }
            return colorToPosnMap[color];
        }

		//Maps player onto next tile's duplicate tile location and finds end of the path on the new tile
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


		// Gets all valid moves for the player with the input color, and the input hand
        //
        // A valid move is a non-elimination move 
        // If all moves are elimination moves, every rotation of every tile is a valid move
        //
        // Each different rotation of the same tile is added to the result separately
        public List<Tile> getLegalMoves(List<Tile> hand, string color)
        {
            List<Tile> nonElimMoves = new List<Tile>();
            List<Tile> allMoves = new List<Tile>();

            // Add all rotations of hand to validMoves
            foreach (Tile t in hand)
            {
                List<Tile> diffRotations = t.getDifferentRotations();
                allMoves.AddRange(diffRotations);

                foreach (Tile rot in diffRotations)
                {
                    if (!isEliminationMove(color, rot))
                    {
                        nonElimMoves.Add(rot);
                    }
                }
            }

            if (nonElimMoves.Count != 0)
            {
                return nonElimMoves;
            }
            else
            {
                return allMoves;
            }
        }

		/*************** SETTERS ****************************/

		public void addPlayerToBoard(string color, Posn posn)
        {
            colorToPosnMap.Add(color, posn);
        }

		/*************** PREDICATES ****************************/
        public bool tileExistsOnBoard(Tile t)
		{
			for (int i = 0; i < grid.GetLength(0); i++)
            {
				for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null && grid[i,j].isEqualOrRotation(t))
                    {
						return true;
                    }
                }
            }
            return false;
		}


		public bool isEliminationMove(string color, Tile t)
        {
			// Get next tile position
			Posn playerPosn = colorToPosnMap[color];
            int[] newGridLoc = nextTileCoord(playerPosn);

            // Put tile on mock board
            Board mockBoard = this.clone();
            mockBoard.grid[newGridLoc[0], newGridLoc[1]] = t;

            // Move player on fake board
            Posn endPos = mockBoard.followPathsMock(playerPosn);

            // See if elimination move
            return isElimPosn(endPos);
        }
        
        // Returns true if a position is on the edge of the board, due to elimination
		public bool isElimPosn(Posn p)
        {
            int row = p.returnRow();
            int col = p.returnCol();
            int tilePos = p.returnLocationOnTile();

            // Phantom tile positions are valid start positions on the board edge, and don't cause elimination
			bool phantomTile = col == -1 || col == 6 || row == -1 || row == 6;
			if (phantomTile) {
				return false;
			}
            
			bool topEdgeElim = row == 0 && (tilePos == 0 || tilePos == 1);
			bool bottomEdgeElim = row == 5 && (tilePos == 4 || tilePos == 5);
			bool leftEdgeElim = col == 0 && (tilePos == 6 || tilePos == 7);
			bool rightEdgeElim = col == 5 && (tilePos == 2 || tilePos == 3);

			return topEdgeElim || bottomEdgeElim || leftEdgeElim || rightEdgeElim;
        }

        // Returns true if there is already a player at the input position
		public bool locationOccupied(Posn inputPosn)
        {
            foreach (Posn p in colorToPosnMap.Values.ToList())
            {
                if (p.isEqual(inputPosn))
                {
                    return true;
                }
            }
            return false;
        }


		/*************** GAME PLAY FUNCTIONS ****************************/

		// The player with the input color places tile t at the grid position they
        // are about to move to
        //
        // Returns the end position of the given player given the tile placement, 
        // but does not actually move him/her
        public Posn placeTile(String color, Tile t)
        {
            int[] newGridLoc = new int[2];
            Posn playerPosn = colorToPosnMap[color];

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

            // Calculate end position of player on new tile
            Posn endPos = new Posn(newRow, newCol, newTilePosn);
            // Calculate end position of player if additional tiles to move across
            endPos = followPathsMock(endPos);
            return endPos;
        }

        public void placeTileAt(Tile tile, int row, int col)
        {
            grid[row, col] = tile;
        }


        // Returns end position for the given start position, after following all
        // on board paths
        //
        // Does not actually move player on board
        public Posn followPathsMock(Posn startPos)
        {
            if (isElimPosn(startPos))
            {
                return startPos;
            }

            int[] nextCoord = nextTileCoord(startPos);
            int nextRow = nextCoord[0];
            int nextCol = nextCoord[1];

            // End recursion if no more tiles along path
            if (grid[nextRow, nextCol] == null)
            {
                return startPos;
            }
            else // Recursively follow path
            {
                // set the current location of the player to be the the next grid location
                Tile nextTile = grid[nextRow, nextCol];
                int endPosn = getEndOfPathOnTile(nextTile, startPos.returnLocationOnTile());
                Posn newPosn = new Posn(nextRow, nextCol, endPosn);
                return followPathsMock(newPosn);
            }
        }

		// Moves all active players to the end of their path
        // Returns list of colors of players who end up on the edge
        //
        // Actually moves players' positions on board
        public List<string> moveActivePlayers(List<string> activePlayerColors)
        {
            List<string> onEdgePlayerColors = new List<string>();

            for (int i = 0; i < activePlayerColors.Count; i++)
            {
                String color = activePlayerColors[i];
                Posn startPosn = colorToPosnMap[color];
                Posn endPos = followPathsMock(startPosn);

                // Update player position map
                colorToPosnMap[color] = endPos;
                if (isElimPosn(endPos))
                {
                    onEdgePlayerColors.Add(color);
                }
            }

            return onEdgePlayerColors;
        }

		/*************** HELPER FUNCTIONS ****************************/
        
        // Returns the [row, col] of the next grid position a player at position p
        // will move to
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
	}


}
