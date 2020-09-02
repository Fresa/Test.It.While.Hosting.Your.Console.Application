using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public interface IApplicationHost
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        /// <param name="cancellationToken">Cancels the start process</param>
        /// <param name="args">Any argument that the application might use</param>
        /// <returns>Execution status returned by the application</returns>
        Task<int> RunAsync(CancellationToken cancellationToken = default, params string[] args);

        /// <summary>
        /// Unhandled exception event
        /// </summary>
        event HandleExceptionAsync OnUnhandledException;
    }
}