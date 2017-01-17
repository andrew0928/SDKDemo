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









    /// <summary>
    /// 取得 "特生中心102年繁殖鳥大調查資料集" OpenData 用的 API
    /// </summary>
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

        private const int MaxTake = 10;

        /// <summary>
        /// 按照指定條件，傳回符合的資料列。同時會在 HEADER 內標示總比數、從哪一筆開始、這次總共傳回幾筆
        /// $start: 指定從第幾筆開始回傳
        /// $take:  指定最多傳回幾筆 (上限 10 筆)
        /// </summary>
        /// <returns></returns>
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
        

        /// <summary>
        /// 傳回指定的 SN 資料
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public BirdInfo Get(string id)
        {
            return BirdInfoRepo.Get(id);
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
