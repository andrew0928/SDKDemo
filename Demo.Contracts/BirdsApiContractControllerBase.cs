using System.Collections.Generic;

namespace Demo.Contracts
{
    public abstract class BirdsApiContractControllerBase<T> : ApiContractControllerBase<T>, IBirdsApiContract where T : IApiContract
    {
        public abstract IEnumerable<BirdInfo> Get();
        public abstract BirdInfo Get(string serialNo);
        public abstract void Head();
        public abstract string Options();
    }
}