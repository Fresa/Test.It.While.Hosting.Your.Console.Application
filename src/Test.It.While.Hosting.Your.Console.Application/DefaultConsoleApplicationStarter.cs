using System;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    internal class DefaultConsoleApplicationStarter<TConsoleClient> : BaseConsoleApplicationStarter<TConsoleClient> 
        where TConsoleClient : IConsoleClient
    {
        public DefaultConsoleApplicationStarter(Action starter, TConsoleClient console)
        {
            Client = console;
            Starter = starter;
        }

        protected override TConsoleClient Client { get; }

        protected override Action Starter { get; }
    }
}