using Test.It.Specifications;
using Test.It.Starters;
using Test.It.While.Hosting.Your.Console.Application.Consoles;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public abstract class
        DefaultConsoleApplicationBuilder : IConsoleApplicationBuilder
    {
        private string[] _arguments = new string[0];

        public abstract IConsoleApplication Create(
            ITestConfigurer configurer);

        public IApplicationStarter<IHostController> CreateWith(
            ITestConfigurer configurer)
        {
            var application = Create(configurer);

            var host = new DefaultConsoleHost(application);

            return new DefaultConsoleApplicationStarter<IHostController>(
                host, _arguments, _hostController);
        }

        private readonly IHostController _hostController = new DefaultHostController();
        protected IConsole Console => _hostController; 

        public IConsoleApplicationBuilder WithStartArguments(
            params string[] arguments)
        {
            _arguments = arguments;
            return this;
        }
    }
}