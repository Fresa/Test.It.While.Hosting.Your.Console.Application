namespace Test.It.While.Hosting.Your.Console.Application
{
    public interface IConsoleApplication
    {
        /// <summary>
        /// Starts the Console Application
        /// </summary>
        /// <param name="args">Any argument that the Console Application should start with</param>
        /// <returns>Start process status defined by the Console Application.</returns>
        int Start(params string[] args);
    }
}