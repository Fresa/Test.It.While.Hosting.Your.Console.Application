using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public abstract class ConsoleApplicationSpecification<THostStarter>
        where THostStarter : class, IConsoleApplicationHostStarter, new()
    {
        private readonly SemaphoreSlim _waitForDisconnect = new SemaphoreSlim(0);
        private readonly ConcurrentQueue<Exception> _exceptions = new ConcurrentQueue<Exception>();

        /// <summary>
        /// Start arguments. Defaults to none.
        /// </summary>
        protected virtual IEnumerable<string> StartArguments { get; } = Enumerable.Empty<string>();

        /// <summary>
        /// Execution timeout. Defaults to 3 seconds.
        /// </summary>
        protected TimeSpan Timeout { private get; set; } = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Set when the application exits
        /// </summary>
        protected int ExitCode { get; private set; }

        /// <summary>
        /// Bootstraps and starts the hosted application.
        /// </summary>
        /// <param name="hostStarter">Console application hostStarter</param>
        /// <param name="cancellationToken">Cancels the hostStarter process</param>
        public async Task SetConfigurationAsync(THostStarter hostStarter, CancellationToken cancellationToken = default)
        {
            Client = hostStarter.Create(new SimpleTestConfigurer(Given), StartArguments.ToArray());
            Client.OnUnhandledExceptionAsync += OnUnhandledExceptionAsync;
            Client.Disconnected += (sender, exitCode) =>
            {
                ExitCode = exitCode;
                _waitForDisconnect.Release();
            };

            var timeoutCancellationToken = CancellationTokenSource
                                           .CreateLinkedTokenSource(
                                               cancellationToken,
                                               new CancellationTokenSource(
                                                   Timeout).Token)
                                           .Token;

            await hostStarter.StartAsync(timeoutCancellationToken)
                             .ConfigureAwait(false);

            await WhenAsync(timeoutCancellationToken)
                .ConfigureAwait(false);

            await _waitForDisconnect.WaitAsync(timeoutCancellationToken)
                                    .ConfigureAwait(false);
            
            HandleExceptions();
        }

        private void HandleExceptions()
        {
            if (_exceptions.Any() == false)
            {
                return;
            }

            var uniqueExceptions = _exceptions.Distinct()
                                              .ToList();
            if (uniqueExceptions.Count == 1)
            {
                ExceptionDispatchInfo.Capture(uniqueExceptions.First()).Throw();
            }

            throw new AggregateException(uniqueExceptions);
        }

        private Task OnUnhandledExceptionAsync(
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (!(exception is AggregateException aggregateException))
            {
                _exceptions.Enqueue(exception);
                return Task.CompletedTask;
            }

            foreach (var innerException in aggregateException.InnerExceptions)
            {
                _exceptions.Enqueue(innerException);
            }
        
            return Task.CompletedTask;
        }

        /// <summary>
        /// Client to communicate with the hosted console application.
        /// </summary>
        protected IConsoleClient Client { get; private set; }

        /// <summary>
        /// OBS! <see cref="Client"/> is not ready here since the application is in a startup face where you control the service hostStarter.
        /// </summary>
        /// <param name="configurer">Service container</param>
        protected virtual void Given(IServiceContainer configurer) { }

        /// <summary>
        /// Application has started, and is reachable through <see cref="Client"/>.
        /// </summary>
        protected virtual Task WhenAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}