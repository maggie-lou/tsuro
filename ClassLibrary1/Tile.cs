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
        bool isEqualOrRotation(Tile t);
    }

	[Serializable]
    public class Tile : ITile
    {
        public List<Path> paths;
		int symmetricity; 

        public Tile(List<Path> pt)
        {
            paths = pt;
			symmetricity = -1;
        }

        public Tile()
        {
            paths = new List<Path>();
			symmetricity = -1;
        }
        
        // Lazily computes the symmetricity of the tile
		public int lazyGetSymmetricity() {
			if (symmetricity == -1) {
				symmetricity = generateSymmetricity();
			}

			return symmetricity;
		}

        // The symmetricity equals the numer of different ways a tile can be rotated
        // to yield different paths
        // (i.e. symmetricity is 1 if no matter how it is rotated, the pathways are 
        // always the same)
		public int generateSymmetricity() {
			return getDifferentRotations().Count;
		}

        // Returns a list of different rotations of the current tile
        //
        // Different rotations have different pathways
		public List<Tile> getDifferentRotations() {
			List<Tile> diffRotations = new List<Tile>();
			Tile temp = this;

			for (int rotations = 0; rotations < 4; rotations++) {
				if (!contains(diffRotations, temp)) {
				    diffRotations.Add(temp);
				}
				temp = temp.rotate();
			}
			return diffRotations;
		}

        // Returns true if tiles contains tile t
        //
        // Only considers the current rotation of tile t
        // I.e. will return false if tiles only contains a different rotation of t
		public static bool contains(List<Tile> tiles, Tile target) {
			foreach (Tile t in tiles ) {
				if (t.isEqual(target)) {
					return true;
				}
			}
			return false;
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

        // Returns true if the current tile is equal to t
        //
		// Tiles are equal if they have the same paths, regardless of their ordering
		// (i.e. Tile [(0,1) (2,3) (4,5) (6,7)] is equal to Tile [(2,3) (4,5) (0,1) (6,7)]
		public bool isEqual(Tile t) {
			List<Path> currentTilePaths = new List<Path> { this.paths[0], this.paths[1], this.paths[2], this.paths[3] };
            foreach (Path p in t.paths)
            {
                bool foundMatch = false;

                // Check if path p is in the current tile's list of paths, at any position
                for (int i = 0; i < currentTilePaths.Count; i++)
                {
                    if (p.isEqual(currentTilePaths[i]))
                    {
                        foundMatch = true;
                        currentTilePaths.RemoveAt(i);
                        i--;
                        break;
                    }
                }

                // If path p isn't found in the current tile's list of paths, tiles aren't equal
                if (!foundMatch)
                {
					return false;
                }
            }

			return true;
		}

		// Returns true if the current tile is equal to or a rotation of t
        public bool isEqualOrRotation(Tile t)
        {
			Tile temp = this;
			for (int rotations = 0; rotations < 4; rotations++)
			{
				if (temp.isEqual(t)) {
					return true;
				}
				temp = temp.rotate();
			}

			return false; 
        }

        // Sorts input tiles in place, in descending order
		// (i.e. first tile in sorted list is the most symmetric)
		public static void sortBySymmetricity(List<Tile> unsortedTiles) {
			unsortedTiles.Sort((x, y) => (x.lazyGetSymmetricity().CompareTo(y.lazyGetSymmetricity())));
		}
    }
}
