using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tsuro
{
	[Serializable]
    public class RandomPlayer : AutomatedPlayer
    {
		public RandomPlayer(string name) 
        {
			this.name = name;
        }

		public RandomPlayer() {
			name = "Random";
		}

        public override Tile playTurn(Board b, List<Tile> playerHand, int numTilesInDrawPile)
        {
			if (playerHand.Count == 0)
            {
                throw new TsuroException("Random player cannot play turn: hand is empty.");
            }

            List<Tile> legalMoves = b.getLegalMoves(playerHand, color);

			Random r = new Random();
			int rInt = r.Next(0, legalMoves.Count);

			return legalMoves[rInt];
        }
    }
}
