using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public interface IConsoleApplication
    {
        /// <summary>
        /// Run the Console Application
        /// </summary>
        /// <param name="args">Any argument that the Console Application should start with</param>
        /// <returns>Exit code defined by the Console Application.</returns>
        Task<int> RunAsync(params string[] args);
    }
}