namespace Tests;

using NLog;
using NUnit.Framework;
using Solutions;

/// <summary>
/// Unit tests for 2019 Forumas.
/// </summary>
[TestFixture]
public class Forumas2019U2Test
{

	/// <summary>
	/// Test data, where each element is a pair of input and expected output.
	/// </summary>
	private class InOut
	{

		/// <summary>
		/// Input.
		/// </summary>
		public Forumas2019U2.Input Input { get; init; }

		/// <summary>
		/// Expected output.
		/// </summary>
		public Forumas2019U2.Output Output { get; init; }

		/// <summary>
		/// Gets test data as a list of input-output pairs.
		/// </summary>
		/// <returns>Input-output pairs list</returns>
		public static List<InOut> GetInOuts()
		{
			return Forumas2019U2.TestData.Inputs
				.Zip(
					Forumas2019U2.TestData.Outputs,
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
				var output = Forumas2019U2.Run(io.InOut.Input);

				Assert.That(
					io.InOut.Output.ChairsBroken.SequenceEqual(output.ChairsBroken),
					$"Output is not matching expected output at test data #{io.Index}."
				);
			});
	}
}