using Test.It.Specifications;
using Test.It.Starters;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    /// <summary>
    /// Builds the Console Application host process
    /// </summary>
    public interface IConsoleApplicationBuilder
    {
        /// <summary>
        /// Creates the Console Application hosting process with a test configuration.
        /// </summary>
        /// <param name="configurer">A test configuration used to override the Console Application configuration.</param>
        /// <returns>An application starter</returns>
        IApplicationStarter<IHostController> CreateWith(ITestConfigurer configurer);

        /// <summary>
        /// Configure with start arguments
        /// </summary>
        /// <param name="arguments">Start arguments</param>
        /// <returns></returns>
        IConsoleApplicationBuilder WithStartArguments(params string[] arguments);
    }
}