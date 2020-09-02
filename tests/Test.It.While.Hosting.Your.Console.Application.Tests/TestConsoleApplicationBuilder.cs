using System.Threading;
using System.Threading.Tasks;
using Test.It.Specifications;

namespace Test.It.While.Hosting.Your.Console.Application.Tests
{
    public class TestConsoleApplicationBuilder : DefaultConsoleApplicationBuilder
    {
        public override IConsoleApplication Create(ITestConfigurer configurer)
        {
            var app = new TestConsoleApp(configurer.Configure, Console);

            return new TestConsoleApplicationWrapper(app);
        }

        private class TestConsoleApplicationWrapper : IConsoleApplication
        {
            private readonly TestConsoleApp _app;

            public TestConsoleApplicationWrapper(TestConsoleApp app)
            {
                _app = app;
            }

            public async Task<int> RunAsync(
                CancellationToken cancellationToken = default,
                params string[] args)
                => await _app.StartAsync(args, cancellationToken)
                             .ConfigureAwait(false);
        }
    }
}