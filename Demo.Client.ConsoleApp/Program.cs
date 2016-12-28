using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Client.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // 方法1: 直接用 HttpClient 呼叫 web api
            ListAll_DirectHttpCall();

            // 方法2: 透過 IEnumerable 包裝呼叫的 web api
            //ListAll_UseYield();
            
            Console.WriteLine($"* Total Time: {timer.ElapsedMilliseconds} msec.");
        }

        // Columns:
        // * 流水號(SerialNo)
        // * 調查日期(SurveyDate)
        // * 調查地點(Location)
        // * 經度(WGS84)(WGS84Lon)
        // * 緯度(WGS84)(WGS84Lat)
        // * 科名(FamilyName)
        // * 學名(ScienceName)
        // * 中研院學名代碼(TaiBNETCode)
        // * 鳥中名(CommonName)
        // * 數量(Quantity)
        // * 鳥名代碼(BirdId)
        // * 調查站碼(SiteId)
        static Dictionary<string, string> _columns_name = new Dictionary<string, string>()
        {
            { "SerialNo",       "流水號" },
            { "SurveyDate",     "調查日期" },
            { "Location",       "調查地點" },
            { "WGS84Lon",       "經度" },
            { "WGS84Lat",       "緯度"},
            { "FamilyName",     "科名"},
            { "ScienceName",    "學名" },
            { "TaiBNETCode",    "中研院學名代碼" },
            { "CommonName",     "鳥中名"},
            { "Quantity",       "數量"},
            { "BirdId",         "鳥名代碼" },
            { "SiteId",         "調查站碼"}
        };

        /// <summary>
        /// 一般寫法，直接呼叫 HttpClient 分多次讀取資料分頁
        /// </summary>
        static void ListAll_DirectHttpCall()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:56648");
            
            int current = 0;
            int pagesize = 5;

            do
            {
                Console.WriteLine($"[info] loading data... ({current} ~ {current + pagesize}) ---");
                HttpResponseMessage result = client.GetAsync($"/api/birds?$start={current}&$take={pagesize}").Result;

                var result_objs = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(result.Content.ReadAsStringAsync().Result);


                foreach (var item in result_objs)
                {
                    // filter: 調查地點=玉山排雲山莊
                    if (item["Location"] != "玉山排雲山莊") continue;
                    ShowBirdInfo(item);
                }

                if (result_objs.Length == 0) break;
                if (result_objs.Length < pagesize) break;

                current += pagesize;
            } while (true);
        }

        static void ShowBirdInfo(Dictionary<string, string> birdinfo)
        {
            //Console.WriteLine("[ID: {0}] -------------------------------------------------------------", birdinfo["BirdId"], birdinfo["CommonName"]);
            //foreach (string name in _columns_name.Keys)
            //{
            //    Console.WriteLine(
            //        "{0}: {1}",
            //        _columns_name[name].PadLeft(10, '　'),
            //        birdinfo.ContainsKey(name) ? (birdinfo[name]) : ("<NULL>"));
            //}
            //Console.WriteLine();
            //Console.WriteLine();

            Console.WriteLine("[ID: {0}] {1}", birdinfo["BirdId"], birdinfo["CommonName"]);
        }

        static void ListAll_UseYield()
        {
            // filter: 調查地點=玉山排雲山莊
            foreach (var item in (from x in GetBirdsData() where x["Location"] == "玉山排雲山莊" select x))
            {
                ShowBirdInfo(item);
            }

            // filter: SerialNo = 40250，找到之後離開 for-each loop
            //foreach (var item in (from x in GetBirdsData() where x["SerialNo"] == "40250" select x))
            //{
            //    ShowBirdInfo(item);
            //    break;
            //}
        }

        static IEnumerable<Dictionary<string, string>> GetBirdsData()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:56648");

            int current = 0;
            int pagesize = 5;

            do
            {
                Console.WriteLine($"--- loading data... ({current} ~ {current + pagesize}) ---");
                HttpResponseMessage result = client.GetAsync($"/api/birds?$start={current}&$take={pagesize}").Result;

                var result_objs = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(result.Content.ReadAsStringAsync().Result);

                foreach (var item in result_objs)
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
    }
}
