using System;
using System.Threading;
using System.Threading.Tasks;

namespace Listen.Models.Tasks
{
    public interface ILongRunningTask
    {
        Task Run(CancellationToken token = default(CancellationToken));
    }
}
