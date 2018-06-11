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
			Posn startPos = new Posn(-1, 0, 5);
			board.addPlayerToBoard(player.getColor(), startPos);
			player.playerState = SPlayer.State.Placed;
		}

		public void setStartPos(Board board, SPlayer player, Posn pos)
        {
			board.addPlayerToBoard(player.getColor(), pos);
            player.playerState = SPlayer.State.Placed;
        }

		public void putTileOnBoard(Tile tile, Board board, int row, int col) {
			board.placeTileAt(tile, row, col);
		}

		public SPlayer createPlayerAtPos(String color, List<Tile> hand, 
		                                 IPlayer iplayer, Posn posn, Board board) {
			SPlayer player = new SPlayer(color, hand, iplayer);
			player.playerState = SPlayer.State.Playing;
			board.addPlayerToBoard(color, posn);
			return player;
		}

		public Admin createAdminWithDrawPile(List<Tile> drawPile) {
			return new Admin(new List<SPlayer>(), new List<SPlayer>(), null, drawPile);
		}

    }
}
