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
        // returns whether a tile is equal to another
        // (rotated tile should still be equal)
        bool isEqual(Tile t);

        //returns whether paths change when you rotate the tile, i.e if it is not symmetrical 
        bool isSymmetric(Tile t);

        //tells if a tile is 1,2, 4 symmetric, should only return these numbers
        int howSymmetric();
    }

	[Serializable]
    public class Tile : ITile
    {
        public List<Path> paths;

        public Tile(List<Path> pt)
        {
            paths = pt;
        }

        public Tile()
        {
            paths = new List<Path>();
        }
        

        public Tile rotate()
        {
            //Tile rotated_tile = this;
            Tile rotatedTile = new Tile();

            foreach (Path p in this.paths)
            {
                int loc1 = (p.loc1 + 2) % 8;
                int loc2 = (p.loc2 + 2) % 8;
                Path newPath = new Path(loc1, loc2);
                rotatedTile.paths.Add(newPath);
            }
            return rotatedTile;
        }

        public int getLocationEnd(int n)
        {
            foreach (Path p in paths)
            {
                if (p.inPath(n))
                {
                    if (p.loc1 == n)
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

        // Returns true if the current tile is equal to or a rotation of t
        // 
        // Tiles are equal if they have the same paths, regardless of their ordering
        public bool isEqual(Tile t)
        {
			Tile temp = this;
			for (int rotations = 0; rotations < 4; rotations++)
			{
				List<Path> currentTilePaths = new List<Path> { temp.paths[0], temp.paths[1], temp.paths[2], temp.paths[3] };
				// Check is paths in t are the same
				int numPathsEqual = 0;
				foreach (Path p in t.paths)
				{
					bool foundMatch = false; 
					for (int i = 0; i < currentTilePaths.Count; i++)
					{
						if (p.isEqual(currentTilePaths[i]))
						{
							numPathsEqual++;
							foundMatch = true;
							currentTilePaths.RemoveAt(i);
							i--;
							break;
						}
					}
					if (!foundMatch) {
						break;
					}
				}
                
				if (numPathsEqual == 4)
				{
					return true;
				}
				temp = temp.rotate();
			}

			return false; 
        }

        public bool isSymmetric(Tile t)
        {
            int i = 0;
            bool[] symmetric = new bool[4] { false,false,false,false};
     
            foreach (Path p in paths)
            {
                foreach (Path pCheck in t.paths)
                {
                    if (p.isEqual(pCheck))
                    {
                        symmetric[i] = true;
                        break;
                    }
                }
                i++;
            }

            return symmetric.All(x => x);
      
        }

        public int howSymmetric()
        {
            int timesSymmetric = 0;
            Tile checkTile = this.rotate();

            for (int i = 0; i < 4; i++)
            {
                if (this.isSymmetric(checkTile)){
                    timesSymmetric++;
                }
                checkTile = checkTile.rotate();
            }

            return timesSymmetric;
        }
    }
}
