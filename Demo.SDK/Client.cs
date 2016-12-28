using Demo.Contracts;
using Demo.SDK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Demo.SDK
{
    public class Client : ISDKClient
    {
        private HttpClient _http = null;

        public static ISDKClient Create(Uri serviceURL)
        {
            return new Client(serviceURL);
        }

        private Client(Uri serviceURL)
        {
            // do init / check
            this._http = new HttpClient();
            this._http.BaseAddress = serviceURL;
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
