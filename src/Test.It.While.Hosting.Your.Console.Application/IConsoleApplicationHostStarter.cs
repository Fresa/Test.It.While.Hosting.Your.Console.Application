using System;
using System.Threading;
using System.Threading.Tasks;
using Test.It.Specifications;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public interface IConsoleApplicationHostStarter : IDisposable
    {
        /// <summary>
        /// Creates the hosting process.
        /// </summary>
        /// <param name="testConfigurer">Test configurer</param>
        /// <param name="arguments">Start arguments</param>
        /// <returns>Client that communicates with the hosted console application</returns>
        IConsoleClient Create(
            ITestConfigurer testConfigurer,
            params string[] arguments);

        /// <summary>
        /// Starts the hosting process.
        /// </summary>
        Task StartAsync(
            CancellationToken cancellationToken = default);
    }
}