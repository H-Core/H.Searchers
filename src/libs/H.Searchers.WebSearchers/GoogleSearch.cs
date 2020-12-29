using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using H.Core;
using H.Core.Searchers;
using HtmlAgilityPack;

namespace H.Searchers
{
    /// <summary>
    /// 
    /// </summary>
    public class GoogleSearcher : Module, ISearcher
    {
        #region Properties

        private HttpClient HttpClient { get; } = new();

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ICollection<SearchResult>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            var html = await GetHtmlAsync(query, cancellationToken).ConfigureAwait(false);

            var document = new HtmlDocument();
            document.LoadHtml(html);

            return document.DocumentNode
                .SelectNodes("//a[@href]")
                .Where(node => node.Attributes.Contains("ping") &&
                               node.Attributes.Contains("data-ved"))
                .Select(node => new SearchResult(
                    node.Attributes["href"].Value, 
                    node.Descendants("h3").FirstOrDefault()?.InnerText ?? string.Empty))
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GetHtmlAsync(string query, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://www.google.com/search?q={query}&oq={query}&sourceid=chrome&ie=UTF-8"))
            {
                Headers =
                {
                    { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36" },
                }
            };
            using var response = await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            HttpClient.Dispose();
        }

        #endregion
    }
}
