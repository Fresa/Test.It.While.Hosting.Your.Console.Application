using Test.It.Specifications;

namespace Test.It.While.Hosting.Your.Console.Application.Tests
{
    public class TestConsoleApplicationBuilder : DefaultConsoleApplicationBuilder
    {
        public override IConsoleApplication Create(ITestConfigurer configurer)
        {
            var app = new TestConsoleApp(configurer.Configure);

            return new TestConsoleApplicationWrapper(app);
        }

        private class TestConsoleApplicationWrapper : IConsoleApplication
        {
            private readonly TestConsoleApp _app;

            public TestConsoleApplicationWrapper(TestConsoleApp app)
            {
                _app = app;
            }

            public int Start(params string[] args)
            {
                return _app.Start(args);
            }
        }
    }
}