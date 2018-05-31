using System;
using System.Collections.Generic;
using System.Text;

namespace tsuro
{
	[Serializable]
    public class Posn
    {
        int row;
        int col;
        int locOnTile;

        public Posn()
        {
            row = -1;
            col = -1;
            locOnTile = -1;
        }
        public Posn(int r, int c, int l)
        {
            row = r;
            col = c;
            locOnTile = l;
        }

        public int returnRow()
        {
            return row;
        }
        public int returnCol()
        {
            return col;
        }
        public int returnLocationOnTile()
        {
            return locOnTile;
        }

        public void setPlayerPosn(int r, int c, int l)
        {
            row = r;
            col = c;
            locOnTile = l;
        }

        public bool isEqual(Posn checkP)
        {
            if((checkP.returnRow()==row) && (checkP.returnCol() ==col) && 
                (checkP.returnLocationOnTile() == locOnTile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
