using System;
using System.Collections.Generic;


namespace tsuro
{
    interface IBoard
    {
        //returns true if the move was valid and the tile was placed
        //returns false if there was a tile there already 
        bool placeTile(int row, int col, Tile t);
    }

    public class Board:IBoard
    {
        Tile[,] grid = new Tile[6, 6];
        SPlayer onBoard;
        public bool placeTile(int row, int col, Tile t)
        {
            throw new NotImplementedException();

        }
    }
}
