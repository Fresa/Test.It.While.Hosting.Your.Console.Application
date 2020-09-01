using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Test.It.ApplicationBuilders;
using Test.It.Starters;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public abstract class BaseConsoleApplicationStarter<TConsoleClient> : IApplicationStarter<TConsoleClient>
        where TConsoleClient : IHostController
    {
        protected abstract TConsoleClient Client { get; }
        protected abstract IApplicationHost Host { get; }

        protected abstract IDictionary<string, object> Environment { get; }

        public virtual IDictionary<string, object> Start(IApplicationBuilder<TConsoleClient> applicationBuilder)
        {
            applicationBuilder.WithController(Client)
                              .Use(new ConsoleHostingMiddleware(Host, Client));
            return Environment;
        }
    }

    public interface IApplicationHost
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        /// <param name="cancellationToken">Cancels the start process</param>
        /// <param name="args">Any argument that the application might use</param>
        /// <returns>Execution status returned by the application</returns>
        Task<int> RunAsync(CancellationToken cancellationToken = default, params string[] args);

        /// <summary>
        /// Unhandled exception event
        /// </summary>
        event Action<Exception> OnUnhandledException;
    }

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

    internal static class EnvironmentKeys
    {
        public const string StartParameters = "start_parameters";
    }

    public delegate Task StoppedAsync(int exitCode,
        CancellationToken cancellationToken);
}