using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H.Searchers.WebSearchers.IntegrationTests
{
    [TestClass]
    public class SimpleTests
    {
        [TestMethod]
        public async Task SearchTest()
        {
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var cancellationToken = cancellationTokenSource.Token;

            using var searcher = new GoogleSearcher();
            var results = await searcher.SearchAsync("Hello, world!", cancellationToken);

            Console.WriteLine("Results:");
            foreach (var result in results)
            {
                Console.WriteLine($" - {result.Url} {result.Description}");
            }

            Assert.AreNotEqual(0, results.Count, nameof(results.Count));
        }
    }
}
