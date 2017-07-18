using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public abstract class ConsoleApplicationSpecification<TConfiguration> : IUseConfiguration<TConfiguration>
        where TConfiguration : class, IConsoleApplicationHostStarter, new()
    {
        private readonly AutoResetEvent _wait = new AutoResetEvent(false);

        /// <summary>
        /// Start arguments. Defaults to none.
        /// </summary>
        protected virtual IEnumerable<string> StartArguments { get; } = Enumerable.Empty<string>();
        
        /// <summary>
        /// Execution timeout. Defaults to 3 seconds.
        /// </summary>
        protected TimeSpan Timeout { private get; set; } = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Bootstraps and starts the hosted application.
        /// </summary>
        /// <param name="configuration">Console application configuration</param>
        public void SetConfiguration(TConfiguration configuration)
        {
            Client = configuration.Start(new SimpleTestConfigurer(Given), StartArguments.ToArray());
            Client.Disconnected += (sender, exitCode) => _wait.Set();

            When();

            Wait();
        }

        private void Wait()
        {
            if (_wait.WaitOne(Timeout) == false)
            {
                throw new TimeoutException($"Waited {Timeout.Seconds} seconds.");
            }
        }
        
        /// <summary>
        /// Client to communicate with the hosted console application.
        /// </summary>
        protected IConsoleClient Client { get; private set; }

        /// <summary>
        /// OBS! <see cref="Client"/> is not ready here since the application is in a startup face where you control the service configuration.
        /// </summary>
        /// <param name="configurer">Service container</param>
        protected virtual void Given(IServiceContainer configurer) { }

        /// <summary>
        /// Application has started, and is reachable through <see cref="Client"/>.
        /// </summary>
        protected virtual void When() { }
    }
}