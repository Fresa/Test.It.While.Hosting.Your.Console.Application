using Test.It.Specifications;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public interface IConsoleApplicationConfiguration
    {
        /// <summary>
        /// Starts the hosting process.
        /// </summary>
        /// <param name="testConfigurer">Test configuration</param>
        /// <param name="arguments">Start arguments</param>
        /// <returns>Client that communicates with the hosted console application</returns>
        IConsoleClient Start(ITestConfigurer testConfigurer, params string[] arguments);
    }
}