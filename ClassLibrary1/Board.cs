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

        public bool checkPlaceTile(SPlayer p, Tile t)
        {
            SPlayer temp = p;

            int currentRow = temp.getboardLocationRow();
            int currentCol = temp.getboardLocationCol();
            int currentTilePosn = temp.getLocationOnTile();

            int newRow = 0;
            int newCol = 0;
            int newTilePosn = 0;

            if(currentTilePosn == 0 || currentTilePosn == 1)
            {
                newRow = currentRow - 1;
                newCol = currentCol;
            }
            else if (currentTilePosn == 2 || currentTilePosn == 3)
            {
                newCol= currentCol + 1;
                newRow = currentRow;
            }
            else if (currentTilePosn == 5 || currentTilePosn == 4)
            {
                newRow = currentRow + 1;
                newCol = currentCol;
            }
            else if(currentTilePosn == 7|| currentTilePosn == 6)
            {
                newCol = currentCol - 1;
                newRow = currentRow;
            }

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

            //check if the move will lead the player to the edge
            if (newRow == 0)
            {
                if (newTilePosn == 0 || newTilePosn == 1)
                {
                    return false;
                }
            }
            if (newCol == 0)
            {
                if (newTilePosn == 6 || newTilePosn == 7)
                {
                    return false;
                }
            }
            if (newCol == 5)
            {
                if (newTilePosn == 2 || newTilePosn == 3)
                {
                    return false;
                }
            }
            if (newRow == 5)
            {
                if (newTilePosn == 4 || newTilePosn == 5)
                {
                    return false;
                }
            }

            //check if player will collide
            //meaning there is a player in the location we are trying to move to 

            return true;
        }

        public SPlayer placeTile(SPlayer p, Tile t)
        {
            int currentRow = p.getboardLocationRow();
            int currentCol = p.getboardLocationCol();
            int currentTilePosn = p.getLocationOnTile();

            int newRow = 0;
            int newCol = 0;
            int newTilePosn = 0;

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

            //find player location on new tile
            if (currentTilePosn == 0)
            {
                //find the end of new location path
                //set temp player location to this location 
                newTilePosn = t.getLocationEnd(5);
            }
            else if (currentTilePosn == 5)
            {
                newTilePosn = t.getLocationEnd(0);
            }
            else if (currentTilePosn == 1)
            {
                newTilePosn = t.getLocationEnd(4);
            }
            else if (currentTilePosn == 4)
            {
                newTilePosn = t.getLocationEnd(1);
            }
            else if (currentTilePosn == 2)
            {
                newTilePosn = t.getLocationEnd(7);
            }
            else if (currentTilePosn == 7)
            {
                newTilePosn = t.getLocationEnd(2);
            }
            else if (currentTilePosn == 3)
            {
                newTilePosn = t.getLocationEnd(6);
            }
            else if (currentTilePosn == 6)
            {
                newTilePosn = t.getLocationEnd(3);
            }

            grid[newRow, newCol] = t;

            p.setPosn(newRow, newCol, newTilePosn);

            return p;
        }
    }
}
