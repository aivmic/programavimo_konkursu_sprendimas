namespace Tests;

using NUnit.Framework;

using Org.Ktu.T120B197.DBs;
using System.Diagnostics;


/// <summary>
/// Unit test for the third set of queries.
/// </summary>
[TestFixture]
[NonParallelizable]
public class Test03
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public Test03()
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
    /// Query to fetch Example, Category, and Tag, filtering by Tag name.
    /// </summary>
    [Test]
    [NonParallelizable]
    public void TestExampleQueryWithCategoryAndTag()
    {
        using (var db = new DB())
        {
            var tagName = "Minivan"; // example tag filter

            // Query to fetch Example, Category, and Tag, filtering by Tag name.
            var query = from example in db.Example
                        join category in db.Category on example.Category.id equals category.id
                        join tag in db.Tag on example.id equals tag.id
                        where tag.name == tagName
                        select new
                        {
                            ExampleId = example.id,
                            ExampleText = example.text,
                            CategoryName = category.name,
                            TagName = tag.name
                        };

            // Execute the query and assert the results
            var results = query.ToList();

            // Ensure that results are not empty
            Assert.That(results, Is.Not.Empty, "No results found for the given tag name.");

            // Additional assertions can be added to check the correctness of the category and tag
            Assert.That(results.First().TagName, Is.EqualTo(tagName), "Tag name does not match the filter.");
        }
    }
}
