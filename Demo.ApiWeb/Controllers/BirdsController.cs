using Demo.ApiWeb.Models;
using Demo.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Demo.ApiWeb.Controllers
{
    [SDKVersionCheckActionFilter]
    [ContractCheckActionFilter]
    public class BirdsController : ApiController, IBirdsApiContract
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            BirdInfoRepo.Init(File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/birds.json")));
            this.APIVersion = this.GetType().Assembly.GetName().Version;
            base.Initialize(controllerContext);
        }

        private Version APIVersion { get; set; }

        public string Options()
        {
            return this.APIVersion.ToString();
        }

        public void Head()
        {
            System.Web.HttpContext.Current.Response.AddHeader("X-DATAINFO-TOTAL", BirdInfoRepo.Data.Count().ToString());
            return;
        }

        private const int MaxTake = 10;

        public IEnumerable<BirdInfo> Get()
        {
            int start, take;
            if (int.TryParse(this.GetQueryString("$start"), out start) == false) start = 0;
            if (int.TryParse(this.GetQueryString("$take"), out take) == false) take = MaxTake;

            if (take > MaxTake) take = MaxTake;

            System.Web.HttpContext.Current.Response.AddHeader("X-DATAINFO-TOTAL", BirdInfoRepo.Data.Count().ToString());
            System.Web.HttpContext.Current.Response.AddHeader("X-DATAINFO-START", start.ToString());
            System.Web.HttpContext.Current.Response.AddHeader("X-DATAINFO-TAKE", take.ToString());

            IEnumerable<BirdInfo> result = BirdInfoRepo.Data;
            if (start > 0) result = result.Skip(start);
            result = result.Take(take);

            return result;
        }
        
        // GET api/values/5
        public BirdInfo Get(string serialNo)
        {
            return BirdInfoRepo.Get(serialNo);
        }
        

        private string GetQueryString(string name)
        {
            foreach(var pair in this.Request.GetQueryNameValuePairs())
            {
                if (pair.Key == name) return pair.Value;
            }

            return null;
        }

        
    }
}
