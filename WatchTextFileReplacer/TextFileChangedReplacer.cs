namespace WatchTextFileReplacer
{
    class RecursiveReplaceReport
    {
        public int Replaced { get; set; } = 0;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;

        public int MatchFiles { get; set; } = 0;
    }
    internal class TextFileChangedReplacer
    {
        public WatchTask Task { get; private set; }
        private FileSystemWatcher _watcher;

        public void StartWatch(WatchTask task)
        {
            if (_watcher != null) return;

            Task = task;
            _watcher = new FileSystemWatcher();

            _watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName
                              | NotifyFilters.LastWrite | NotifyFilters.Size;
            _watcher.IncludeSubdirectories = true;
            _watcher.Path = Task.WatcherPath;
            _watcher.Filter = Task.WatchFileType;
            _watcher.Changed += OnChanged;
            _watcher.Created += OnFileCreated;
            _watcher.Renamed += OnRenamed;
            _watcher.EnableRaisingEvents = true;

            Logger.ConsoleWriteLine($"Start Watch:{task.WatcherPath}");
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Replace(e.FullPath, Task.OldString, Task.NewString, true);
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Replace(e.FullPath, Task.OldString, Task.NewString, true);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Replace(e.FullPath, Task.OldString, Task.NewString, true);
        }

        public static RecursiveReplaceReport StartRecursiveReplace(WatchTask task)
        {
            var report = new RecursiveReplaceReport();
            report.StartTime = DateTime.Now;
            RecursiveReplace(task.WatcherPath, task.WatchFileType, task.OldString, task.NewString, task.IgnoreCase, report);
            report.EndTime = DateTime.Now;
            return report;
        }

        private static void RecursiveReplace(string watcherPath, string watchFileType, string oldString, string newString, bool ignoreCase, RecursiveReplaceReport report)
        {
            if (Directory.Exists(watcherPath))
            {
                var files = Directory.GetFiles(watcherPath, watchFileType);
                foreach (var file in files)
                {
                    report.MatchFiles++;
                    if (Replace(file, oldString, newString, ignoreCase)) report.Replaced++;
                }
                var dirs = Directory.GetDirectories(watcherPath);
                foreach (var dir in dirs)
                {
                    RecursiveReplace(dir, watchFileType, oldString, newString, ignoreCase, report);
                }
            }
        }

        public static bool Replace(string textPath, string textToReplace, string replacement, bool ignoreCase)
        {
            var content = File.ReadAllText(textPath);
            if (content.Contains(textToReplace, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                content = content.Replace(textToReplace, replacement, ignoreCase, null);
                File.WriteAllText(textPath, content);
                Logger.ConsoleWriteLine($"Text Replaced:{textPath}");
                return true;
            }
            return false;
        }
    }
}
