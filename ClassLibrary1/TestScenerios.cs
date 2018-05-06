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
    }
}
