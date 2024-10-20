namespace Tests;

using NUnit;
using NUnit.Framework;

using Solutions;


/// <summary>
/// Unit tests for Metai Regionas.
/// </summary>
[TestFixture]
public class Dzukija2012U2Test
{
	[Test]
	public void TestSomething()
	{
		//take inputs and outputs for the solution
		var inputs = Dzukija2012U2.TestData.Inputs;
		var outputs = Dzukija2012U2.TestData.Outputs;

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

                var task = new Dzukija2012U2();
                var output = task.Run(input);

				for (int i = 0; i < expectedOutput.ats.Length; i++)
				{
					Assert.That(output.ats[i] == expectedOutput.ats[i],
						$"Wrong answer");
				}
                

            });
	}
}