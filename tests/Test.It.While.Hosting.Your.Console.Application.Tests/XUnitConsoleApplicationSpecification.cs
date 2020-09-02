using System.Threading.Tasks;
using Xunit;

namespace Test.It.While.Hosting.Your.Console.Application.Tests
{
    public class XUnitConsoleApplicationSpecification<TConfiguration> : ConsoleApplicationSpecification<TConfiguration>, IAsyncLifetime
        where TConfiguration : class, IConsoleApplicationHostStarter, new()
    {
        public async Task InitializeAsync() => await SetConfigurationAsync(new TConfiguration())
            .ConfigureAwait(false);

        public Task DisposeAsync() => Task.CompletedTask;
    }
}