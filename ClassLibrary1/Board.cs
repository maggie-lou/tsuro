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
        int[] nextTilePosn(SPlayer p);
        // Takes in a SPlayer and a Tile and returns whether that player can place the tile
        // (this means that it does not lead a player to an edge if they have other tiles
        // and that when you first start, you are on the tile that you will place next)
        bool checkPlaceTile(SPlayer p, Tile t);
        // returns whether SPlayer is on an edge
        bool onEdge(SPlayer p);
        // takes in a Tile, the current location of a player on its current tile
        // and a bool indicating if the player is starting on the edge
        // returns an int of new location on Tile t
        int getEndOfPathOnTile(Tile t, int currTilePosn, bool startOnEdge);
        // returns a new SPlayer that has moved to the end of the path on Tile t
        SPlayer placeTile(SPlayer p, Tile t);
        // returns whether a grid location already has a tile
        bool occupied(int row, int col);
        // takes in a SPlayer and returns an SPlayer that has been moved to a new location on a new tile
        // a recursively called function to move players through multiple tiles
        SPlayer movePlayer(SPlayer p);
        
    }

    public class Board:IBoard
    {
        public Tile[,] grid = new Tile[6, 6]; // grid of tiles placed on the board
        List<SPlayer> onBoard = new List<SPlayer>(); // list of players on the board
        List<SPlayer> eliminated = new List<SPlayer>(); // list of eliminated players
        SPlayer dragonTileHolder = null; //set to the player which is holding the dragon tile 
        public List<Tile> drawPile = new List<Tile>();

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

        public int[] nextTilePosn(SPlayer p)
        {
            int currentRow = p.getboardLocationRow();
            int currentCol = p.getboardLocationCol();
            int currentTilePosn = p.getLocationOnTile();

            int newRow = 0;
            int newCol = 0;
            //int newTilePosn = 0;

            if (currentTilePosn == 0 || currentTilePosn == 1)
            {
                newRow = currentRow - 1;
                newCol = currentCol;
            }
            else if (currentTilePosn == 2 || currentTilePosn == 3)
            {
                newCol = currentCol + 1;
                newRow = currentRow;
            }
            else if (currentTilePosn == 5 || currentTilePosn == 4)
            {
                newRow = currentRow + 1;
                newCol = currentCol;
            }
            else if (currentTilePosn == 7 || currentTilePosn == 6)
            {
                newCol = currentCol - 1;
                newRow = currentRow;
            }

            int[] newposn = new int[2];

            newposn[0] = newRow;
            newposn[1] = newCol;
            return newposn;
        }

        public bool checkPlaceTile(SPlayer p, Tile t)
        {
            // make a copy of player p in temp
            SPlayer temp = new SPlayer(p.returnColor(), p.returnHand(), p.hasMoved);
            temp.setPosn(p.getboardLocationRow(), p.getboardLocationCol(), p.getLocationOnTile());

            int currentTilePosn = temp.getLocationOnTile();
            int newTilePosn = 0;
            int[] newGridLoc = new int[2];

            // if player is on the edge and it is their first turn
            if (onEdge(temp) && !temp.hasMoved)
            {
                temp.hasMoved = true;
                newGridLoc[0] = temp.getboardLocationRow();
                newGridLoc[1] = temp.getboardLocationCol();

                newTilePosn = getEndOfPathOnTile(t, currentTilePosn, true);
               
            }
            else // if player is not on the edge, it is not their first turn
                // or it is on the edge but it is not their first turn (should be eliminated)
            {
                newGridLoc = nextTilePosn(p);
                newTilePosn = getEndOfPathOnTile(t, currentTilePosn, false);
            }

            temp.setPosn(newGridLoc[0], newGridLoc[1], newTilePosn);

            if (onEdge(temp))
            {
                return false;
            }

            return true;
        }

        public bool onEdge(SPlayer p)
        {
            int newRow = p.getboardLocationRow();
            int newCol = p.getboardLocationCol();
            int newTilePosn = p.getLocationOnTile();
            //check if the move will lead the player to the edge
            if (newRow == 0)
            {
                if (newTilePosn == 0 || newTilePosn == 1)
                {
                    return true;
                }
            }
            if (newCol == 0)
            {
                if (newTilePosn == 6 || newTilePosn == 7)
                {
                    return true;
                }
            }
            if (newCol == 5)
            {
                if (newTilePosn == 2 || newTilePosn == 3)
                {
                    return true;
                }
            }
            if (newRow == 5)
            {
                if (newTilePosn == 4 || newTilePosn == 5)
                {
                    return true;
                }
            }
            return false;
        }

        public int getEndOfPathOnTile(Tile t, int currTilePosn, bool startOnEdge)
        {
            int newTilePosn = 0;

            if (startOnEdge) // if it is a players first turn, make new posn be based on current grid location
            {
                if (currTilePosn == 0)
                {
                    newTilePosn = t.getLocationEnd(0);
                }
                else if (currTilePosn == 5)
                {
                    newTilePosn = t.getLocationEnd(5);
                }
                else if (currTilePosn == 1)
                {
                    newTilePosn = t.getLocationEnd(1);
                }
                else if (currTilePosn == 4)
                {
                    newTilePosn = t.getLocationEnd(4);
                }
                else if (currTilePosn == 2)
                {
                    newTilePosn = t.getLocationEnd(2);
                }
                else if (currTilePosn == 7)
                {
                    newTilePosn = t.getLocationEnd(7);
                }
                else if (currTilePosn == 3)
                {
                    newTilePosn = t.getLocationEnd(3);
                }
                else if (currTilePosn == 6)
                {
                    newTilePosn = t.getLocationEnd(6);
                }
            }
            else //find player location on new tile
            {
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
            }
            
            return newTilePosn;
        }

        public SPlayer placeTile(SPlayer p, Tile t)
        {
            int[] newGridLoc = new int[2];
            bool startOnEdge;

            // if player is on the edge and it is their first turn
            if (onEdge(p) && !p.hasMoved)
            {
                // set hasMoved (first turn) to be true
                p.hasMoved = true;
                // set new grid location for player to be the same initial grid location
                newGridLoc[0] = p.getboardLocationRow();
                newGridLoc[1] = p.getboardLocationCol();
                // set startOnEdge to be true
                startOnEdge = true;
            }
            else // if player is not on the edge, if it is not the players first turn anymore
            {
                // set new grid location to be the next location that player can place tile in
                newGridLoc = nextTilePosn(p);
                startOnEdge = false;
            }

            // get the current player location on their current tile
            int currentTilePosn = p.getLocationOnTile();

            int newRow = newGridLoc[0];
            int newCol = newGridLoc[1];
            // get the new player location on the next tile
            int newTilePosn = getEndOfPathOnTile(t, currentTilePosn, startOnEdge);

            // set the next grid location on the board to be the tile
            grid[newRow, newCol] = t;

            // set the player position to be the next grid location and new player location on new tile
            p.setPosn(newRow, newCol, newTilePosn);
            // call movePlayer to move the player if there are additional tiles to move
            p = movePlayer(p);

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

        public SPlayer movePlayer(SPlayer p)
        {
            // copy SPlayer p to temp to manipulate
            SPlayer temp = new SPlayer(p.returnColor(), p.returnHand(), p.hasMoved);
            temp.setPosn(p.getboardLocationRow(), p.getboardLocationCol(), p.getLocationOnTile());

            // initialize current location of player on current tile
            int currLoc;

            // if player is on the edge but has not moved yet (first turn)
            if (onEdge(temp) && !temp.hasMoved)
            {
                // if there is no tile where the player currently is, return the same player
                if (!occupied(temp.getboardLocationRow(), temp.getboardLocationCol()))
                {
                    return temp;
                }
                else // if there is a tile placed where player currently is
                {
                    // set the current grid location of the player ot be the same as where it starts
                    // set current location of player to be at the end of the path on the tile
                    currLoc = getEndOfPathOnTile(grid[temp.getboardLocationRow(), temp.getboardLocationCol()], temp.getLocationOnTile(), true);
                    temp.setPosn(temp.getboardLocationRow(), temp.getboardLocationCol(), currLoc);
                    // set hasMoved (first turn) to be true
                    temp.hasMoved = true;
                    // call movePlayer recursively to see if there are any more tiles to move
                    return movePlayer(temp);
                }
            }
            // else if player is on the edge but it is no longer their first turn
            else if (onEdge(temp) && temp.hasMoved)
            {
                // return the player
                return temp;
            }

            // if player is not on the edge
            // get next grid location that the player can move to
            int[] nextGridLoc = nextTilePosn(p);
            int row = nextGridLoc[0];
            int col = nextGridLoc[1];

            // if there is no tile in next grid location, return the player
            if (!occupied(row, col))
            {
                return temp;
            }
            else // if there is a tile in the next grid location
            {
                // set the current location of the player to be the the next grid location
                // with startOnEdge set to false (no longer first turn)
                currLoc = getEndOfPathOnTile(grid[row, col], temp.getLocationOnTile(), false);
                temp.setPosn(row, col, currLoc);
                // recursively call movePlayer to see if there are additional tiles to move
                return movePlayer(temp);
            }
        }

        public bool locationOccupied(int row, int col, int loc)
        {
            foreach (SPlayer p in onBoard)
            {
                if (p.getboardLocationRow() == row)
                {
                    if (p.getboardLocationCol() == col)
                    {
                        if (p.getLocationOnTile() == loc)
                        {
                            return true;
                        }
                    } 
                }
            }
            return false;
        }
    }
}
