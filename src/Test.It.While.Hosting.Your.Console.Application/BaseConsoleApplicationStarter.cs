using System.Collections.Generic;
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
}