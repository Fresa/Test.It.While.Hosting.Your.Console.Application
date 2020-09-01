using System;
using System.Collections;
using System.Collections.Generic;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    internal class
        DefaultConsoleApplicationStarter<TConsoleClient> :
            BaseConsoleApplicationStarter<TConsoleClient>
        where TConsoleClient : IHostController
    {
        public DefaultConsoleApplicationStarter(
            IApplicationHost host,
            IEnumerable arguments,
            TConsoleClient console)
        {
            Client = console;
            Host = host;
            Environment = new Dictionary<string, object>
            {
                {
                    EnvironmentKeys.StartParameters,
                    arguments
                }
            };
        }

        protected override IDictionary<string, object> Environment { get; }

        protected override TConsoleClient Client { get; }

        protected override IApplicationHost Host { get; }
    }
}