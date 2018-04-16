using System;
using System.Collections.Generic;


namespace tsuro
{
    interface IBoard
    {
        //returns true if the move was valid and the tile was placed
        //returns false if there was a tile there already 
        bool placeTile(int row, int col, Tile t);
        //registers players in the beginning of the game
        void registerPlayer(SPlayer p);
        void eliminatePlayer(SPlayer p);
    }

    public class Board:IBoard
    {
        //details of tiles on the board
        Tile[,] grid = new Tile[6, 6];

        List<SPlayer> onBoard;
        List<SPlayer> eliminated;
        //board must track the details of players and tiles on a board

        public bool placeTile(int row, int col, Tile t)
        {
            throw new NotImplementedException();

        }
    }
}
