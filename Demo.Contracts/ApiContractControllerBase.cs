using Demo.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Demo.Contracts
{
    public abstract class ApiContractControllerBase<T> : ApiController where T : IApiContract
    {
    }
}