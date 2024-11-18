namespace Solutions;

using NLog;


/// <summary>
/// Solution for Metai Regionas.
/// </summary>
public class SZ03201903U2
{
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

    public class Input
    {
        public List<Dwarf> Dwarves { get; set; }

        public Input(List<Dwarf> dwarves)
        {
            Dwarves = dwarves;
        }
    }

    public class Output
    {
        public List<(string Name1, string Name2, double Distance)> Friends { get; set; }
        public (string Name1, string Name2) BestFriends { get; set; }

        public Output()
        {
            Friends = new List<(string, string, double)>();
        }
    }

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

    Logger mLog = LogManager.GetCurrentClassLogger();

    public Output Run(Input input)
    {
        mLog.Info("Starting to solve the task");

        Output output = new Output();
        Dictionary<Dwarf, Dwarf> bestFriends = new Dictionary<Dwarf, Dwarf>();

        var rwLock = new ReaderWriterLock();
        const int timeout = 500; // Timeout in milliseconds

        foreach (var dwarf in input.Dwarves)
        {
            Dwarf currentDwarf = dwarf;
            Dwarf threadBestFriend = null;
            double threadMinDistance = double.MaxValue;

            Parallel.ForEach(input.Dwarves, other =>
            {
                Thread.Sleep(1 + Random.Shared.Next(10));
                if (currentDwarf != other)
                {
                    double distance = currentDwarf.DistanceTo(other);

                    rwLock.AcquireReaderLock(timeout);
                    try
                    {
                        if (distance < threadMinDistance)
                        {
                            LockCookie lockCookie = rwLock.UpgradeToWriterLock(timeout);
                            try
                            {
                                if (distance < threadMinDistance)
                                {
                                    threadBestFriend = other;
                                    threadMinDistance = distance;
                                }
                            }
                            finally
                            {
                                rwLock.DowngradeFromWriterLock(ref lockCookie);
                            }
                        }
                    }
                    finally
                    {
                        rwLock.ReleaseReaderLock();
                    }
                }
            });

            rwLock.AcquireWriterLock(timeout);
            try
            {
                bestFriends[currentDwarf] = threadBestFriend;
                output.Friends.Add((currentDwarf.Name, threadBestFriend.Name, threadMinDistance));
            }
            finally
            {
                rwLock.ReleaseWriterLock();
            }

            mLog.Info($"Dwarf {currentDwarf.Name} considers {threadBestFriend.Name} as their best friend with a distance of {threadMinDistance:F4}");
        }

        Parallel.ForEach(input.Dwarves, dwarf =>
        {
            Thread.Sleep(1 + Random.Shared.Next(10));

            rwLock.AcquireReaderLock(timeout);
            bool needUpdate = false;
            Dwarf friend = null;
            try
            {
                friend = bestFriends[dwarf];
                if (bestFriends[friend] == dwarf)
                {
                    needUpdate = true;
                }
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }

            if (needUpdate)
            {
                rwLock.AcquireWriterLock(timeout);
                try
                {
                    if (output.BestFriends == default ||
                        input.Dwarves.IndexOf(dwarf) < input.Dwarves.IndexOf(bestFriends.FirstOrDefault(
                            bf => bf.Key.Name == output.BestFriends.Name1).Key))
                    {
                        output.BestFriends = (dwarf.Name, friend.Name);
                    }
                }
                finally
                {
                    rwLock.ReleaseWriterLock();
                }
            }
        });

        mLog.Info($"The best friends are: {output.BestFriends.Name1} and {output.BestFriends.Name2}");
        return output;
    }
}
