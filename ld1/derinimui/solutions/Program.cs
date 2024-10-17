namespace Solutions;

using NLog;
using NLog.Config;
using NLog.Targets;


/// <summary>
/// Program entry class.
/// </summary>
public class Program
{
    /// <summary>
    /// Logger for this class.
    /// </summary>
    Logger mLog = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Program entry point.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        //configure logging
        var console = new ConsoleTarget("console");
        console.Layout = @"[${date:format=HH\:MM\:ss}][${logger}]: ${message}";

        var cfg = new LoggingConfiguration();
        cfg.AddRule(LogLevel.Info, LogLevel.Fatal, console);

        LogManager.Configuration = cfg;

        //run
        var self = new Program();
        self.Run(args);
    }

    /// <summary>
    /// Program body.
    /// </summary>
    /// <param name="args">Command line argumens.</param>
    void Run(string[] args)
    {
        mLog.Info("Starting.");

        //run solution for the first task
        var task01U1 = new AZ01201701U2();

        foreach (var input in AZ01201701U2.TestData.Inputs)
        {
            var output = task01U1.Run(input);
            // Log input
            mLog.Info($"Input: {string.Join(", ", input.Sequences)}");

            // Log output
            mLog.Info($"Output: {string.Join("\n", output.Results)}");
        }

        //run solution for the second task
        var task02U1 = new SZ02201802U2();

        foreach (var input in SZ02201802U2.TestData.Inputs)
        {
            var output = task02U1.Run(input);

            // Log input
            foreach (var player in input.TeamA)
            {
                mLog.Info($"Team A - Player {player.Number}: Shots = {string.Join(", ", player.Shots)}");
            }
            foreach (var player in input.TeamB)
            {
                mLog.Info($"Team B - Player {player.Number}: Shots = {string.Join(", ", player.Shots)}");
            }

            // Log output
            mLog.Info($"Output: Winning Team = {output.WinningTeam}," +
                $" Top Player Number = {output.TopPlayerNumber}, Points = {output.Points}," +
                $" Successful Shots = {output.SuccessfulShots}, Missed Shots = {output.MissedShots}");
        }

        //run solution for the third task
        var task03U1 = new SZ03201903U2();

        foreach (var input in SZ03201903U2.TestData.Inputs)
        {
            var output = task03U1.Run(input);

            // Log input
            foreach (var dwarf in input.Dwarves)
            {
                mLog.Info($"Nykštukas: {dwarf.Name}, Koordinatės: ({dwarf.X}, {dwarf.Y})");
            }


            // Log output
            foreach (var friend in output.Friends)
            {
                Console.WriteLine($"{friend.Name1} {friend.Name2} {friend.Distance:F4}");
            }

            Console.WriteLine($"{output.BestFriends.Name1} {output.BestFriends.Name2}");
        }
        mLog.Info("All done.");
    }
}