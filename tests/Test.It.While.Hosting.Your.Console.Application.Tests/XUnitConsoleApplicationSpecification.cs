namespace Test.It.While.Hosting.Your.Console.Application.Tests
{
    public class XUnitConsoleApplicationSpecification<TConfiguration> : ConsoleApplicationSpecification<TConfiguration>
        where TConfiguration : class, IConsoleApplicationConfiguration, new()
    {
        public XUnitConsoleApplicationSpecification()
        {
            SetConfiguration(new TConfiguration());
        }
    }
}