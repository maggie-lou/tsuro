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

        public static Posn generateRandomStartPosn() {
            Random random = new Random();
            // 0 : Start on top/bottom of board
            // 1 : Start on left/right
            int leadingSide = random.Next(0, 2);

            int nonLeadingCoord = random.Next(0, 6);
            int[] leadingCoordOptions = new int[] { -1, 6 };
            int leadingCoord = leadingCoordOptions[random.Next(leadingCoordOptions.Length)];
            if (leadingSide == 0)
            {
                int tileLoc = getRandomStartTilePosition(leadingCoord, nonLeadingCoord);
                return new Posn(leadingCoord,nonLeadingCoord,tileLoc);            
            }
            else
            {
                int tileLoc = getRandomStartTilePosition(nonLeadingCoord, leadingCoord);
                return new Posn(nonLeadingCoord,leadingCoord,tileLoc);            
            }
        }

        public static int getRandomStartTilePosition(int row, int col)
        {
            int[] validTilePositions;
            if (col == -1)
            {
                validTilePositions = new int[] { 2, 3 };
            }
            else if (col == 6)
            {
                validTilePositions = new int[] { 6, 7 };
            }
            else if (row == -1)
            {
                validTilePositions = new int[] { 4, 5 };
            }
            else if (row == 6)
            {
                validTilePositions = new int[] { 0, 1 };
            }
            else
            {
                throw new Exception("Invalid row and column inputs when generating random start tile position.");

            }

            Random random = new Random();
            return validTilePositions[random.Next(2)];
        }

        public virtual Posn placePawn(Board b)
        {
            Posn randomStartPos = generateRandomStartPosn();
   
            while (b.locationOccupied(randomStartPos))
            {
                randomStartPos = generateRandomStartPosn();
            }
            return randomStartPos;
        }

        public virtual void endGame(Board b, List<string> allColors)
        {
            throw new NotImplementedException();
        }
    }
}

