using System;
using System.Collections.Generic;


namespace tsuro
{
    interface IBoard
    {
        // returns list of SPlayers that are eliminated
        List<SPlayer> returnEliminated();
        //returns list of SPlayers on the board
        List<SPlayer> returnOnBoard();
        // eliminate a player by removing from inGamePlayers and adding to eliminatedPlayers
        void eliminatePlayer(SPlayer p);
        //registers players in the beginning of the game
        void registerPlayer(SPlayer p);
        // returns an array of [row, col] of next grid location the player can place a tile
        int[] nextTileCoord(Posn p);
        // Takes in a SPlayer and a Tile and returns whether that player can place the tile
        // (this means that it does not lead a player to an edge if they have other tiles
        // and that when you first start, you are on the tile that you will place next)
        bool isEliminationMove(SPlayer p, Tile t);
        // returns whether SPlayer is on an edge
        bool onEdge(Posn p);
        // takes in a Tile, the current location of a player on its current tile
        // and a bool indicating if the player is starting on the edge
        // returns an int of new location on Tile t
        int getEndOfPathOnTile(Tile t, int currTilePosn);
        // returns a new SPlayer that has moved to the end of the path on Tile t
        SPlayer placeTile(SPlayer p, Tile t);
        // returns whether a grid location already has a tile
        bool occupied(int row, int col);
        // takes in a SPlayer and returns an SPlayer that has been moved to a new location on a new tile
        // a recursively called function to move players through multiple tiles
        Posn movePlayer(Posn p);
        
    }

    public class Board:IBoard
    {
        public Tile[,] grid = new Tile[6, 6]; // grid of tiles placed on the board
        List<SPlayer> onBoard = new List<SPlayer>(); // list of players on the board
        List<SPlayer> eliminated = new List<SPlayer>(); // list of eliminated players
        SPlayer dragonTileHolder = null; //set to the player which is holding the dragon tile 
        public List<Tile> drawPile = new List<Tile>();
        public List<Tile> onBoardTiles = new List<Tile>();

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

        public List<SPlayer> returnOnBoard()
        {
            return onBoard;
        }

        public void eliminatePlayer(SPlayer p)
        {
            if ((p.playerState != SPlayer.State.Placed) && (p.playerState != SPlayer.State.Playing))
            {
                throw new Exception("Player is being eliminated before having placed a pawn or played a turn.");
            }
            if(p.returnHand().Count != 0)
            {
                foreach (Tile t in p.returnHand())//adding eliminated players tiles to draw pile
                {
                    addTileToDrawPile(t);
                }
            }
            if(dragonTileHolder != null)
            {
                if (dragonTileHolder.returnColor() == p.returnColor())
                {
                    dragonTileHolder = null;
                }
            }
            
            eliminated.Add(p);
            SPlayer temp = onBoard.Find(x => x.returnColor() == p.returnColor());
            onBoard.Remove(temp);
            p.playerState = SPlayer.State.Eliminated;
        }

        public void registerPlayer(SPlayer p)
        {
            if (p != null)
            {
                onBoard.Add(p);
            }
        }

        public SPlayer returnDragonTileHolder()
        {
            return dragonTileHolder;
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
					nextCoord[1] = currentCol - 1;
					break;
				case 2: case 3:
					nextCoord[0] = currentRow + 1;
					break;
				case 4: case 5:
					nextCoord[1] = currentCol + 1;
					break;
				case 6: case 7:
					nextCoord[0] = currentRow - 1;
					break;	
			}

			return nextCoord;
        }

        public bool isEliminationMove(SPlayer p, Tile t)
        {
           
            // make a copy of player p in temp
            SPlayer temp = new SPlayer(p.returnColor(), p.returnHand(), p.hasMoved);
            Posn playerPosn = p.getPlayerPosn();
            temp.setPosn(playerPosn);

            int currentTilePosn = playerPosn.returnLocationOnTile();
            Posn playerAtUpdatedTilePosn = new Posn();
           
            int[] newGridLoc = nextTileCoord(playerPosn);
            int newTilePosn = getEndOfPathOnTile(t, currentTilePosn);
            
            playerAtUpdatedTilePosn.setPlayerPosn(newGridLoc[0],newGridLoc[1], newTilePosn);
            temp.setPosn(playerAtUpdatedTilePosn);
			Posn endPos = movePlayer(playerAtUpdatedTilePosn);

			if (onEdge(endPos))
            {
                return false;
            }

            return true;
        }
        
        public bool onEdge(Posn p)
        {
			int row = p.returnRow();
			int col = p.returnCol();
			int tilePos = p.returnLocationOnTile();

			if (row == 0 || row == 5)
            {
				if (tilePos == row || tilePos == row + 1)
                {
                    return true;
                }
            }
			if (col == 0)
            {
				if (tilePos == 6 || tilePos == 7)
                {
                    return true;
                }
            }
			if (col == 5)
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

        public SPlayer placeTile(SPlayer p, Tile t)
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
            // set the player position to be the next grid location and new player location on new tile
            Posn newPlayerPosn = new Posn(newRow, newCol, newTilePosn);
            // call movePlayer to move the player if there are additional tiles to move
			Posn endPos = movePlayer(newPlayerPosn);
			p.setPosn(endPos);

            return p;
        }

        public bool occupied(int row, int col)
        {
            if (grid[row,col] != null)
            {
                return true;
            }
            return false;
        }
        
        public Posn movePlayer(Posn startPos)
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
                return movePlayer(newPosn);
            }
        
        
        }
        
		private bool isValidCoord(int[] coord) {
			return coord[0] >= 0 
				&& coord[0] < grid.Length 
				                  && coord[1] >= 0 
				                  && coord[1] < grid.Length;
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
    }
}
