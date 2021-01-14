using Demo.Contracts;
using System.Collections.Generic;

namespace Demo.ApiWeb.Controllers
{
    public abstract class BirdsApiContractControllerBase<T> : ApiContractControllerBase<T>, IBirdsApiContract where T : IApiContract
    {
        public abstract IEnumerable<BirdInfo> Get();
        public abstract BirdInfo Get(string serialNo);
        public abstract void Head();
        public abstract string Options();
    }
}