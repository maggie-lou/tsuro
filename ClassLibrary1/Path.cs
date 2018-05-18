using System;
using System.Collections.Generic;
using System.Text;

namespace tsuro
{
	[Serializable]
    public class Path
    {
        public int loc1;
        public int loc2;

        public Path()
        {
            loc1 = -1;
            loc2 = -1;
        }
        
        public Path(int one, int two)
        {
            loc1 = one;
            loc2 = two;
        } 

        public bool isEqual(Path p)
        {
            return (((p.loc1 == loc1) && (p.loc2 == loc2))|| ((p.loc1 == loc2) && (p.loc2 == loc1)));
        }

        public bool inPath(int n)
        {
            if(n == loc1 || n == loc2)
            {
                return true;
            }
            return false; 
        }

    }
}
