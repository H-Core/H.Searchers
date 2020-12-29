//using System.Collections.Generic;
//using System.Threading.Tasks;
//using H.Core;

//namespace H.Searchers
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class GoogleSearcher : Module, ISearcher
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="query"></param>
//        /// <returns></returns>
//        public Task<List<string>> Search(string query)
//        {
//            /*
//            using (var service = new CustomsearchService(
//                new BaseClientService.Initializer
//                {
//                    ApiKey = GoogleSearchApiKey
//                }))
//            {
//                var requests = service.Cse.List(query);
//                requests.Cx = GoogleCx;
//                requests.Num = MaxResults;

//                var results = requests.Execute().Items;
//                if (results == null)
//                {
//                    return new List<string>();
//                }

//                return results.Select(i => i.Link).ToList();
//            }
//            */

//            return Task.FromResult(new List<string>());
//        }
//    }
//}
