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

    public class Tile:ITile
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

        public int getLocationEnd(int n)
        {
            foreach (Path p in paths)
            {
                if (p.inPath(n))
                {
                    if(p.loc1 == n)
                    {
                        return p.loc2;
                    }
                    else
                    {
                        return p.loc1;
                    }
                }
            }
            //something went wrong, number not in any path
            return -1;
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
