using Demo.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.SDK.TestConsole
{
    class Program
    {
        static string apiurl = "http://localhost:56648";


        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                apiurl = args[0];
            }

            Stopwatch timer = new Stopwatch();

            timer.Restart();
            TestSingleQuery();
            Console.WriteLine($"Test case execute time: {timer.ElapsedMilliseconds} msec.");

            timer.Restart();
            TestLinqQuery();
            Console.WriteLine($"Test case execute time: {timer.ElapsedMilliseconds} msec.");

            timer.Restart();
            TestLinqQuery();
            Console.WriteLine($"Test case execute time: {timer.ElapsedMilliseconds} msec.");
        }



        static void TestLinqQuery()
        {
            ISDKClient client = Client.Create(new Uri(apiurl));

            string[] results = (from x in client.GetBirdInfos() where x.Location == "中橫沿線，塔塔加" orderby x.BirdNo ascending select x.BirdNo).ToArray<string>();


            Assert.AreEqual<int>(results.Length, 85);
            Assert.AreEqual<string>(results[0], "39739");
            Assert.AreEqual<string>(results[5], "39744");
            Assert.AreEqual<string>(results[35], "39774");
            Assert.AreEqual<string>(results[84], "39823");


/*
39739
39740
39741
39742
39743
39744
39745
39746
39747
39748
39749
39750
39751
39752
39753
39754
39755
39756
39757
39758
39759
39760
39761
39762
39763
39764
39765
39766
39767
39768
39769
39770
39771
39772
39773
39774
39775
39776
39777
39778
39779
39780
39781
39782
39783
39784
39785
39786
39787
39788
39789
39790
39791
39792
39793
39794
39795
39796
39797
39798
39799
39800
39801
39802
39803
39804
39805
39806
39807
39808
39809
39810
39811
39812
39813
39814
39815
39816
39817
39818
39819
39820
39821
39822
39823
*/
        }


        static void TestSingleQuery()
        {
            ISDKClient client = Client.Create(new Uri(apiurl));
            BirdInfo result = null;


            // test-1
            result = client.GetBirdInfo("40298");
            Assert.NotNull(result);
            Assert.AreEqual<string>(result.BirdId, "B0364");
            Assert.AreEqual<string>(result.BirdNo, "40298");
            Assert.AreEqual<string>(result.CommonName, "灰頭花翼");

            // test-2
            result = client.GetBirdInfo("40294");
            Assert.NotNull(result);
            Assert.AreEqual<string>(result.BirdId, "B0286");
            Assert.AreEqual<string>(result.BirdNo, "40294");
            Assert.AreEqual<string>(result.CommonName, "中杜鵑");

            // test-3
            result = client.GetBirdInfo("39350");
            Assert.NotNull(result);
            Assert.AreEqual<string>(result.BirdId, "B0382");
            Assert.AreEqual<string>(result.BirdNo, "39350");
            Assert.AreEqual<string>(result.CommonName, "烏頭翁");
        }
    }

    class Assert
    {
        public static void AreEqual<T>(T actual, T expected)
        {
            if (actual == null && expected != null) throw new Exception();
            if (actual != null && actual.Equals(expected) == false) throw new Exception();

            Console.WriteLine("PASS!");
        }

        public static void NotNull(object actual)
        {
            if (actual == null) throw new Exception();

            Console.WriteLine("PASS!");
        }
    }
}
