using Test.It.Specifications;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public class DefaultConsoleApplicationConfiguration<TApplicationBuilder> : IConsoleApplicationConfiguration 
        where TApplicationBuilder : IConsoleApplicationBuilder, new()
    {
        public IConsoleClient Start(ITestConfigurer testConfigurer, params string[] arguments)
        {
            var applicationBuilder = new TApplicationBuilder();
            var server = ConsoleApplicationTestServer.Create(applicationBuilder.WithStartArguments(arguments).CreateWith(testConfigurer).Start);

            return server.Client;
        }
    }
}