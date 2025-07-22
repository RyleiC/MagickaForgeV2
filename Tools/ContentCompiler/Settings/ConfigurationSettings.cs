using ContentCompiler.Data.Languages;
using ContentCompiler.Settings.Nested;
using ContentCompiler.Settings.Nested.Compilation;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ContentCompiler.Settings
{
    public class ConfigurationSettings
    {
        [JsonPropertyName("ContentCompiler Version")]
        public string Version { get; init; }
        [JsonPropertyName("Magicka game path")]
        public string GamePath { get; init; }

        public LocalizationSettings Localization { get; init; }
        public CompilationSettings Compilation { get; init; }


        //Made to fix the weird Singleton issue

        private readonly static ConfigurationSettings _defaultConfiguration = new ConfigurationSettings()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
            GamePath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Magicka\\",
            Localization = new LocalizationSettings()
            {
                GenerateLanguageFiles = true,
                LanguageFileName = "ModdedStrings",
                Language = Languages.eng
            },

            Compilation = new CompilationSettings()
            {
                VerifyXNBs = true,
                Characters = new CharacterSettings()
                {
                    UseModern = true,
                }
            }
        };


        [JsonIgnore]
        public string LocalizationPath => Path.Combine(GamePath, $"Content\\Languages\\{Localization.Language}");
        public static ConfigurationSettings Default => _defaultConfiguration;
    }
}
