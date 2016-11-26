using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Demo.ApiWeb.Controllers
{
    public class SDKVersionCheckActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // step 1, get SDK required version info from HTTP request header: X-SDK-REQUIRED-VERSION
            Version required_version = null;
            try
            {
                foreach (string hvalue in actionContext.Request.Headers.GetValues("X-SDK-REQUIRED-VERSION"))
                {
                    required_version = new Version(hvalue);
                    break;
                }
            }
            catch
            {
                // client 沒指定版本，就直接略過檢查
                return;
            }

            // step 2, get current API version from Assembly metadata
            Version current_version = this.GetType().Assembly.GetName().Version;

            // step 3, check compatibility
            Debug.WriteLine($"check SDK version:");
            Debug.WriteLine($"- required: {required_version}");
            Debug.WriteLine($"- current:  {current_version}");

            if (current_version.Major != required_version.Major) throw new InvalidOperationException();
            if (current_version.Minor < required_version.Minor) throw new InvalidOperationException();

            return;
        }
    }
}