using System.Collections.Concurrent;
using static Solutions.Forumas2019U2.Output;

namespace Solutions;

/// <summary>
/// Solution for 2019 Forumas.
/// </summary>
public class Forumas2019U2
{
    /// <summary>
    /// A single input.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Number of columns in the grid.
        /// </summary>
        public int Columns { get; init; }

        /// <summary>
        /// Grid of the game:
        /// 'L' - chair,
        /// 'X' - player,
        /// '.' - empty space
        /// </summary>
        public char[,] Grid { get; init; }

        /// <summary>
        /// Number of rows in the grid.
        /// </summary>
        public int Rows { get; init; }
    }

    /// <summary>
    /// A single output.
    /// </summary>
    public class Output
    {
        /// <summary>
        /// Broken chairs.
        /// </summary>
        public List<Chair> ChairsBroken { get; init; }

        /// <summary>
        /// A single chair within the grid.
        /// </summary>
        /// <param name="at">Coordinates of the chair</param>
        public class Chair(Coordinate at)
        {
            /// <summary>
            /// Coordinate of the chair.
            /// </summary>
            public Coordinate At { get; } = at;

            /// <summary>
            /// Whether the chair is occupied.
            /// </summary>
            public bool IsOccupied { get; set; }

            public override string ToString() => $"({At.Row + 1} {At.Column + 1})";

            public override bool Equals(object obj)
            {
                if (obj is not Chair other)
                {
                    return false;
                }

                return At == other.At && IsOccupied == other.IsOccupied;
            }

            public override int GetHashCode() => At.GetHashCode();
        }

        /// <summary>
        /// Chair distance to an unspecified player.
        /// </summary>
        /// <param name="Chair">Chair</param>
        /// <param name="Distance">Distance to unspecified player that is holding this instance</param>
        public record ChairDistance(
            Chair Chair,
            int Distance
        );

        /// <summary>
        /// A single coordinate.
        /// </summary>
        /// <param name="Row">Row index in the grid</param>
        /// <param name="Column">Column index in the grid</param>
        public record Coordinate(
            int Row,
            int Column
        )
        {
            public override string ToString() => $"({Row + 1} {Column + 1})";
        }

        /// <summary>
        /// A single player within the grid.
        /// </summary>
        /// <param name="at">Coordinates of the player</param>
        public class Player(Coordinate at)
        {
            /// <summary>
            /// Coordinate of the player.
            /// </summary>
            public Coordinate At { get; } = at;

            /// <summary>
            /// Whether the player is out of the game.
            /// </summary>
            public bool IsOut { get; set; }
        }
    }

    /// <summary>
    /// Test data. You can use a different layout if you want.
    /// </summary>
    public class TestData
    {
        public static Input[] Inputs { get; } =
        [
            new Input
            {
                Columns = 5,
                Grid = new[,]
                {
                    { '.', '.', 'X', 'L', 'X' },
                    { '.', '.', 'X', '.', '.' },
                    { 'X', '.', '.', '.', 'L' },
                    { '.', '.', 'X', '.', '.' }
                },
                Rows = 4
            },
            new Input
            {
                Columns = 7,
                Grid = new[,]
                {
                    { '.', '.', '.', '.', '.', '.', 'X' },
                    { 'X', '.', '.', '.', 'L', 'X', '.' },
                    { '.', '.', 'X', '.', '.', '.', '.' }
                },
                Rows = 3
            },
            new Input
            {
                Columns = 5,
                Grid = new[,]
                {
                    { '.', 'X', 'L', 'X', '.' },
                    { '.', '.', '.', '.', 'L' },
                    { '.', '.', 'X', '.', '.' },
                    { 'L', '.', 'X', '.', '.' }
                },
                Rows = 4
            },
        ];

        public static Output[] Outputs { get; } =
        [
            new Output
            {
                ChairsBroken =
                [
                    new Chair(new Coordinate(0, 3)),
                    new Chair(new Coordinate(2, 4))
                ]
            },
            new Output
            {
                ChairsBroken = []
            },
            new Output
            {
                ChairsBroken =
                [
                    new Chair(new Coordinate(0, 2))
                ]
            },
        ];
    }

    /// <summary>
    /// Calculates distances between chairs and players.
    /// </summary>
    /// <param name="chairs">Chairs in the grid</param>
    /// <param name="players">Players in the grid</param>
    /// <returns>Dictionary of player and sorted distances to chairs, where earlier entry is nearer</returns>
    private static IDictionary<Player, IList<ChairDistance>> CalculateChairDistances(IEnumerable<Chair> chairs,
        IEnumerable<Player> players)
    {
        var distances = new ConcurrentDictionary<Player, IList<ChairDistance>>();

        Parallel.ForEach(players, player =>
        {
            distances[player] = chairs
                .Select(chair => new ChairDistance(
                    chair,
                    Math.Abs(player.At.Row - chair.At.Row) +
                    Math.Abs(player.At.Column - chair.At.Column)
                ))
                .OrderBy(cd => cd.Distance)
                .ToList();
        });

        return distances;
    }

    /// <summary>
    /// Parses the grid from the input.
    /// </summary>
    /// <param name="input">Input to parse grid from</param>
    /// <returns>Chairs and players</returns>
    private static (IReadOnlyCollection<Chair>, IReadOnlyCollection<Player>) ParseGrid(Input input)
    {
        var chairs = new ConcurrentBag<Chair>();
        var players = new ConcurrentBag<Player>();

        Parallel.ForEach(
            Enumerable.Range(0, (int)input.Grid.LongLength),
            index =>
            {
                var row = index / input.Columns;
                var column = index % input.Columns;

                switch (input.Grid[row, column])
                {
                    case 'L':
                        chairs.Add(new Chair(
                            new Coordinate(
                                row,
                                column
                            )
                        ));

                        break;

                    case 'X':
                        players.Add(new Player(
                            new Coordinate(
                                row,
                                column
                            )
                        ));

                        break;
                }
            }
        );

        return (chairs, players);
    }

    /// <summary>
    /// Lock object.
    /// </summary>
    private static readonly object Lock = new();

    /// <summary>
    /// Runs the task solution.
    /// </summary>
    /// <param name="input">Input</param>
    /// <returns>Output</returns>
    public static Output Run(Input input)
    {
        var (chairs, players) = ParseGrid(input);
        var distances = CalculateChairDistances(chairs, players);

        return new Output
        {
            ChairsBroken = GrabChairs(distances)
                .OrderBy(c => c.At.Row)
                .ThenBy(c => c.At.Column)
                .ToList()
        };
    }

    /// <summary>
    /// Grabs chairs based on distances between players and chairs.
    /// </summary>
    /// <param name="distances">Distances between player and chairs</param>
    /// <returns>Broken chairs that had multiple players grab them</returns>
    private static IList<Chair> GrabChairs(IDictionary<Player, IList<ChairDistance>> distances)
    {
        var broken = new List<Chair>();

        Parallel.ForEach(distances.Keys, player =>
        {
            lock (Lock)
            {
                if (player.IsOut)
                {
                    return;
                }

                var playersOther = distances.Keys
                    .Where(p => p != player)
                    .ToList();

                foreach (var (chair, distance) in distances[player])
                {
                    if (chair.IsOccupied || broken.Contains(chair))
                    {
                        continue;
                    }

                    // If other player is closer to the chair, skip
                    if (
                        playersOther
                        .Where(p => !p.IsOut)
                        .Any(p =>
                        {
                            var candidate = distances[p].FirstOrDefault(dp =>
                                !broken.Contains(dp.Chair) &&
                                !dp.Chair.IsOccupied
                            );

                            return candidate != null &&
                                   candidate.Chair.Equals(chair) &&
                                   candidate.Distance < distance &&
                                   !p.IsOut;
                        })
                    )
                    {
                        continue;
                    }

                    // If same distance between players to the chair, break chair and kick players out
                    var playersRunning = playersOther
                        .Where(p =>
                            !p.IsOut &&
                            distances[p].First(cd => cd.Chair.Equals(chair)).Distance == distance
                        )
                        .ToList();

                    Thread.Sleep(Random.Shared.Next(100, 500));


                    if (playersRunning.Count > 0)
                    {
                        broken.Add(chair);

                        playersRunning.ForEach(p => p.IsOut = true);

                        player.IsOut = true;

                        break;
                    }

                    Thread.Yield();

                    // Well, we can just occupy the chair
                    chair.IsOccupied = true;
                    player.IsOut = true;

                    break;
                }
            }
        });

        return broken;
    }
}