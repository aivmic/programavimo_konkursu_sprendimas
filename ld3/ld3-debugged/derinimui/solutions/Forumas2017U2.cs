using static Solutions.Forumas2017U2.Output;
using static Solutions.Forumas2017U2.Output.WordResult;

namespace Solutions;

/// <summary>
/// Solution for 2017 Forumas.
/// </summary>
public class Forumas2017U2
{
    /// <summary>
    /// A single input.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Count of <see cref="Words"/>.
        /// </summary>
        public int Count { get; init; }

        /// <summary>
        /// Initial word to compare against.
        /// </summary>
        public string WordInitial { get; init; }

        /// <summary>
        /// Words to compare against <see cref="WordInitial"/>.
        /// </summary>
        public IEnumerable<string> Words { get; init; }
    }

    /// <summary>
    /// A single output.
    /// </summary>
    public class Output
    {
        /// <summary>
        /// Results of the comparison between <see cref="Input.WordInitial"/> and <see cref="Input.Words"/>.
        /// </summary>
        public IEnumerable<WordResult> Results { get; init; }

        /// <summary>
        /// Result of the comparison between <see cref="Input.WordInitial"/> and a single word.
        /// </summary>
        /// <param name="Word">Word that was compared against <see cref="Input.WordInitial"/></param>
        public record WordResult(string Word)
        {
            /// <summary>
            /// Successful comparison result.
            /// </summary>
            /// <param name="Word">Word that was compared against <see cref="Input.WordInitial"/></param>
            public record Success(string Word) : WordResult(Word)
            {
                public override string ToString() => $"{Word}: tinkamas";
            }

            /// <summary>
            /// Failed comparison result.
            /// </summary>
            /// <param name="Word">Word that was compared against <see cref="Input.WordInitial"/></param>
            /// <param name="Reason">Reason of failure</param>
            public record Failure(string Word, Failure.Type Reason) : WordResult(Word)
            {
                /// <summary>
                /// Reason of failure.
                /// </summary>
                /// <param name="Letter">Letter that caused the failure</param>
                public record Type(char Letter)
                {
                    /// <summary>
                    /// Too many letters of a single type.
                    /// </summary>
                    /// <param name="Letter">Cause letter</param>
                    public record LetterExceeded(char Letter) : Type(Letter)
                    {
                        public override string ToString() => $"raidziu {Letter} per daug";
                    }

                    /// <summary>
                    /// Missing letter.
                    /// </summary>
                    /// <param name="Letter">Cause letter</param>
                    public record LetterMissing(char Letter) : Type(Letter)
                    {
                        public override string ToString() => $"nebuvo raides {Letter}";
                    }
                }

                public override string ToString() => $"{Word}: netinkamas ({Reason})";
            }
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
                Count = 3,
                WordInitial = "PROGRAMUOTOJAS",
                Words =
                [
                    "PROGRAMA",
                    "PROGRAMUOTI",
                    "SAGOS"
                ]
            },
            new Input
            {
                Count = 4,
                WordInitial = "KEBABAS",
                Words =
                [
                    "BABA",
                    "KAS",
                    "KASA",
                    "RAKSTIS"
                ]
            },
            new Input
            {
                Count = 7,
                WordInitial = "KALNAS",
                Words =
                [
                    "KALNAS",
                    "KALNIS",
                    "KALNOS",
                    "LASA",
                    "ALNAS",
                    "KAS",
                    "JEZUS"
                ]
            }
        ];

        public static Output[] Outputs { get; } =
        [
            new Output
            {
                Results =
                [
                    new Success("PROGRAMA"),
                    new Failure(
                        "PROGRAMUOTI",
                        new Failure.Type.LetterMissing('I')
                    ),
                    new Failure(
                        "SAGOS",
                        new Failure.Type.LetterExceeded('S')
                    )
                ]
            },
            new Output
            {
                Results =
                [
                    new Success("BABA"),
                    new Success("KAS"),
                    new Success("KASA"),
                    new Failure(
                        "RAKSTIS",
                        new Failure.Type.LetterMissing('I')
                    )
                ]
            },
            new Output
            {
                Results =
                [
                    new Success("KALNAS"),
                    new Failure(
                        "KALNIS",
                        new Failure.Type.LetterMissing('I')
                    ),
                    new Failure(
                        "KALNOS",
                        new Failure.Type.LetterMissing('O')
                    ),
                    new Success("LASA"),
                    new Success("ALNAS"),
                    new Success("KAS"),
                    new Failure(
                        "JEZUS",
                        new Failure.Type.LetterMissing('E')
                    )
                ]
            }
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
        var characters = input.WordInitial.ToCharCounts();
        var words = new LinkedList<string>(input.Words);
        var results = new HashSet<WordResult>();

        var tasks = Enumerable
            .Range(0, Program.Threads)
            .AsParallel()
            .Select(_ => Task.Run(() =>
            {
                while (words.Count > 0)
                {
                    lock (Lock)
                    {
                        if (words.Count == 0)
                        {
                            break;
                        }

                        Thread.Sleep(Random.Shared.Next(100, 500));

                        var candidate = words.FirstOrDefault();

                        words.Remove(candidate);

                        var candidateCharacters = candidate
                            .ToCharCounts()
                            .OrderBy(entry => entry.Key)
                            .ToList();

                        // 1. Missing letters in initial map
                        var missing = candidateCharacters
                            .FirstOrDefault(entry => !characters.ContainsKey(entry.Key))
                            .To(entry => new
                            {
                                Letter = entry.Key,
                                Result = new Failure(
                                    candidate,
                                    new Failure.Type.LetterMissing(entry.Key)
                                ) as WordResult
                            });

                        // 2. More letters in candidate than in initial map
                        var more = candidateCharacters
                            .FirstOrDefault(entry =>
                                characters.ContainsKey(entry.Key) &&
                                entry.Value > characters[entry.Key]
                            )
                            .To(entry => new
                            {
                                Letter = entry.Key,
                                Result = new Failure(
                                    candidate,
                                    new Failure.Type.LetterExceeded(entry.Key)
                                ) as WordResult
                            });

                        Thread.Yield();

                        results.Add(
                            missing != default && more != default
                                ? missing.Letter > more.Letter
                                    ? more.Result
                                    : missing.Result
                                : missing != default
                                    ? missing.Result
                                    : (more != default)
                                        ? more.Result
                                        : new Success(candidate)
                        );
                    }
                }
            }))
            .ToArray();

        Task.WaitAll(tasks);

        return new Output
        {
            Results = results
        };
    }
}