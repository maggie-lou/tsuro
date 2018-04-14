using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tsuro
{
    interface ITile
    {
        //tile orientation
         /* 0   1
        +-----------+
        |           |
      7 |           |  2
        |           |
      6 |           |  3
        |           |
        +-----------+
            5   4*/
        void rotate();
        bool onBoard();
    }

    public class Tile
    {
        public List<Path> paths;

        public Tile(List<Path> pt)
        {
            paths = pt;
        }

        public void rotate()
        {
            foreach (Path p in paths)
            {
                p.loc1 = (p.loc1+ 2) % 8;
                p.loc2 = (p.loc2+ 2) % 8;
            }
        }

        public bool onBoard()
        {
            throw new NotImplementedException();

        }

        public bool isEqual(Tile t)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                if (!(paths.ElementAt(i).isEqual(t.paths.ElementAt(i))))
                {
                    return false;
                }    
            }
            return true;
        }

    }
}
