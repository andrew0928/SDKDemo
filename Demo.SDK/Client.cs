using Demo.Contracts;
using Demo.SDK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
        private Version _require_API_version = new Version(10, 0, 0, 0);
        //private Version _actual_API_version = null;

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

            //HttpResponseMessage result = _http.SendAsync(new HttpRequestMessage(
            //    HttpMethod.Options,
            //    $"/api/birds")).Result;
            //this._actual_API_version = new Version(JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result));
            

            //// do API version check
            //if (this._require_API_version.Major != this._actual_API_version.Major) throw new InvalidOperationException();
            //if (this._require_API_version.Minor > this._actual_API_version.Minor) throw new InvalidOperationException();
        }

        

        public IEnumerable<BirdInfo> GetBirdInfos()
        {
            int current = 0;
            int pagesize = 5;

            do
            {
                //Console.WriteLine($"--- loading data... ({current} ~ {current + pagesize}) ---");
                HttpResponseMessage result = _http.GetAsync($"/api/birds?$start={current}&$take={pagesize}").Result;

                var result_objs = JsonConvert.DeserializeObject<BirdInfo[]>(result.Content.ReadAsStringAsync().Result);

                foreach (BirdInfo item in result_objs)
                {
                    yield return item;
                }

                if (result_objs.Length == 0) break;
                if (result_objs.Length < pagesize) break;

                current += pagesize;
            } while (true);

            yield break;
        }

        public BirdInfo GetBirdInfo(string serialNo)
        {
            HttpResponseMessage result = _http.GetAsync($"/api/birds/{serialNo}").Result;
            var result_obj = JsonConvert.DeserializeObject<BirdInfo>(result.Content.ReadAsStringAsync().Result);
            return result_obj;
        }
    }
}
