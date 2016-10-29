using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Contracts
{
    public interface IBirdsApiContract : IApiContract
    {
        string Options();

        void Head();

        IEnumerable<BirdInfo> Get();

        BirdInfo Get(string serialNo);
    }
}
