using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Contracts
{
    public abstract class ApiContractBase<T> where T : IApiContract
    {
    }


#if Debug

    public interface ISomeContract : IApiContract
    {
        object Get();
    }

    public abstract class ImplementClassBase<T> : ApiContractBase<T>, ISomeContract where T : IApiContract
    {
        public abstract object Get();
    }

    /// <summary>
    /// 實作類別
    /// </summary>
    public class ImplementClass : ImplementClassBase<ImplementClass>
    {
        public override object Get()
        {
            return new object();
        }
    }

#endif

}
