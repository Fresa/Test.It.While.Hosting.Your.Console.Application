using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Console.Application.Consoles
{
    public interface IHostController : IConsole, IConsoleClient
    {
        void Disconnect(int exitCode);

        Task RaiseExceptionAsync(
            Exception exception,
            CancellationToken cancellationToken = default);
    }
}