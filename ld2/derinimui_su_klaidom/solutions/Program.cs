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
		var task01U1 = new Dzukija2010U2();

		foreach( var input in Dzukija2010U2.TestData.Inputs )
		{
			var output = task01U1.Run(input);
			for (int i = 0; i < input.dienos; i++)
			{
				Console.WriteLine(output.ats[i]);
			}
            //TODO: log output, input pair here			
        }
        mLog.Info("First Done");
        //run solution for the second task
        var task02U1 = new Dzukija2011U2();

		foreach (var input in Dzukija2011U2.TestData.Inputs)
		{
			var output = task02U1.Run(input);
			Console.WriteLine("Kopeciu skaicius: {0}", output.kk);
			Console.WriteLine("Zalciu skaicius: {0}", output.zk);
			if (output.kk > 0)
			{
                Console.WriteLine("Didziausias pakilimas: {0}", output.dp);
            }
			if (output.zk > 0)
			{
                Console.WriteLine("Didziausias nusileidimas: {0}", output.dn);
            }
			
			Console.WriteLine("Kopecios:");
			if (output.kk > 0)
			{
				for (int i = 0; i < output.kk; i++)
				{
					Console.WriteLine(output.kopecios[i]);
				}
			}
            Console.WriteLine("Zalciai:");
			if (output.zk > 0)
			{
				for (int i = 0; i < output.zk; i++)
				{
					Console.WriteLine(output.zalciai[i]);
				}
			}
			Console.WriteLine();
            //TODO: log output, input pair here			
        }
        mLog.Info("Second Done");
        //run solution for the third task
        var task03U1 = new Dzukija2012U2();

		foreach( var input in Dzukija2012U2.TestData.Inputs )
		{
			var output = task03U1.Run(input);
			//TODO: log output, input pair here		
			for (int i = 0; i < input.dominanciosvietos; i++) {
				Console.Write(input.vietos[i]);
                Console.Write(" ");
                Console.WriteLine(output.ats[i]);
            }
        }

        //
        mLog.Info("All done.");
	}
}