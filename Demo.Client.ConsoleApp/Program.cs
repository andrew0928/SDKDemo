using Demo.Contracts;
using Demo.SDK;
using System;
using System.Diagnostics;
using System.Linq;

namespace Demo.Client.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            
            // 方法3: 使用 SDK
            ListAll_UseSDK();
            
            Console.WriteLine($"* Total Time: {timer.ElapsedMilliseconds} msec.");
        }
        
        static void ListAll_UseSDK()
        {
            Demo.SDK.Client client = new Demo.SDK.Client(new Uri("http://localhost:56648"));
            //ISDKClient client = Demo.SDK.Client.Create(new Uri("http://localhost:56648"));
            foreach (var item in (from x in client.GetBirdInfos() where x.BirdNo == "40250" select x))
            {
                ShowBirdInfo(item);
                break;
            }
        }

        static void ShowBirdInfo(BirdInfo birdinfo)
        {
            Console.WriteLine($"[ID: {birdinfo.BirdId}] -------------------------------------------------------------");
            Console.WriteLine($"        流水號: {birdinfo.BirdNo}");
            Console.WriteLine($"      調查日期: {birdinfo.SurveyDate}");
            Console.WriteLine($"      調查地點: {birdinfo.Location}");
            Console.WriteLine($"     經度/緯度: {birdinfo.WGS84Lon}/{birdinfo.WGS84Lat}");
            Console.WriteLine($"          科名: {birdinfo.FamilyName}");
            Console.WriteLine($"          學名: {birdinfo.ScienceName}");
            Console.WriteLine($"中研院學名代碼: {birdinfo.TaiBNETCode}");
            Console.WriteLine($"        鳥中名: {birdinfo.CommonName}");
            Console.WriteLine($"          數量: {birdinfo.Quantity}");
            Console.WriteLine($"      鳥名代碼: {birdinfo.BirdId}");
            Console.WriteLine($"      調查站碼: {birdinfo.SiteId}");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}