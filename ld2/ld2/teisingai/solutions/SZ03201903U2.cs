namespace Solutions;

using NLog;


/// <summary>
/// Solution for Metai Regionas.
/// </summary>
public class SZ03201903U2
{
    /// <summary>
    /// Data class that stores information about a dwarf.
    /// </summary>
    public class Dwarf
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public Dwarf(string name, double x, double y)
        {
            Name = name;
            X = x;
            Y = y;
        }

        public double DistanceTo(Dwarf other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }
    }

    /// <summary>
    /// A single input.
    /// </summary>
    public class Input
    {
        public List<Dwarf> Dwarves { get; set; }

        public Input(List<Dwarf> dwarves)
        {
            Dwarves = dwarves;
        }
    }

    /// <summary>
    /// A single output.
    /// </summary>
    public class Output
    {
        public List<(string Name1, string Name2, double Distance)> Friends { get; set; }
        public (string Name1, string Name2) BestFriends { get; set; }

        public Output()
        {
            Friends = new List<(string, string, double)>();
        }
    }

    /// <summary>
    /// Test data. You can use a different layout if you want.
    /// </summary>
    public class TestData
    {
        public static Input[] Inputs { get; } = {
                new Input(new List<Dwarf>
                {
                    new Dwarf("EM", 8, 9),
                    new Dwarf("AA", 1, 3),
                    new Dwarf("BA", 1.2, 5),
                    new Dwarf("CB", 3, 1.4),
                    new Dwarf("DE", 4.5, 2)
                })
            };
        public static Output[] Outputs { get; } = {
                new Output
                {
                    Friends = new List<(string, string, double)>
                    {
                        ("EM", "DE", 7.8262),
                        ("AA", "BA", 2.0100),
                        ("BA", "AA", 2.0100),
                        ("CB", "DE", 1.6155),
                        ("DE", "CB", 1.6155)
                    },
                    BestFriends = ("AA", "BA")
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
        mLog.Info("Starting to solve the task");

        Output output = new Output();
        Dictionary<Dwarf, Dwarf> bestFriends = new Dictionary<Dwarf, Dwarf>();

        foreach (var dwarf in input.Dwarves)
        {
            Dwarf bestFriend = null;
            double minDistance = double.MaxValue;


            Parallel.ForEach(input.Dwarves, other =>
            {
                if (dwarf != other)
                {
                    double distance = dwarf.DistanceTo(other);

                    if (distance < minDistance)
                    {
                        bestFriend = other;
                        minDistance = distance;
                    }
                }
            });

            bestFriends[dwarf] = bestFriend;
            output.Friends.Add((dwarf.Name, bestFriend.Name, minDistance));


            mLog.Info($"Dwarf {dwarf.Name} considers {bestFriend.Name} as their best friend with a distance of {minDistance:F4}");
        }

        Parallel.ForEach(input.Dwarves, dwarf =>
        {
            var friend = bestFriends[dwarf];

            if (bestFriends[friend] == dwarf)
            {
                lock (output)
                {
                    if (output.BestFriends == default || input.Dwarves.IndexOf(dwarf) < input.Dwarves.IndexOf(bestFriends.FirstOrDefault(bf => bf.Key.Name == output.BestFriends.Name1).Key))
                    {
                        output.BestFriends = (dwarf.Name, friend.Name);
                    }
                }
            }
        });

        mLog.Info($"The best friends are: {output.BestFriends.Name1} and {output.BestFriends.Name2}");
        return output;
    }
}