using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    internal class ConsoleHostingMiddleware : IMiddleware
    {
        private readonly IApplicationHost _host;
        private readonly IHostController _hostController;
        private Func<IDictionary<string, object>, CancellationToken, Task> _next;

        public ConsoleHostingMiddleware(IApplicationHost host, IHostController hostController)
        {
            _host = host;
            _hostController = hostController;
        }

        public void Initialize(Func<IDictionary<string, object>, CancellationToken, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment, CancellationToken cancellationToken)
        {
            try
            {
                var startParameters =
                    environment[EnvironmentKeys.StartParameters] as string[] ??
                    new string[0];

                _host.OnUnhandledException += OnUnhandledException;
                _ = Task.Run(
                    async () =>
                    {
                        var exit = await _host
                                         .RunAsync(
                                             cancellationToken, startParameters)
                                         .ConfigureAwait(false);
                        _hostController.Disconnect(exit);
                        _host.OnUnhandledException -= OnUnhandledException;
                    }, cancellationToken);
            }
            catch (Exception exception)
            {
                await _hostController.RaiseExceptionAsync(exception, cancellationToken)
                                     .ConfigureAwait(false);
            }

            if (_next != null)
            {
                await _next.Invoke(environment, cancellationToken);
            }
        }

        private void OnUnhandledException(Exception exception)
        {
            _hostController.RaiseExceptionAsync(exception)
                           .ConfigureAwait(false);
        }
    }
}