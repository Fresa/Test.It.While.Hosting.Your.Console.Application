using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Console.Application.Consoles
{
    public class Console : IConsole
    {
        public void WriteLine(string message)
        {
            System.Console.WriteLine(message);
        }

        public async ValueTask<string> ReadAsync(
            TimeSpan timeout = default,
            CancellationToken cancellationToken = default)
        {
            return await System.Console.In.ReadLineAsync()
                               .ConfigureAwait(false);
        }
    }
}