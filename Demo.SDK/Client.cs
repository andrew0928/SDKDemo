using Demo.SDK.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Demo.SDK
{
    public class Client
    {
        private HttpClient _http = null;

        public Client(Uri serviceURL)
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
                Console.WriteLine($"--- loading data... ({current} ~ {current + pagesize}) ---");
                HttpResponseMessage result = _http.GetAsync($"/api/birds?$start={current}&$take={pagesize}").Result;

                var result_objs = JsonConvert.DeserializeObject<BirdInfo[]>(result.Content.ReadAsStringAsync().Result);

                foreach (BirdInfo item in result_objs)
                {
                    //Console.WriteLine("ID: {0}", item["BirdId"]);
                    yield return item;
                }

                if (result_objs.Length == 0) break;
                if (result_objs.Length < pagesize) break;

                current += pagesize;
            } while (true);

            yield break;
        }

        public BirdInfo GetBirdInfo(string id)
        {
            throw new NotImplementedException();
        }
    }
}
