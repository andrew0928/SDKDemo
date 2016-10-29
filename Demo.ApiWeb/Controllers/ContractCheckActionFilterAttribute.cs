using Demo.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Demo.ApiWeb.Controllers
{
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
                if (i.GetInterface(typeof(IApiContract).FullName) != null)
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
            foreach (var m in contractType.GetMethods())
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

}