using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tsuro
{
    public class AutomatedPlayer
    {
        protected string name = "";
        protected List<string> allPlayers = new List<string>();
        protected string[] validNames = new string[] {"blue","red","green","orange","sienna"
            ,"hotpink","darkgreen","purple"};

        public virtual string getName()
        {
            return name;
        }

        public virtual void initialize(string playerColor, List<string> allColors)
        {
            allPlayers = allColors;
            if (validNames.Contains(playerColor))
            {
                name = playerColor;
            }
            else
            {
                throw new Exception("not a valid color!");
            }
        }

        public virtual Posn placePawn(Board b)
        {
            // row is either 0(0 and 1) or 5(4 and 5)
            // col is either 0(6 and 7) or 5(2 and 3)
            int[] edgeRows = new int[] { 0, 5 };
            int[] edgeCols = new int[] { 0, 5 };
            Dictionary<int, int[]> edgeRowLoc = new Dictionary<int, int[]>();
            edgeRowLoc.Add(0, new int[] { 0, 1 });
            edgeRowLoc.Add(5, new int[] { 4, 5 });
            Dictionary<int, int[]> edgeColLoc = new Dictionary<int, int[]>();
            edgeColLoc.Add(0, new int[] { 6, 7 });
            edgeColLoc.Add(5, new int[] { 2, 3 });

            for (int i = 0; i < edgeRows.Length; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    foreach (int loc in edgeRowLoc[edgeRows[i]])
                    {
                        if (!b.locationOccupied(edgeRows[i], j, loc))
                        {
                            return new Posn(edgeRows[i], j, loc);
                        }
                    }
                }
            }

            for (int i = 0; i < edgeCols.Length; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    foreach (int loc in edgeColLoc[edgeCols[i]])
                    {
                        if (!b.locationOccupied(j, edgeCols[i], loc))
                        {
                            return new Posn(j, edgeCols[i], loc);
                        }
                    }
                }
            }
            throw new Exception("Edges of Board are Full.");
        }

        public virtual void endGame(Board b, List<string> allColors)
        {
            throw new NotImplementedException();
        }
    }
}

