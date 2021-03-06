﻿using System;
using System.Collections.Generic;
using System.Text;

namespace tsuro
{
	[Serializable]
    public class TurnResult
    {
        //the list of tiles in the draw pile not in players hands
        public List<Tile> drawPile;
        //list of players stil in the game
        public List<SPlayer> currentPlayers;
        //list of the players that have been eliminated
        public List<SPlayer> eliminatedPlayers;
        //The board
        public Board b;
        //null if the game is not over
        //returns the list of players if the game is over
        public List<SPlayer> playResult;
        
        public TurnResult(List<Tile> drawPile, List<SPlayer> inGame, List<SPlayer> elim, Board b, List<SPlayer> winners)
        {
			this.drawPile = drawPile;
            currentPlayers = inGame;
            eliminatedPlayers = elim;
            this.b = b;
            playResult = winners;
        }
    }

}
