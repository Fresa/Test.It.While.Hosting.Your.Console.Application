using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Console.Application
{
    public delegate Task HandleExceptionAsync(Exception exception,
        CancellationToken cancellationToken);
}