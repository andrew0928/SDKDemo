using Demo.ApiWeb.Models;
using Demo.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Demo.ApiWeb.Controllers
{
    [SDKVersionCheckActionFilter]
    public class Birds2Controller : ApiController
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            BirdInfoRepo.Init(File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/birds.json")));
            this.APIVersion = this.GetType().Assembly.GetName().Version;
            base.Initialize(controllerContext);
        }

        private Version APIVersion { get; set; }

        /// <summary>
        /// OPTION: 傳回 API Version
        /// </summary>
        /// <returns></returns>
        public string Options()
        {
            return this.APIVersion.ToString();
        }

        /// <summary>
        /// HEAD: 不傳回資料，只透過 HEADER 傳回資料總筆數，方便前端 APP 預知總比數，計算進度及分頁頁數。
        /// </summary>
        public void Head()
        {
            System.Web.HttpContext.Current.Response.AddHeader("X-DATAINFO-TOTAL", BirdInfoRepo.Data.Count().ToString());
            return;
        }


        public BirdInfo Get(string id)
        {
            //return (from x in BirdInfoRepo.Data where x.BirdNo == id select x).FirstOrDefault();
            return BirdInfoRepo.Get(id);
        }



        public IEnumerable<string> Get()
        {
            foreach(BirdInfo b in BirdInfoRepo.Data)
            {
                yield return b.BirdNo;
            }
        }

    }
}
