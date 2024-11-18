namespace Tests;

using NUnit.Framework;

using Org.Ktu.T120B197.DBs;
using System.Diagnostics;


/// <summary>
/// Unit test for the second set of queries.
/// </summary>
[TestFixture]
[NonParallelizable]
public class Test02
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public Test02()
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
    /// Query to fetch Example, Comments, and Tags, filtering by Example number.
    /// </summary>
    [Test]
    [NonParallelizable]
    public void TestExampleQueryWithCommentAndTag()
    {
        using (var db = new DB())
        {
            var exampleid = 10; // example number filter

            // Query to fetch Example, Comments, and Tags, filtering by Example number.
            var query = from example in db.Example
                        join comment in db.Comment on example.id equals comment.exampleId
                        join tag in db.Tag on example.id equals tag.id
                        where example.id == exampleid
                        select new
                        {
                            ExampleId = example.number,
                            ExampleText = example.text,
                            CommentContent = comment.content,
                            TagName = tag.name
                        };

            // Execute the query and assert the results
            var results = query.ToList();

            // Ensure that results are not empty

            Assert.That(results, Is.Not.Empty, "No results found for the given example id.");

            // Additional assertions can be added to check the correctness of the comments and tags
            Assert.That(results.First().ExampleId, Is.EqualTo(exampleid), "Example id does not match the filter.");
        }
    }
}