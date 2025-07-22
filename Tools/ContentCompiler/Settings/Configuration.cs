using ContentCompiler.Misc;
using System.Reflection;
using System.Text.Json;

namespace ContentCompiler.Settings
{
    internal class Configuration
    {
        private static readonly Lazy<Configuration> _instance = new Lazy<Configuration>(LoadConfiguration());
        public ConfigurationSettings Settings { get; private set; }

        private Configuration()
        {

        }

        public void ReloadSettings()
        {
            var configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config");
            var configFile = Path.Combine(configPath, "config.json");

            var text = File.ReadAllText(configFile);
            Settings = JsonSerializer.Deserialize<ConfigurationSettings>(text);
        }

        private static Configuration LoadConfiguration()
        {
            var configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config");
            var configFile = Path.Combine(configPath, "config.json");

            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
                GenerateBaseConfig(configFile);
            }
            else if (!File.Exists(configFile))
            {
                GenerateBaseConfig(configFile);
            }

            var text = File.ReadAllText(configFile);
            var configuration = new Configuration()
            {
                Settings = JsonSerializer.Deserialize<ConfigurationSettings>(text)
            };

            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (configuration.Settings.Version != currentVersion)
            {
                Logger.WriteWarning("Your config is not made for the same version of ContentCompiler! Consider deleting the config to rebuild it!\n");
            }

            if (!Directory.Exists(configuration.Settings.GamePath))
            {
                throw new DirectoryNotFoundException($"The specified game path '{configuration.Settings.GamePath}' does not exist. Please update the configuration file at '{configFile}'.");
            }

            return configuration;
        }

        private static void GenerateBaseConfig(string path)
        {
            using var streamWriter = new StreamWriter(File.Create(path));

            var json = JsonSerializer.Serialize(ConfigurationSettings.Default, new JsonSerializerOptions
            {
                WriteIndented = true,
            });

            streamWriter.Write(json);
        }

        public static Configuration Instance => _instance.Value;
    }
}
