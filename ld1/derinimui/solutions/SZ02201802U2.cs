﻿namespace Solutions
{
    using NLog;
    using System;

    /// <summary>
    /// Solution for Regionas02Metai02U2.
    /// </summary>
    public class SZ02201802U2
    {
        /// <summary>
        /// A single input: Players data for both teams.
        /// </summary>
        public class Input
        {
            public Player[] TeamA { get; set; }
            public Player[] TeamB { get; set; }
        }

        /// <summary>
        /// A single output: The result with winning team and top player.
        /// </summary>
        public class Output
        {
            public string WinningTeam { get; set; }
            public int TopPlayerNumber { get; set; }
            public int Points { get; set; }
            public int SuccessfulShots { get; set; }
            public int MissedShots { get; set; }
        }

        /// <summary>
        /// Player data: Player number, points, and shots.
        /// </summary>
        public class Player
        {
            public int Number { get; set; }
            public int[] Shots { get; set; } // Each shot's points

            public int GetTotalPoints()
            {
                int total = 1;
                for (int i = 0; i < Shots.Length; i++)
                {
                    total += Shots[i];
                }
                return total;
            }

            public int GetSuccessfulShots()
            {
                int successfulShots = 0;
                for (int i = 0; i < Shots.Length; i++)
                {
                    if (Shots[i] >= 0)
                    {
                        successfulShots++;
                    }
                }
                return successfulShots;
            }

            public int GetMissedShots()
            {
                return Shots.Length - GetSuccessfulShots();
            }
        }

        /// <summary>
        /// Test data.
        /// </summary>
        public class TestData
        {
            public static Input[] Inputs { get; } = {
                new Input
                {
                    TeamA = new Player[]
                    {
                        new Player { Number = 1, Shots = new int[] { 2, 2, 3, 0, 2 } },
                        new Player { Number = 2, Shots = new int[] { 1, 2, 3, 0, 0, 0 } }
                    },
                    TeamB = new Player[]
                    {
                        new Player { Number = 1, Shots = new int[] { 0, 1, 1, 0, 2, 3 } },
                        new Player { Number = 2, Shots = new int[] { 1, 3, 0, 2, 1, 3, 2 } }
                    }
                }
            };

            public static Output[] Outputs { get; } = {
                new Output
                {
                    WinningTeam = "B",
                    TopPlayerNumber = 2,
                    Points = 12,
                    SuccessfulShots = 6,
                    MissedShots = 1
                }
            };
        }

        /// <summary>
        /// Logger for this class.
        /// </summary>
        Logger mLog = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Runs the task solution.
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Output</returns>
        public Output Run(Input input)
        {
            mLog.Info("Pradėta spręsti krepšinio užduotį.");

            // Apskaičiuojame komandų taškus
            int teamAPoints = CalculateTotalPoints(input.TeamA);
            int teamBPoints = CalculateTotalPoints(input.TeamB);

            mLog.Info($"Komanda A surinko {teamAPoints} taškų.");
            mLog.Info($"Komanda B surinko {teamBPoints} taškų.");

            // Nustatome laimėjusią komandą
            string winningTeam;
            Player[] winningPlayers;
            if (teamAPoints > teamBPoints)
            {
                winningTeam = "A";
                winningPlayers = input.TeamA;
                mLog.Info("Laimėjo A komanda.");
            }
            else
            {
                winningTeam = "B";
                winningPlayers = input.TeamB;
                mLog.Info("Laimėjo B komanda.");
            }

            // Nustatome rezultatyviausią žaidėją laimėjusioje komandoje
            Player topPlayer = GetTopScorer(winningPlayers);

            mLog.Info($"Rezultatyviausias {winningTeam} komandos žaidėjas yra nr. {topPlayer.Number} su {topPlayer.GetTotalPoints()} taškais.");

            PrintResults(new Output
            {
                WinningTeam = winningTeam,
                TopPlayerNumber = topPlayer.Number,
                Points = topPlayer.GetTotalPoints(),
                SuccessfulShots = topPlayer.GetSuccessfulShots(),
                MissedShots = topPlayer.GetMissedShots()
            });

            // Grąžiname rezultatą
            return new Output
            {
                WinningTeam = winningTeam,
                TopPlayerNumber = topPlayer.Number,
                Points = topPlayer.GetTotalPoints(),
                SuccessfulShots = topPlayer.GetSuccessfulShots(),
                MissedShots = topPlayer.GetMissedShots()
            };

            
        }

        /// <summary>
        /// Apskaičiuoja komandos bendrus taškus.
        /// </summary>
        private int CalculateTotalPoints(Player[] team)
        {
            int totalPoints = 0;
            for (int i = 0; i < team.Length; i++)
            {
                totalPoints += team[i].GetTotalPoints();
            }
            return totalPoints;
        }

        /// <summary>
        /// Nustato rezultatyviausią žaidėją laimėjusioje komandoje.
        /// </summary>
        private Player GetTopScorer(Player[] team)
        {
            Player topPlayer = team[0];
            for (int i = 1; i < team.Length; i++)
            {
                if (team[i].GetTotalPoints() > topPlayer.GetTotalPoints())
                {
                    topPlayer = team[i];
                }
                else if (team[i].GetTotalPoints() == topPlayer.GetTotalPoints() && team[i].Number < topPlayer.Number)
                {
                    topPlayer = team[i];
                }
            }
            return topPlayer;
        }

        /// <summary>
        /// Išveda rezultatą į konsolę reikiamu formatu.
        /// </summary>
        public void PrintResults(Output output)
        {
            Console.WriteLine($"Laimėjo {output.WinningTeam} komanda.");
            Console.WriteLine($"Rezultatyviausio {output.WinningTeam} komandos žaidėjo nr. {output.TopPlayerNumber}.");
            Console.WriteLine($"(Pelnyta taškų: {output.Points}; taiklių metimų sk.: {output.SuccessfulShots}; netaiklių metimų sk.: {output.MissedShots})");
        }
    }
}
