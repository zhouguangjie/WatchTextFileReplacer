using Newtonsoft.Json.Linq;

namespace WatchTextFileReplacer
{
    public class WatchTask
    {
        public string WatcherPath { get; set; }
        public string WatchFileType { get; set; }
        public string OldString { get; set; }
        public string NewString { get; set; }
        public bool IgnoreCase { get; set; }
    }
    class AppConfig
    {
        public List<WatchTask> Tasks { get; set; }

        public static AppConfig ReadFromJsonFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var jobj = JObject.Parse(json);
            var config = new AppConfig();
            config.Tasks = new List<WatchTask>();

            var taskObjs = jobj["Tasks"].ToArray();

            foreach (var task in taskObjs)
            {
                var taskObj = new WatchTask()
                {
                    WatcherPath = task["WatchPath"].Value<string>(),
                    WatchFileType = task["WatchFileType"].Value<string>(),
                    OldString = task["OldString"].Value<string>(),
                    NewString = task["NewString"].Value<string>(),
                    IgnoreCase = task["IgnoreCase"].Value<bool>(),
                };
                config.Tasks.Add(taskObj);
            }
            return config;

        }
    }

    internal class ConfigReader
    {
        public static AppConfig Read(string configPath)
        {
            return AppConfig.ReadFromJsonFile(configPath);
        }
    }
}
