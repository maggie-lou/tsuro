using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tsuro;
using System.Diagnostics;

namespace TsuroTests
{
   [TestClass]
    public class Tournament
    {
        [TestMethod]
        [DeploymentItem("drawPilepaths.txt")]
        public void RunATournament()
        {
			TestScenerios test = new TestScenerios();
			int numOfTournaments = 20;
            int randomWins = 0;
            int lstSymWins = 0;
            int mostSymWins = 0;


			for (int i = 0; i < numOfTournaments; i++)
			{
				// Initialize tournament
				Admin a = new Admin();

				List<string> playerOrder = new List<string>() { "blue", "hotpink", "green" };

				SPlayer randomPlayer = new SPlayer(new RandomPlayer());
				randomPlayer.initialize(playerOrder[0], playerOrder);
                
				SPlayer leastSymPlayer = new SPlayer(new LeastSymmetricPlayer());
				leastSymPlayer.initialize(playerOrder[1], playerOrder);

				SPlayer mostSymPlayer = new SPlayer(new MostSymmetricPlayer());
				mostSymPlayer.initialize(playerOrder[2], playerOrder);            

				// Start game play
				List<SPlayer> winners = a.play(new List<SPlayer> {randomPlayer, leastSymPlayer, mostSymPlayer});
               

                foreach (SPlayer p in winners)
                {
                    if (p.getColor() == "blue")
                    {
                        randomWins++;
                    }
                    else if (p.getColor() == "hotpink")
                    {
                        lstSymWins++;
                    }
                    else
                    {
                        mostSymWins++;
                    }
                    Console.WriteLine(p.getColor() + " has won!");
                }
			}

			Console.WriteLine("Random Player Wins: " + randomWins + "/" + numOfTournaments);
			Console.WriteLine("Least Symmetric Player Wins: " + lstSymWins + "/" + numOfTournaments);
			Console.WriteLine("Most Symmetric Player Wins: " + mostSymWins + "/" + numOfTournaments);
        }
    }
}
