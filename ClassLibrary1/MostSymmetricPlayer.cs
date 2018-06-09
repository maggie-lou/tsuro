using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tsuro
{
	[Serializable]
    public class MostSymmetricPlayer : AutomatedPlayer
    {
		public MostSymmetricPlayer(string name)
		{
			this.name = name;
		}

		public MostSymmetricPlayer()
        {
            name = "MSP";
        }

		// Called to ask the player to make a move
		// Returns the tile the player should place, suitably rotated
		//
		// Selects the most symmetric legal move
		public override Tile playTurn(Board b, List<Tile> playerHand, int numTilesInDrawPile)
		{
			if (playerHand.Count == 0)
			{
				throw new TsuroException("MSP cannot play turn: hand is empty.");
			}

			List<Tile> legalMoves = b.getLegalMoves(playerHand, color);

            // Sorts moves in descending order of symmetricity 
			Tile.sortBySymmetricity(legalMoves);

			return legalMoves[0];
		}
    }
}
