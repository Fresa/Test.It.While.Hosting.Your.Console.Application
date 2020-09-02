using System;
using System.Threading.Tasks;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application.Tests
{
    public class TestConsoleApp
    {
        private readonly IConsole _console;
        private static TestConsoleApp _app;
        private readonly SimpleServiceContainer _serviceContainer;

        public TestConsoleApp(
            Action<IServiceContainer> reconfigurer,
            IConsole console)
        {
            _console = console;
            _serviceContainer = new SimpleServiceContainer();

            reconfigurer(_serviceContainer);
            _serviceContainer.Verify();
        }

        public async Task<int> StartAsync(
            params string[] args)
        {
            _console.WriteLine(
                await _console.ReadAsync()
                              .ConfigureAwait(false));
            _console.WriteLine("Arguments: " + string.Join(", ", args));
            Stopped?.Invoke(this, 0);
            return 0;
        }

        public event EventHandler<int> Stopped;

        public static async Task<int> Main(
            params string[] args)
        {
            _app = new TestConsoleApp(container => { }, new Consoles.Console());
            return await _app.StartAsync(args)
                             .ConfigureAwait(false);
        }
    }
}