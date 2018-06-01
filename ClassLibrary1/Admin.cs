using System;
using System.Collections.Generic;
using System.IO;



namespace tsuro
{
	interface IAdmin
	{
		// takes in a SPlayer, a Board, and a Tile t that will be played
		// returns true if the tile that is going to be played is in player's hand
		// AND if placing the tile is not a move that will eliminate the player
		bool legalPlay(SPlayer p, Board b, Tile t);

		// takes in a List of Tiles of the drawpile, a list of SPlayers for the inGamePlayers
		// a list of SPlayers that are eliminated, a Board, and a Tile t that will be placed during that turn
		// for the first SPlayer in inGamePlayers
		TurnResult playATurn(Board b, Tile t);
	}
	public class Admin : IAdmin
	{
		public List<Tile> initializeDrawPile(string filename)
		{
			List<Tile> drawPile = new List<Tile>();
			StreamReader reader = File.OpenText(filename);
			string tile;
			while ((tile = reader.ReadLine()) != null)
			{
				List<Path> tilePaths = new List<Path>();
				string[] paths = tile.Split(',');
				foreach (string path in paths)
				{
					string[] locs = path.Split(' ');
					int i = 0;
					int[] loc_arr = new int[2];
					foreach (string loc in locs)
					{
						loc_arr[i] = int.Parse(loc);
						i++;
					}
					Path newPath = new Path(loc_arr[0], loc_arr[1]);
					tilePaths.Add(newPath);
				}
				Tile newTile = new Tile(tilePaths);
				drawPile.Add(newTile);
			}
			return drawPile;
		}

		public void dealTiles(List<SPlayer> activePlayers, Board b)
		{
			foreach (SPlayer p in activePlayers)
			{
				for (int i = 0; i < 3; i++)
				{
					Tile t = b.drawATile();
					p.addTileToHand(t);
				}
			}
		}

		public bool legalPlay(SPlayer p, Board b, Tile t)
		{
			//return (tileInHand(p, t) && b.isNotEliminationMove(p,t));
			if ((b.isNotEliminationMove(p, t)) && (p.tileInHand(t)))
			{
				return true;
			}
			else if (!b.isNotEliminationMove(p, t) && (p.allMovesEliminatePlayer(b, t)) && (p.tileInHand(t)))
			{
				return true;
			}
			return false;
		}

		//public bool legalPlay(SPlayer p, Board b, Tile t)
		//{
		//    return p.tileInHand(t)
		//            && (b.isNotEliminationMove(p, t)
		//                || (!b.isNotEliminationMove(p, t)
		//                    && (p.allMovesEliminatePlayer(b, t))));
		//}

		public TurnResult playATurn(Board b, Tile t)
		{
			//if (b.getNumActive() == 0)
			//{
			//	// return TurnResult with the same drawpile, same list of Players in game,
			//	// same list of Players eliminated, same board, and null for list of players who are winners
			//	TurnResult trSame = new TurnResult(pile, inGamePlayers, eliminatedPlayers, b, null);
			//	return trSame;
			//}

			// Place tile on board
			SPlayer currentPlayer = b.getFirstActivePlayer();
			b.placeTile(currentPlayer, t);

			// Move active players if newly placed tile affects them
			b.movePlayers();
			bool isCurrentPlayerEliminated = b.isEliminated(currentPlayer.returnColor());

			// Check if game is over
			if (b.isGameOver()) {
				return b.GetTurnResult();
			}
            
            // Players draw
			if (b.existsDragonTileHolder()) {
				b.drawTilesWithDragonHolder();
			} else {
				if (!isCurrentPlayerEliminated) {
					if (b.isDrawPileEmpty()) {
						b.setDragonTileHolder(currentPlayer);
					} else {
						currentPlayer.addTileToHand(b.drawATile());
					}
				}
			}
         
            // Update game play order
			if (!isCurrentPlayerEliminated) {
				b.moveCurrentPlayerToEndOfPlayOrder();
			}

			// Compute turn result
			return b.GetTurnResult();

		}
      
	}

}
