using Demo.SDK.Contracts;
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

        public static BirdInfo Get(string id)
        {
            if (string.IsNullOrEmpty(id) || !_DataIndex.ContainsKey(id))
            {
                return null;
            }
            return _DataIndex[id];
        }

        public static void Init(string jsonText)
        {
            Data = JsonConvert.DeserializeObject<BirdInfo[]>(jsonText);
            _DataIndex = new Dictionary<string, BirdInfo>();
            foreach(BirdInfo bi in Data)
            {
                _DataIndex[bi.SerialNo] = bi;
            }
        }
        #endregion
    }


}