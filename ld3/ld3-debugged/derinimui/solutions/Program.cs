using NLog;
using NLog.Config;
using NLog.Targets;

namespace Solutions;

/// <summary>
/// Program entry class.
/// </summary>
public class Program
{
    /// <summary>
    /// Logger for this class.
    /// </summary>
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// The number of threads to use for the parallel processing.
    /// </summary>
    public static readonly int Threads = Environment.ProcessorCount;

    /// <summary>
    /// Static constructor.
    /// </summary>
    static Program()
    {
        ConfigureLogger();
    }

    /// <summary>
    /// Configures logger.
    /// </summary>
    private static void ConfigureLogger()
    {
        var configuration = new LoggingConfiguration();

        configuration.AddRule(
            LogLevel.Info,
            LogLevel.Fatal,
            new ConsoleTarget("console")
            {
                Layout = @"[${date:format=HH\:MM\:ss}][${logger}]: ${message}"
            }
        );

        LogManager.Configuration = configuration;
    }

    /// <summary>
    /// Program entry point.
    /// </summary>
    /// <param name="_">Command line arguments.</param>
    public static void Main(string[] _) => new Program()
        .Run();

    /// <summary>
    /// Program body.
    /// </summary>
    private void Run()
    {
        Logger.Info("Starting.");

        var divider = "-".Repeat(60);

        Logger.Info(divider);

        RunFirstTask();

        Logger.Info(divider);

        RunSecondTask();

        Logger.Info(divider);

        RunThirdTask();

        Logger.Info(divider);
        Logger.Info("Done.");
    }

    /// <summary>
    /// Runs the first task.
    /// </summary>
    private void RunFirstTask()
    {
        Logger.Info(nameof(Forumas2017U2));

        for (var i = 0; i < Forumas2017U2.TestData.Inputs.Length; i++)
        {
            var input = Forumas2017U2.TestData.Inputs[i];

            Logger.Info($"#{i + 1}.");
            Logger.Info("Input:");
            Logger.Info($" - Words Count: {input.Count}");
            Logger.Info($" - Initial Word: {input.WordInitial}");

            var output = Forumas2017U2.Run(input);

            Logger.Info("Output:");

            output.Results
                .ToList()
                .ForEach(result => Logger.Info($"   - {result}"));
        }
    }


    /// <summary>
    /// Runs the second task.
    /// </summary>
    private void RunSecondTask()
    {
        Logger.Info(nameof(Forumas2019U2));

        for (var i = 0; i < Forumas2019U2.TestData.Inputs.Length; i++)
        {
            var input = Forumas2019U2.TestData.Inputs[i];

            Logger.Info($"#{i + 1}.");
            Logger.Info("Input:");
            Logger.Info($" - Rows: {input.Rows}");
            Logger.Info($" - Columns: {input.Columns}");
            Logger.Info(" - Grid:");

            for (var r = 0; r < input.Rows; r++)
            {
                Logger.Info(
                    "   - " + string.Join(
                        " ",
                        Enumerable
                            .Range(
                                0,
                                input.Columns
                            )
                            .Select(c => input.Grid[r, c])
                    )
                );
            }

            var output = Forumas2019U2.Run(input);

            Logger.Info("Output:");
            Logger.Info(" - Chairs Broken: " + output.ChairsBroken.Count);

            output.ChairsBroken.ForEach(c => Logger.Info($"   - {c}"));
        }
    }

    /// <summary>
    /// Runs the third task.
    /// </summary>
    private void RunThirdTask()
    {
        Logger.Info(nameof(Regionai2022U2));

        for (var i = 0; i < Regionai2022U2.TestData.Inputs.Length; i++)
        {
            var input = Regionai2022U2.TestData.Inputs[i];

            Logger.Info($"#{i + 1}.");
            Logger.Info("Input:");
            Logger.Info($" - Artists Count: {input.ArtistsCount}");
            Logger.Info(" - Songs Per Artist:");

            var output = Regionai2022U2.Run(input);

            for (var a = 0; a < input.ArtistsCount; a++)
            {
                Logger.Info($"   - #{a + 1} Artist: {input.SongsPerArtist[a]}");
            }

            Logger.Info("Output:");
            Logger.Info(" - Playlist:");
            Logger.Info($"   - {string.Join(" -> ", output.Playlist)}");
        }
    }
}