// See https://aka.ms/new-console-template for more information
using WatchTextFileReplacer;

internal class Program
{
    private static AppConfig _appConfig;
    private static List<TextFileChangedReplacer> _replacers = null;

    private static void Main(string[] args)
    {

        var configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WatchTextFileReplacer", "config.json");
        _replacers = new List<TextFileChangedReplacer>();
        if (File.Exists(configPath))
        {
            _appConfig = ConfigReader.Read(configPath);

            if (args.Contains("-run"))
            {
                foreach (var task in _appConfig.Tasks)
                {
                    var report = TextFileChangedReplacer.StartRecursiveReplace(task);
                    Logger.ConsoleWriteLine($"Matched Files:{report.MatchFiles}");
                    Logger.ConsoleWriteLine($"Replaced:{report.Replaced}");
                    Logger.ConsoleWriteLine($"Duration:{report.Duration}");
                }
            }
            else
            {
                foreach (var task in _appConfig.Tasks)
                {
                    var replacer = new TextFileChangedReplacer();
                    replacer.StartWatch(task);
                    _replacers.Add(replacer);
                }
                while (Console.ReadLine() == null) ;
            }

        }
        else
        {
            Logger.ConsoleWriteLine($"Could not find config:{configPath}");
            return;
        }
    }
}