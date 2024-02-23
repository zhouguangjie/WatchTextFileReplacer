namespace WatchTextFileReplacer
{
    internal class Logger
    {
        public static void ConsoleWriteLine(string message)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("yyyyMMddHHmmss")}] {message}");
        }
    }
}
