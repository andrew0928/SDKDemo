using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.ApiWeb.Models
{
    public class BirdInfo
    {
        public string SerialNo { get; set; }
        public string SurveyDate { get; set; }
        public string Location { get; set; }
        public string WGS84Lon { get; set; }
        public string WGS84Lat { get; set; }
        public string FamilyName { get; set; }
        public string ScienceName { get; set; }
        public string TaiBNETCode { get; set; }
        public string CommonName { get; set; }
        public string Quantity { get; set; }
        public string BirdId { get; set; }
        public string SiteId { get; set; }

        #region data repository helper
        public static BirdInfo[] Data { get; private set; }
        private static Dictionary<string, BirdInfo> _DataIndex = null;

        public static BirdInfo Get(string birdId)
        {
            if (string.IsNullOrEmpty(birdId) || !_DataIndex.ContainsKey(birdId))
            {
                return null;
            }
            return _DataIndex[birdId];
        }

        public static void Init(string jsonText)
        {
            Data = JsonConvert.DeserializeObject<BirdInfo[]>(jsonText);
            _DataIndex = new Dictionary<string, BirdInfo>();
            foreach(BirdInfo bi in Data)
            {
                _DataIndex[bi.BirdId] = bi;
            }
        }
        #endregion
    }


}