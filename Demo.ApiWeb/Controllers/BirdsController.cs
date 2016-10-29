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
    /// 識別是否為 Contract 用途的 interface
    /// </summary>
    interface IContract
    {
    }

    /// <summary>
    /// Birds API Controller Contract
    /// </summary>
    interface IBirdsContract : IContract
    {
        string Options();

        void Head();

        IEnumerable<BirdInfo> Get();

        BirdInfo Get(string serialNo);
    }



    public class SDKVersionCheckActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // step 1, get SDK required version info from HTTP request header: X-SDK-REQUIRED-VERSION
            Version required_version = null;
            foreach(string hvalue in actionContext.Request.Headers.GetValues("X-SDK-REQUIRED-VERSION"))
            {
                required_version = new Version(hvalue);
                break;
            }

            // step 2, get current API version from Assembly metadata
            Version current_version = this.GetType().Assembly.GetName().Version;

            // step 3, check compatibility
            Debug.WriteLine($"check SDK version:");
            Debug.WriteLine($"- required: {required_version}");
            Debug.WriteLine($"- current:  {current_version}");

            if (current_version.Major != required_version.Major) throw new InvalidOperationException();
            if (current_version.Minor < required_version.Minor) throw new InvalidOperationException();
        }
    }


    public class ContractCheckActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///  檢查即將要被執行的 Action, 是否已被定義在 contract interface ?
        ///  若找不到 contract interface, 或是找不到 action name 對應的 method, 則丟出 NotSupportedException
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Debug.WriteLine(
                "check contract for API: {0}/{1}",
                actionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                actionContext.ActionDescriptor.ActionName);


            //
            // 搜尋 controller 實作的 contract interface
            //
            System.Type contractType = null;
            foreach (var i in actionContext.ActionDescriptor.ControllerDescriptor.ControllerType.GetInterfaces())
            {
                if (i.GetInterface(typeof(IContract).FullName) != null)
                {
                    contractType = i;
                    Debug.WriteLine($"- contract interface found: {contractType.FullName}.");
                    break;
                }
            }
            if (contractType == null) throw new NotSupportedException("API method(s) must defined in contract interface.");

            //
            // 搜尋 action method
            //
            bool isMatch = false;
            foreach(var m in contractType.GetMethods())
            {
                if (m.Name == actionContext.ActionDescriptor.ActionName)
                {
                    isMatch = true;
                    Debug.WriteLine($"- contract method found: {m.Name}.");
                    break;
                }
            }

            if (isMatch == false) throw new NotSupportedException("API method(s) must defined in contract interface.");
        }
    }


    [SDKVersionCheckActionFilter]
    [ContractCheckActionFilter]
    public class BirdsController : ApiController, IBirdsContract
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
