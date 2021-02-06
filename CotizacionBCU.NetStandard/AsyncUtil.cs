using System;
using System.Threading.Tasks;

namespace CotizacionBCU.NetStandard
{
    public static class AsyncUtil
    {
        public static TResult RunSync<TResult>(Func<Task<TResult>> task) =>

            Task.Factory.StartNew(task)
                        .Unwrap()
                        .GetAwaiter()
                        .GetResult();
    }
}
