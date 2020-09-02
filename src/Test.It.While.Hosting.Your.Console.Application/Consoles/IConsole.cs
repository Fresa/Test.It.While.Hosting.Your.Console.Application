using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Console.Application.Consoles
{
    public interface IConsole
    {
        void WriteLine(
            string message);

        ValueTask<string> ReadAsync(
            TimeSpan timeout = default,
            CancellationToken cancellationToken = default);
    }
}