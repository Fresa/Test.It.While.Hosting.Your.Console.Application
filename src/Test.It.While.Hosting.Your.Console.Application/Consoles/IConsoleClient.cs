using System;

namespace Test.It.While.Hosting.Your.Console.Application.Consoles
{
    public interface IConsoleClient
    {
        event EventHandler<string> OutputReceived;
        event EventHandler<int> Disconnected;
        event HandleExceptionAsync OnUnhandledExceptionAsync;
        void Input(string message);
    }
}