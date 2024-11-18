namespace Tests;

using NUnit.Framework;
using System.Diagnostics;


/// <summary>
/// Unit tests for Metai Regionas.
/// </summary>

[TestFixture]
[NonParallelizable]
public class AZ01201701U2Test
{
    [Test]
    [NonParallelizable]
    public void TestPairs()
    {
        // Take inputs and outputs for the solution
        var inputs = Solutions.AZ01201701U2.TestData.Inputs;
        var outputs = Solutions.AZ01201701U2.TestData.Outputs;

        // Pair inputs with outputs
        var inouts =
            inputs
                .Zip(outputs, (inp, outp) => new { inp = inp, outp = outp })
                .ToList();

        // Test each pair
        inouts
            .Select((inout, index) => new { inout = inout, index = index }).ToList()
            .ForEach(it =>
            {
                var input = it.inout.inp;
                var expectedOutput = it.inout.outp;
                var index = it.index;

                var task = new Solutions.AZ01201701U2();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var resultTask = Task.Run(() => task.Run(input));

                if (resultTask.Wait(1000))
                {
                    stopwatch.Stop();

                    var output = task.Run(input);

                    Assert.That(output.Results, Is.EqualTo(expectedOutput.Results),
                        $"Actual output is not matching expected output at test data index {index}.");
                }
                else
                {
                    Assert.Fail($"Task did not complete in 10000 ms at test data index {index}.");
                }
            });
    }

}