using System.Threading;
using System.Threading.Tasks;

namespace Everest.Engineering.ConsoleApp.Executor.Abstractions
{
    public interface IExecutor
    {
        Task CancelOperationAsync(CancellationToken stoppingToken);
        Task RunOperationsAsync(CancellationToken stoppingToken);
    }
}
