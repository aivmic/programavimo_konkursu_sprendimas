namespace Tests
{
    using NUnit;
    using NUnit.Framework;
    using Solutions;

    /// <summary>
    /// Unit tests for S.Z.2018.
    /// </summary>
    [TestFixture]
    public class SZ02201802U2Test
    {
        [Test]
        public void TestBasketball()
        {
            // Take inputs and outputs for the solution
            var inputs = SZ02201802U2.TestData.Inputs;
            var outputs = SZ02201802U2.TestData.Outputs;

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

                    var task = new SZ02201802U2();
                    var output = task.Run(input);

                    // Compare the properties of Output
                    Assert.That(output.WinningTeam, Is.EqualTo(expectedOutput.WinningTeam),
                        $"WinningTeam does not match at test data index {index}.");
                    Assert.That(output.TopPlayerNumber, Is.EqualTo(expectedOutput.TopPlayerNumber),
                        $"TopPlayerNumber does not match at test data index {index}.");
                    Assert.That(output.Points, Is.EqualTo(expectedOutput.Points),
                        $"Points do not match at test data index {index}.");
                    Assert.That(output.SuccessfulShots, Is.EqualTo(expectedOutput.SuccessfulShots),
                        $"SuccessfulShots do not match at test data index {index}.");
                    Assert.That(output.MissedShots, Is.EqualTo(expectedOutput.MissedShots),
                        $"MissedShots do not match at test data index {index}.");
                });
        }
    }
}
