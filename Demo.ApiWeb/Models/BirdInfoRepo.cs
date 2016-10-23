using Demo.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.ApiWeb.Models
{
    public class BirdInfoRepo
    {
        #region data repository helper
        public static BirdInfo[] Data { get; private set; }
        private static Dictionary<string, BirdInfo> _DataIndex = null;

        public static BirdInfo Get(string serialNo)
        {
            if (string.IsNullOrEmpty(serialNo) || !_DataIndex.ContainsKey(serialNo))
            {
                return null;
            }
            return _DataIndex[serialNo];
        }

        public static void Init(string jsonText)
        {
            Data = JsonConvert.DeserializeObject<BirdInfo[]>(jsonText);
            _DataIndex = new Dictionary<string, BirdInfo>();
            foreach(BirdInfo bi in Data)
            {
                _DataIndex[bi.BirdNo] = bi;
            }
        }
        #endregion
    }


}