namespace Tests;

using NUnit.Framework;

using Org.Ktu.T120B197.DBs;
using System.Diagnostics;


/// <summary>
/// Unit test for the fifth set of queries.
/// </summary>
[TestFixture]
[NonParallelizable]
public class Test05
{

    /// <summary>
    /// Constructor.
    /// </summary>
    public Test05()
    {
        //set DB logger
        DB.Logger =
            (msg) =>
            {
                //this will show in console when running with "dotnet test -v n"
                TestContext.Out.WriteLine(msg);

                //this will show in debug console when running test via debugger
                Debug.WriteLine(msg);
            };
    }

    /// <summary>
    /// Test for query/queries.
    /// Query to fetch examples, tag, and author by tag
    /// </summary>
    [Test]
    [NonParallelizable]
    public void TestExamplesQueryWithAuthorAndTag()
    {
        using (var db = new DB())
        {
            // Query to fetch examples, tag, and author by tag
            var tagName = "Minivan"; // Replace with the actual tag name you want to filter by
            var query = from example in db.Example
                        from tag in example.Tags
                        where tag.name == tagName
                        select new
                        {
                            Example = example,
                            Tag = tag,
                            Author = example.Author
                        };

            var results = query.ToList();

            // Add assertions to test the results
            Assert.That(results, Is.Not.Null);
            Assert.That(results, Is.Not.Empty);
            foreach (var result in results)
            {
                Assert.That(result.Tag.name, Is.EqualTo(tagName));
                Assert.That(result.Example, Is.Not.Null);
                Assert.That(result.Author, Is.Not.Null);
            }
        }
    }
}
