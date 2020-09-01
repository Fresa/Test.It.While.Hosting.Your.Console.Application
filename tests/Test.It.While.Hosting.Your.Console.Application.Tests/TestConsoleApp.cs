using System;
using System.Threading.Tasks;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application.Tests
{
    public class TestConsoleApp
    {
        private static TestConsoleApp _app;
        private readonly SimpleServiceContainer _serviceContainer;

        public TestConsoleApp(Action<IServiceContainer> reconfigurer)
        {
            _serviceContainer = new SimpleServiceContainer();
            _serviceContainer.RegisterSingleton<IConsole, Consoles.Console>();

            reconfigurer(_serviceContainer);
            _serviceContainer.Verify();
        }

        public Task<int> StartAsync(params string[] args)
        {
            var console = _serviceContainer.Resolve<IConsole>();
            console.WriteLine(console.ReadLine());
            console.WriteLine("Arguments: " + string.Join(", ", args));
            Stopped?.Invoke(this, 0);
            return Task.FromResult(0);
        }

        public event EventHandler<int> Stopped;

        public static async Task<int> Main(params string[] args)
        {
            _app = new TestConsoleApp(container => { });
            return await _app.StartAsync(args)
                .ConfigureAwait(false);
        }
    }
}