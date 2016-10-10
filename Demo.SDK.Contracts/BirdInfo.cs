using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.SDK.Contracts
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
    }


}