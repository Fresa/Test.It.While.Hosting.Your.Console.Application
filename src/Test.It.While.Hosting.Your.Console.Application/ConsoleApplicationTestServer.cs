﻿using System;
using System.Collections.Generic;
using Microsoft.Owin.Builder;
using Test.It.AppBuilders;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public class ConsoleApplicationTestServer : IDisposable
    {
        private ConsoleApplicationTestServer(Action<IAppBuilder<IConsoleClient>> startup)
        {
            var appBuilder = new AppBuilder();
            var builder = new ControllerProvidingAppBuilder<IConsoleClient>(appBuilder);
            startup(builder);
            Client = builder.Controller;
            appBuilder.Build()(new Dictionary<string, object>());
        }

        public IConsoleClient Client { get; }

        public static ConsoleApplicationTestServer Create(Action<IAppBuilder<IConsoleClient>> startup)
        {
            return new ConsoleApplicationTestServer(startup);
        }

        public void Dispose()
        {
        }
    }
}