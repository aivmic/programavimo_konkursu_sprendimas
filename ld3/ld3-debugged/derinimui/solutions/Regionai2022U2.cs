namespace Solutions;

/// <summary>
/// Solution for 2022 Regionai.
/// </summary>
public class Regionai2022U2
{
    /// <summary>
    /// A single input.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Number of artists.
        /// </summary>
        public int ArtistsCount { get; init; }

        /// <summary>
        /// Number of songs to be performed by each artist.
        /// </summary>
        public List<int> SongsPerArtist { get; init; }
    }

    /// <summary>
    /// A single output.
    /// </summary>
    public class Output
    {
        /// <summary>
        /// Playlist, where each number represents an artist.
        /// </summary>
        public List<int> Playlist { get; init; }
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
                ArtistsCount = 3,
                SongsPerArtist = [2, 4, 1]
            },
            new Input
            {
                ArtistsCount = 4,
                SongsPerArtist = [1, 5, 1, 4]
            },
            new Input
            {
                ArtistsCount = 2,
                SongsPerArtist = [5, 1]
            },
        ];

        public static Output[] Outputs { get; } =
        [
            new Output
            {
                Playlist = [2, 1, 2, 3, 2, 1, 2]
            },
            new Output
            {
                Playlist = [2, 4, 2, 4, 2, 4, 2, 3, 4, 2, 1]
            },
            new Output
            {
                Playlist = [1, 2, 1, 1, 1, 1]
            },
        ];
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
        var frequencies = input.SongsPerArtist
            .Select((count, index) => (Index: index + 1, Count: count))
            .ToDictionary(
                x => x.Index,
                x => x.Count
            );

        var queue = new PriorityQueue<int, int>(
            frequencies
                .Select(f => (f.Key, f.Value))
                .ToList(),
            // Higher priority for artists with more songs
            Comparer<int>.Create((a, b) => b - a)
        );

        var playlist = new List<int>();
        var previous = (int?)null;

        var tasks = Enumerable
            .Range(0, Program.Threads)
            .AsParallel()
            .Select(_ => Task.Run(() =>
            {
                while (queue.Count > 0)
                {
                    int current, frequency;

                    lock (Lock)
                    {
                        if (queue.Count == 0)
                        {
                            return;
                        }

                        Thread.Sleep(Random.Shared.Next(100, 500));

                        current = queue.Dequeue();
                        frequency = --frequencies[current];

                        if (frequency >= 0)
                        {
                            playlist.Add(current);
                        }

                        if (previous != null)
                        {
                            queue.Enqueue(
                                (int)previous,
                                frequencies[(int)previous]
                            );
                        }
                        else if (frequency > 0 && playlist.Count != 1)
                        {
                            queue.Enqueue(
                                current,
                                frequency
                            );

                            previous = null;

                            continue;
                        }
                    }

                    Thread.Yield();

                    previous = frequency > 0 ? current : null;
                }
            }))
            .ToArray();

        Task.WaitAll(tasks);

        return new Output
        {
            Playlist = playlist
        };
    }
}