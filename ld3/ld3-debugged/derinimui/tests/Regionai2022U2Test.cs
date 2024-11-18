namespace Tests;

using NUnit.Framework;
using Solutions;

/// <summary>
/// Unit tests for 2022 Regionai.
/// </summary>
[TestFixture]
public class Regionai2022U2Test
{
    /// <summary>
    /// Test data, where each element is a pair of input and expected output.
    /// </summary>
    private class InOut
    {
        /// <summary>
        /// Input.
        /// </summary>
        public Regionai2022U2.Input Input { get; init; }

        /// <summary>
        /// Expected output.
        /// </summary>
        public Regionai2022U2.Output Output { get; init; }

        /// <summary>
        /// Gets test data as a list of input-output pairs.
        /// </summary>
        /// <returns>Input-output pairs list</returns>
        public static List<InOut> GetInOuts()
        {
            return Regionai2022U2.TestData.Inputs
                .Zip(
                    Regionai2022U2.TestData.Outputs,
                    (i, o) => new InOut
                    {
                        Input = i,
                        Output = o
                    }
                )
                .ToList();
        }
    }

    [Test(Description = "Tests whether the outputs match expected outputs.")]
    public void TestOutputsMatch()
    {
        InOut
            .GetInOuts()
            .Select((inout, index) => new
            {
                InOut = inout,
                Index = index
            })
            .ToList()
            .ForEach(io =>
            {
                var output = Regionai2022U2.Run(io.InOut.Input);

                Assert.That(
                    output.Playlist.SequenceEqual(io.InOut.Output.Playlist),
                    $"Output is not matching expected output at test data #{io.Index}."
                );
            });
    }
}