using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ICollection<SearchResult>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            var url = $"https://www.yandex.ru/search/?text={query}";
            var web = new HtmlWeb();
            var document = await web.LoadFromWebAsync(url, cancellationToken);

            return document.DocumentNode
                .SelectNodes("//a[@href]")
                .Where(i => i.Attributes.Contains("tabindex") && i.Attributes["tabindex"].Value == "2")
                .Select(i => i.Attributes["href"].Value)
                .Select(i => new SearchResult(i, string.Empty))
                .ToList();
        }
    }
}
