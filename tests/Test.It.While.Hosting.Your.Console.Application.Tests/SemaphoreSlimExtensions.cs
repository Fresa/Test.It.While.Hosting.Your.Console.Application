using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Console.Application.Tests
{
    internal static class SemaphoreSlimExtensions
    {
        internal static async Task WaitForSignalsAsync(
            this SemaphoreSlim semaphore,
            int numberOfSignals,
            CancellationToken cancellationToken)
        {
            
            foreach (var _ in Enumerable.Range(0, numberOfSignals))
            {
                await semaphore.WaitAsync(cancellationToken)
                               .ConfigureAwait(false);
            }
        }
    }
}