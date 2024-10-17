namespace Tests
{
    using NUnit.Framework;
    using Solutions;
    using System.Linq;

    /// <summary>
    /// Unit tests for S.Z. 2019.
    /// </summary>
    [TestFixture]
    public class SZ03201903U2Test
    {
        [Test]
        public void TestDwarfFriendship()
        {
            // Retrieve inputs and expected outputs from the solution's test data.
            var inputs = SZ03201903U2.TestData.Inputs;
            var expectedOutputs = SZ03201903U2.TestData.Outputs;

            // Pair inputs with expected outputs for comparison.
            var inputOutputPairs =
                inputs
                    .Zip(expectedOutputs, (input, expectedOutput) => new { Input = input, ExpectedOutput = expectedOutput })
                    .ToList();

            // Test each pair
            inputOutputPairs
                .Select((pair, index) => new { Pair = pair, Index = index })
                .ToList()
                .ForEach(it =>
                {
                    var input = it.Pair.Input;
                    var expectedOutput = it.Pair.ExpectedOutput;
                    var index = it.Index;

                    // Instantiate the solution and run it
                    var task = new SZ03201903U2();
                    var actualOutput = task.Run(input);

                    // Check if the lists of friends match
                    Assert.That(
                        actualOutput.Friends.Count == expectedOutput.Friends.Count,
                        $"Number of friend pairs do not match at test data index {index}."
                    );

                    foreach (var (actual, expected) in actualOutput.Friends.Zip(expectedOutput.Friends, (a, e) => (a, e)))
                    {
                        Assert.That(
                            actual.Name1 == expected.Name1 && actual.Name2 == expected.Name2 && Math.Abs(actual.Distance - expected.Distance) < 0.0001,
                            $"Friend pair mismatch at test data index {index}: Expected ({expected.Name1}, {expected.Name2}, {expected.Distance}), but got ({actual.Name1}, {actual.Name2}, {actual.Distance})."
                        );
                    }

                    // Check if the best friends pair match
                    Assert.That(
                        actualOutput.BestFriends.Name1 == expectedOutput.BestFriends.Name1 && actualOutput.BestFriends.Name2 == expectedOutput.BestFriends.Name2,
                        $"Best friends mismatch at test data index {index}: Expected ({expectedOutput.BestFriends.Name1}, {expectedOutput.BestFriends.Name2}), but got ({actualOutput.BestFriends.Name1}, {actualOutput.BestFriends.Name2})."
                    );
                });
        }
    }
}
