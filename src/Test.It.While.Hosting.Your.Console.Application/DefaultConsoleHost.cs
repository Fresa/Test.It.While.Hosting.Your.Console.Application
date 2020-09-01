using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public sealed class DefaultConsoleHost : IApplicationHost
    {
        private readonly IConsoleApplication _application;

        public DefaultConsoleHost(IConsoleApplication application)
        {
            _application = application;
        }

        public async Task<int> RunAsync(
            CancellationToken cancellationToken = default,
            params string[] args)
        {
            try
            {
                return await _application.RunAsync(args)
                                         .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                OnUnhandledException?.Invoke(e);
                return -1;
            }
        }

        public event Action<Exception> OnUnhandledException;
    }
}