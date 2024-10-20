namespace Tests;

using NUnit;
using NUnit.Framework;

using Solutions;


/// <summary>
/// Unit tests for Metai Regionas.
/// </summary>
[TestFixture]
public class Dzukija2011U2Test
{
	[Test]
	public void TestSomething()
	{
		//take inputs and outputs for the solution
		var inputs = Dzukija2011U2.TestData.Inputs;
		var outputs = Dzukija2011U2.TestData.Outputs;

		//pair inputs with outputs
		var inouts = 
			inputs
				.Zip(outputs, (inp, outp) => new { inp = inp, outp = outp})
				.ToList();
		
		//test each pair
		inouts
			.Select((inout, index) => new {inout = inout, index = index}).ToList()
			.ForEach(it => {
                var input = it.inout.inp;
				var expectedOutput = it.inout.outp;
                var index = it.index;

                var task = new Dzukija2011U2();
                var output = task.Run(input);


				Assert.That(output.kk == expectedOutput.kk,
				   $"Wrong answer");
                //Assert.That(output.kk == expectedOutput.kk &&
                //   output.zk == expectedOutput.zk &&
                //   output.dp == expectedOutput.dp &&
                //   output.dn == expectedOutput.dn,
                //   $"Wrong answer");

                for (int i = 0; i < expectedOutput.kopecios.Length; i++)
				{
					Assert.That(output.kopecios[i] == expectedOutput.kopecios[i],
						$"Wrong answer");
                }
                for (int i = 0; i < expectedOutput.zalciai.Length; i++)
                {
                    Assert.That(output.zalciai[i] == expectedOutput.zalciai[i],
                        $"Wrong answer");
                }
            });
	}
}