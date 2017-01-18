using Demo.Contracts;
using Demo.SDK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Demo.SDK
{
    public class Client : ISDKClient
    {
        private HttpClient _http = null;

        /// <summary>
        /// 指定這個版本的 SDK，需要對應 API 的最低版本號碼
        /// </summary>
        private Version _require_API_version = new Version(10, 1, 0, 0);
        private Version _actual_API_version = null;

        public static ISDKClient Create(Uri serviceURL)
        {
            return new Client(serviceURL);
        }

        private Client(Uri serviceURL)
        {
            // do init / check
            this._http = new HttpClient();
            this._http.BaseAddress = serviceURL;
            this._http.DefaultRequestHeaders.Add("X-SDK-REQUIRED-VERSION", this._require_API_version.ToString());

            HttpResponseMessage result = _http.SendAsync(new HttpRequestMessage(
                HttpMethod.Options,
                $"/api/birds2")).Result;
            this._actual_API_version = new Version(JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result));

            // do API version check
            if (this._require_API_version.Major != this._actual_API_version.Major) throw new InvalidOperationException();
            if (this._require_API_version.Minor > this._actual_API_version.Minor) throw new InvalidOperationException();
        }
      

        public IEnumerable<BirdInfo> GetBirdInfos()
        {
            HttpResponseMessage result = _http.GetAsync($"/api/birds2").Result;
            string[] idlist = JsonConvert.DeserializeObject<string[]>(result.Content.ReadAsStringAsync().Result);

            if (idlist != null)
            {
                foreach (string id in idlist)
                {
                    yield return this.GetBirdInfo(id);
                }
            }
            yield break;
        }

        public BirdInfo GetBirdInfo(string serialNo)
        {
            // bug here
            string cachekey = $"cache://birds/{serialNo}";

            if (this._cache.Contains(cachekey))
            {
                return this._cache.Get(cachekey) as BirdInfo;
            }
            else
            {
                HttpResponseMessage result = _http.GetAsync($"/api/birds2/{serialNo}").Result;
                var result_obj = JsonConvert.DeserializeObject<BirdInfo>(result.Content.ReadAsStringAsync().Result);

                this._cache.Set(cachekey, result_obj, DateTimeOffset.Now.AddMinutes(10));

                return result_obj;
            }
        }

        private ObjectCache _cache = MemoryCache.Default;
    }
}
