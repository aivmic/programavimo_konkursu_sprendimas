﻿namespace Solutions;

using NLog;


/// <summary>
/// Solution for Metai Regionas.
/// </summary>
public class SZ02201802U2
{
    /// <summary>
    /// A single input.
    /// </summary>
    public class Input
    {
        public Player[] TeamA { get; set; }
        public Player[] TeamB { get; set; }
    }

    /// <summary>
    /// A single output.
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
        public int[] Shots { get; set; }

        public int GetTotalPoints()
        {
            int total = 0;
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
                if (Shots[i] > 0)
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
    /// Test data. You can use a different layout if you want.
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

        int teamAPoints = CalculateTotalPoints(input.TeamA);
        int teamBPoints = CalculateTotalPoints(input.TeamB);

        mLog.Info($"Komanda A surinko {teamAPoints} taškų.");
        mLog.Info($"Komanda B surinko {teamBPoints} taškų.");

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
    /// Calculate total points for a team.
    /// </summary>
    private int CalculateTotalPoints(Player[] team)
    {
        int totalPoints = 0;

        Parallel.For(0, team.Length, i =>
        {
            Thread.Sleep(1 + Random.Shared.Next(10));
            int points = team[i].GetTotalPoints();
            Interlocked.Add(ref totalPoints, points);
        });

        return totalPoints;

    }

    /// <summary>
    /// Get top scorer from a team with incorrect Monitor usage.
    /// </summary>
    private Player GetTopScorer(Player[] team)
    {
        if (team == null || team.Length == 0)
            return null;
        if (team.Length == 1)
            return team[0];

        int bestPoints = int.MinValue;
        Player bestPlayer = null;
        object lockObject = new object();

        Parallel.ForEach(team, player =>
        {
            Thread.Sleep(1 + Random.Shared.Next(10));
            int currentPoints = player.GetTotalPoints();

            Monitor.Enter(lockObject);
            if (currentPoints > bestPoints ||
            (currentPoints == bestPoints && player.Number < bestPlayer.Number))
            {
                bestPoints = currentPoints;
                bestPlayer = player;
            }
        });

        return bestPlayer;
    }


    /// <summary>
    /// Print results to the console.
    /// </summary>
    public void PrintResults(Output output)
    {
        Console.WriteLine($"Laimėjo {output.WinningTeam} komanda.");
        Console.WriteLine($"Rezultatyviausio {output.WinningTeam} komandos žaidėjo nr. {output.TopPlayerNumber}.");
        Console.WriteLine($"(Pelnyta taškų: {output.Points}; taiklių metimų sk.: {output.SuccessfulShots}; netaiklių metimų sk.: {output.MissedShots})");
    }
}