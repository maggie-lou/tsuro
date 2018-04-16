using System;
using System.Collections.Generic;
using System.Text;

namespace tsuro
{
    public class TurnResult
    {
        //the list of tiles in the draw pile not in players hands
        List<Tile> drawPile;
        //list of players stil in the game
        List<SPlayer> currentPlayers;
        //list of the players that have been eliminated
        List<SPlayer> eliminatedPlayers;
        //The board
        Board b;
        //null if the game is not over
        //returns the list of players if the game is over
        List<SPlayer> playResult;
        
        public TurnResult(List<Tile> d, List<SPlayer> inGame, List<SPlayer> elim, Board bd, List<SPlayer> winners)
        {
            drawPile = d;
            currentPlayers = inGame;
            eliminatedPlayers = elim;
            b = bd;
            playResult = winners;
        }
    }

}
