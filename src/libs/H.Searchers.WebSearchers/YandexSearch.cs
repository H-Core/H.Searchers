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
    public class YandexSearcher : Module, ISearcher
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
                .Where(i => i.Attributes.Contains("tabindex") && i.Attributes["tabindex"].Value == "2")
                .Select(i => i.Attributes["href"].Value)
                .Select(i => new SearchResult(i, string.Empty))
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
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://www.yandex.ru/search/?text={query}&lr=76"))
            {
                Headers =
                {
                    { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36" },
                    { "device-memory", "8" },
                    { "downlink", "3.4" },
                    { "dpr", "2.25" },
                    { "ect", "4g" },
                    { "rtt", "100" },
                    { "viewport-width", "1138" },
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
