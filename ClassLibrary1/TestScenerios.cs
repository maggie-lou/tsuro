using System;
using System.Collections.Generic;
using System.Text;

namespace tsuro
{
    public class TestScenerios
    {
        public Tile makeTile(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            Path first = new Path(a, b);
            Path second = new Path(c, d);
            Path third = new Path(e, f);
            Path fourth = new Path(g, h);

            List<Path> paths = new List<Path>()
            {
                first,
                second,
                third,
                fourth
            };
            Tile t1 = new Tile(paths);
            return t1;
        }

        public List<Tile> makeHand(params Tile[] list)
        {
            List<Tile> hand = new List<Tile>();
            foreach (Tile t in list)
            {
                hand.Add(t);
            }
            return hand;
        }

        public List<Tile> makeDrawPile(params Tile[] list)
        {
            List<Tile> drawPile = new List<Tile>();
            foreach (Tile t in list)
            {
                drawPile.Add(t);
            }
            return drawPile;

        }
		public void setStartPos00(Board board, SPlayer player)
        {
		//public void setStartPos00(Board board, SPlayer player, string color, List<string> playerOrder) {
			Posn startPos = new Posn(-1, 0, 5);
			player.setPosn(startPos);
			board.registerPlayer(player);
			player.playerState = SPlayer.State.Placed;
		}

		public void setStartPos(Board board, SPlayer player, Posn pos)
        {
            player.setPosn(pos);
            board.registerPlayer(player);
            player.playerState = SPlayer.State.Placed;
        }

		public void putTileOnBoard(Tile tile, Board board, int row, int col) {
			board.grid[row, col] = tile;
            board.onBoardTiles.Add(tile);
		}

		public SPlayer createPlayerAtPos(String name, List<Tile> hand, 
		                                 IPlayer iplayer, Posn posn, Board board) {
			SPlayer player = new SPlayer(name, hand, iplayer);
            player.setPosn(posn);
			player.playerState = SPlayer.State.Playing;
			board.registerPlayer(player);
			return player;
		}

		public Board createBoardWithDrawPile(List<Tile> drawPile){
			Board board = new Board();
			board.drawPile = drawPile;
			return board;
		}
    }
}
