using System.Threading;
using System.Threading.Tasks;
using Test.It.Specifications;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public class DefaultConsoleApplicationHostStarter<TApplicationBuilder> : IConsoleApplicationHostStarter 
        where TApplicationBuilder : IConsoleApplicationBuilder, new()
    {
        private ConsoleApplicationTestServer _server;

        public IHostController Create(ITestConfigurer testConfigurer, params string[] arguments)
        {
            var applicationBuilder = new TApplicationBuilder();
            _server = ConsoleApplicationTestServer
                .Create(applicationBuilder
                        .WithStartArguments(arguments)
                        .CreateWith(testConfigurer));

            return _server.Client;
        }

        public async Task StartAsync(
            CancellationToken cancellationToken = default)
            => await _server.StartAsync(cancellationToken)
                      .ConfigureAwait(false);

        public void Dispose()
        {
            _server?.Dispose();
        }
    }
}