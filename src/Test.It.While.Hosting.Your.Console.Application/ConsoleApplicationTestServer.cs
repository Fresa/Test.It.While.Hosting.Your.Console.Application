using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Test.It.Starters;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public class ConsoleApplicationTestServer : IDisposable
    {
        private readonly DefaultApplicationBuilder _appBuilder;
        private readonly IDictionary<string, object> _environment;

        private ConsoleApplicationTestServer(IApplicationStarter<IHostController> applicationStarter)
        {
            _appBuilder = new DefaultApplicationBuilder();
            var builder = new ControllerProvidingAppBuilder<IHostController>(_appBuilder);
            _environment = applicationStarter.Start(builder);
            Client = builder.Controller;
        }

        public IHostController Client { get; }

        public static ConsoleApplicationTestServer Create(IApplicationStarter<IHostController> applicationStarter)
        {
            return new ConsoleApplicationTestServer(applicationStarter);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _appBuilder.Build()(_environment, cancellationToken);
        }
        public void Dispose()
        {
        }
    }
}