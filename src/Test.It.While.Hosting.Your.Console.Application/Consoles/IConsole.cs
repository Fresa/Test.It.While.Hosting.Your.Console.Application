namespace Test.It.While.Hosting.Your.Console.Application.Consoles
{
    public interface IConsole
    {
        void WriteLine(string message);
        string ReadLine();
        string Title { get; set; }
    }
}