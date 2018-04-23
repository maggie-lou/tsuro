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
        // returns a rotated tile
        Tile rotate();
        // takes in a location on a tile and returns the end location of path
        int getLocationEnd(int n);
        // returns whether tile is on board
        bool onBoard();
        // returns whether a tile is equal to another
        // (rotated tile should still be equal)
        bool isEqual(Tile t);
    }

    public class Tile:ITile
    {
        public List<Path> paths;

        public Tile(List<Path> pt)
        {
            paths = pt;
        }

        public Tile rotate()
        {
            Tile rotated_tile = this;
            foreach (Path p in rotated_tile.paths)
            {
                p.loc1 = (p.loc1+ 2) % 8;
                p.loc2 = (p.loc2+ 2) % 8;
            }
            return rotated_tile;
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
                Tile rotated_tile = t;
                bool path_isequal = false;
                for (int j = 0; j < 4; j++)
                {
                    if (!(paths.ElementAt(i).isEqual(rotated_tile.paths.ElementAt(i))))
                    {
                        rotated_tile = rotated_tile.rotate();
                    }
                    else
                    {
                        path_isequal = true;
                        break;
                    }
                }
                if (!path_isequal)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
