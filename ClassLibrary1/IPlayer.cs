using System;
using System.Collections.Generic;
using System.Text;

namespace tsuro
{
    public interface IPlayer
    {
        string getName();
        void initialize(string playerColor, List<string> allColors);
        Posn placePawn(Board b);
        Tile playTurn(Board b, List<Tile> playerHand, int numTilesInDrawPile);
        void endGame(Board b, List<string> allColors);
    }
}
