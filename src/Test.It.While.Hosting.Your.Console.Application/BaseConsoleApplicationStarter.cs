using System;
using Test.It.AppBuilders;
using Test.It.Starters;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public abstract class BaseConsoleApplicationStarter<TConsoleClient> : IApplicationStarter<TConsoleClient>
        where TConsoleClient : IConsoleClient
    {
        protected abstract TConsoleClient Client { get; }
        protected abstract Action Starter { get; }

        public void Start(IAppBuilder<TConsoleClient> applicationBuilder)
        {
            applicationBuilder.WithController(Client).Use(new TaskStartingMiddleware(Starter));
        }        
    }
}