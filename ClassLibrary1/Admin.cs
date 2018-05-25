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
		TurnResult playATurn(List<Tile> pile, List<SPlayer> inGamePlayers, List<SPlayer> eliminatedPlayers,
			Board b, Tile t);
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

		public void dealTiles(Board b)
		{
			foreach (SPlayer p in b.returnOnBoard())
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

		public TurnResult playATurn(List<Tile> pile, List<SPlayer> inGamePlayers,
									List<SPlayer> eliminatedPlayers, Board b, Tile t)
		{
			if (inGamePlayers.Count == 0)
			{
				// return TurnResult with the same drawpile, same list of Players in game,
				// same list of Players eliminated, same board, and null for list of players who are winners
				TurnResult trSame = new TurnResult(pile, inGamePlayers, eliminatedPlayers, b, null);
				return trSame;
			}
		
    		// Initialize state of board with inputs
    		b.drawPile = pile;
    		b.onBoard = inGamePlayers;
    		b.eliminated = eliminatedPlayers;

			// Place tile on board
			SPlayer currentPlayer = inGamePlayers[0];
			b.placeTile(currentPlayer, t);

			// Move active players if newly placed tile affects them
			List<SPlayer> onEdgePlayers = new List<SPlayer>();
			for (int i = 0; i < inGamePlayers.Count; i++) {
				SPlayer player = inGamePlayers[i];
			
				Posn endPos = b.movePlayer(player.getPlayerPosn());
				player.setPosn(endPos);

				if (b.onEdge(endPos))
                {
                    onEdgePlayers.Add(player);
					b.eliminatePlayer(player);
					i--;
                }
			}

			// Check if game is over
			TurnResult tr = null;
			if (b.onBoardTiles.Count == 35
			    || inGamePlayers.Count == 1) {
				tr = new TurnResult(pile, inGamePlayers, eliminatedPlayers, b, inGamePlayers);
			} else if (inGamePlayers.Count == 0) {
				tr = new TurnResult(pile, inGamePlayers, eliminatedPlayers, b, onEdgePlayers);
			}

			if (tr != null) {
				return tr;
			}
            

			// Dragon holder draws first, then all players until draw 
            // pile empties 
			bool isCurrentPlayerEliminated = !(inGamePlayers[0].returnColor()
                                              == currentPlayer.returnColor());
			if (b.returnDragonTileHolder() != null) {
				if (b.drawPile.Count != 0) {
					SPlayer dragonHolder = b.returnDragonTileHolder();
                    int dragonHolderIndex = inGamePlayers.FindIndex(x =>
                        x.returnColor() == dragonHolder.returnColor());
                    int toDrawIndex = dragonHolderIndex;
                    SPlayer nextPlayerToDraw = dragonHolder;
                    do
                    {
                        nextPlayerToDraw.addTileToHand(b.drawATile());
                        toDrawIndex++;
                        nextPlayerToDraw = inGamePlayers[(toDrawIndex)
                                                         % inGamePlayers.Count];
                    } while (b.drawPile.Count != 0 &&
                             nextPlayerToDraw.returnHand().Count < 3);

                    b.setDragonTileHolder(nextPlayerToDraw);
				}
			} else {
				if (!isCurrentPlayerEliminated) {
					if (b.drawPile.Count != 0)
                    {
                        currentPlayer.addTileToHand(b.drawATile());
                    }
                    else
                    {
                        b.setDragonTileHolder(currentPlayer);
                    }

				}
			}

            // Update game play order
			if (!isCurrentPlayerEliminated) {
				inGamePlayers.RemoveAt(0);
                inGamePlayers.Add(currentPlayer);
			}

			// Compute turn result
			tr = new TurnResult(pile, inGamePlayers, eliminatedPlayers, b, null);
			return tr;

		}
      
	}

}
