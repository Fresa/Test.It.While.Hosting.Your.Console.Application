using Test.It.Specifications;
using Test.It.Starters;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public abstract class DefaultConsoleApplicationBuilder : IConsoleApplicationBuilder
    {
        private string[] _arguments = new string[0];
        public abstract IConsoleApplication Create(ITestConfigurer configurer);
        
        public IApplicationStarter<IConsoleClient> CreateWith(ITestConfigurer configurer)
        {
            var testConsole = new TestConsole();
            var application = Create(new TestConsoleRegistringTestConfigurerDecorator(testConsole, configurer));

            void InternalStarter()
            {
                var exitCode = application.Start(_arguments);
                testConsole.Disconnect(exitCode);
            }

            return new DefaultConsoleApplicationStarter<IConsoleClient>(InternalStarter, new ConsoleClient(testConsole));
        }

        public IConsoleApplicationBuilder WithStartArguments(params string[] arguments)
        {
            _arguments = arguments;
            return this;
        }

        private class TestConsoleRegistringTestConfigurerDecorator : ITestConfigurer
        {
            private readonly IConsole _console;
            private readonly ITestConfigurer _configurer;

            public TestConsoleRegistringTestConfigurerDecorator(IConsole console, ITestConfigurer configurer)
            {
                _console = console;
                _configurer = configurer;
            }

            public void Configure(IServiceContainer container)
            {
                _configurer.Configure(container);
                container.RegisterSingleton(() => _console);
            }
        }
    }
}