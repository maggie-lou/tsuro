using System;
using System.Collections.Generic;


namespace tsuro
{
    interface IBoard
    {
        //returns true if the move was valid and the tile was placed
        //returns false if there was a tile there already 
        SPlayer placeTile(SPlayer p, Tile t);
        //registers players in the beginning of the game
        void registerPlayer(SPlayer p);
        void eliminatePlayer(SPlayer p);
        bool onEdge(SPlayer p);
        SPlayer movePlayer(SPlayer p);
        
    }

    public class Board:IBoard
    {
        //details of tiles on the board
        Tile[,] grid = new Tile[6, 6];
        
        List<SPlayer> onBoard = new List<SPlayer>();
        List<SPlayer> eliminated = new List<SPlayer>();
        //board must track the details of players and tiles on a board
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

        public bool checkEliminated(SPlayer p)
        {
            return true;
        }

        // Calculates the new grid location of where tile can be placed based on
        // the players current position
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
            SPlayer temp = new SPlayer(p.returnColor(), p.returnHand());
            temp.setPosn(p.getboardLocationRow(), p.getboardLocationCol(), p.getLocationOnTile());

            int currentTilePosn = temp.getLocationOnTile();
            int newTilePosn = 0;

            int[] newGridLoc = nextTilePosn(p);
            
            //find player location on new tile
            if (currentTilePosn == 0)
            {
                //find the end of new location path
                //set temp player location to this location 
                newTilePosn = t.getLocationEnd(5); 
            }
            else if(currentTilePosn == 5)
            {
                newTilePosn = t.getLocationEnd(0);
            }
            else if(currentTilePosn == 1)
            {
                newTilePosn = t.getLocationEnd(4);
            }
            else if(currentTilePosn == 4)
            {
                newTilePosn = t.getLocationEnd(1);
            }
            else if(currentTilePosn == 2)
            {
                newTilePosn = t.getLocationEnd(7);
            }
            else if(currentTilePosn == 7)
            {
                newTilePosn = t.getLocationEnd(2);
            }
            else if(currentTilePosn == 3)
            {
                newTilePosn = t.getLocationEnd(6);
            }
            else if(currentTilePosn == 6)
            {
                newTilePosn = t.getLocationEnd(3);
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

        // gets new location on tile
        public int getEndOfPathOnTile(Tile t, int currTilePosn)
        {
            int newTilePosn = 0;
            //find player location on new tile
            if (currTilePosn == 0)
            {
                //find the end of new location path
                //set temp player location to this location 
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
        // returns a new SPlayer that has moved to the end of the path on Tile t
        public SPlayer placeTile(SPlayer p, Tile t)
        {
            int[] newGridLoc = nextTilePosn(p);

            int currentTilePosn = p.getLocationOnTile();

            int newRow = newGridLoc[0];
            int newCol = newGridLoc[1];
            int newTilePosn = getEndOfPathOnTile(t, currentTilePosn);

            grid[newRow, newCol] = t;

            p.setPosn(newRow, newCol, newTilePosn);

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
            SPlayer temp = new SPlayer(p.returnColor(), p.returnHand());
            temp.setPosn(p.getboardLocationRow(), p.getboardLocationCol(), p.getLocationOnTile());
            int currLoc;


            if (onEdge(p) && !p.hasMoved)
            {
                currLoc = getEndOfPathOnTile(grid[temp.getboardLocationRow(), temp.getboardLocationCol()], temp.getLocationOnTile());
                temp.setPosn(temp.getboardLocationRow(), temp.getboardLocationCol(), currLoc);
                p.hasMoved = true;
                return movePlayer(temp);
            }
            
            int[] nextGridLoc = nextTilePosn(p);
            int row = nextGridLoc[0];
            int col = nextGridLoc[1];


            if (!occupied(row, col))
            {
                return temp;
            }
            else if (onEdge(temp))
            {
                return temp;
            }

            int currLoc = getEndOfPathOnTile(grid[row, col], temp.getLocationOnTile());
            temp.setPosn(row, col, currLoc);
            return movePlayer(temp);
        }
    }
}
