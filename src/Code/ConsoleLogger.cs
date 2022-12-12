namespace PurTools
{
    public class ConsoleLogger : Logger
    {
        public override void Info(string message) => Console.WriteLine($"Info: {message}");
        public override void Warn(string message) => Console.WriteLine($"Warn: {message}");
        public override void Error(string message) => Console.WriteLine($"Error: {message}");
    }
}
