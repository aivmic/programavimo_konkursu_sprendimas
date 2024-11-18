namespace Tests;

using NUnit.Framework;

using Org.Ktu.T120B197.DBs;
using System.Diagnostics;


/// <summary>
/// Unit test for the first set of queries.
/// </summary>
[TestFixture]
[NonParallelizable]
public class Test01
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public Test01()
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
    /// Query to fetch Examples, Authors, and Categories, filtering by category name.
    /// </summary>
    [Test]
    [NonParallelizable]
    public void TestExampleQueryByAuthorAndCategory()
    {
        using (var db = new DB())
        {
            var categoryName = "Sedan"; // example category name filter

            // Query to fetch Examples, Authors, and Categories, filtering by category name.
            var query = from example in db.Example
                        join author in db.Author on example.Author.id equals author.id
                        join category in db.Category on example.Category.id equals category.id
                        where category.name == category.name
                        select new
                        {
                            ExampleId = example.id,
                            ExampleText = example.text,
                            AuthorName = author.name,
                            CategoryName = category.name
                        };

            // Execute the query and assert the results
            var results = query.ToList();


            // Ensure that results are not empty

            Assert.That(results, Is.Not.Empty, "No results found for the given category.");

            // Additional assertions can be added to check that results match expected criteria
            Assert.That(results.First().CategoryName, Is.EqualTo(categoryName), "Category name does not match the filter.");
        }
    }
}