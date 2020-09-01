using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Should.Fluent;
using Test.It.While.Hosting.Your.Console.Application.Consoles;
using Xunit;

namespace Test.It.While.Hosting.Your.Console.Application.Tests
{
    public class When_testing_a_console_application :
        XUnitConsoleApplicationSpecification<
            DefaultConsoleApplicationHostStarter<TestConsoleApplicationBuilder>>
    {
        private readonly List<string> _output = new List<string>();

        protected override IEnumerable<string> StartArguments { get; } =
            new List<string> {"arg1", "arg2"};

        protected override Task WhenAsync(
            CancellationToken cancellation)
        {
            Client.OutputReceived += (
                sender,
                message) => _output.Add(message);
            Client.Input("test");
            return Task.CompletedTask;
        }

        [Fact]
        public void It_should_have_written_the_input()
        {
            _output.Should()
                   .Contain.Item("test");
        }

        [Fact]
        public void It_should_have_output_the_start_arguments()
        {
            _output.Should()
                   .Contain.Item("Arguments: arg1, arg2");
        }
    }
}