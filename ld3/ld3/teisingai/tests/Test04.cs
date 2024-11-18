namespace Tests;

using NUnit.Framework;

using Org.Ktu.T120B197.DBs;
using System.Diagnostics;


/// <summary>
/// Unit test for the fourth set of queries.
/// </summary>
[TestFixture]
[NonParallelizable]
public class Test04
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public Test04()
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
    /// Query to fetch Comments, Examples, and Categories, filtering by Category name
    /// </summary>
    [Test]
    [NonParallelizable]
    public void TestCommentsQueryWithExampleAndCategory()
    {
        using (var db = new DB())
        {
            var categoryName = "Sedan"; // example category filter

            // Query to fetch Comments, Examples, and Categories, filtering by Category name
            var query = from comment in db.Comment
                        join example in db.Example on comment.exampleId equals example.id
                        join category in db.Category on example.Category.id equals category.id
                        where category.name == categoryName
                        select new
                        {
                            CommentText = comment.content,
                            ExampleText = example.text,
                            CategoryName = category.name
                        };

            // Execute the query and assert the results
            var results = query.ToList();

            // Ensure that results are not empty
            Assert.That(results, Is.Not.Empty, "No comments found for the given category name.");

            // Additional assertions can be added to check the correctness of the category and example
            Assert.That(results.First().CategoryName, Is.EqualTo(categoryName), "Category name does not match the filter.");
        }
    }
}