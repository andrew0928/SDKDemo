using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Contracts
{
    public interface ISDKClient
    {
        IEnumerable<BirdInfo> GetBirdInfos();
        BirdInfo GetBirdInfo(string serialNo);
    }
}
