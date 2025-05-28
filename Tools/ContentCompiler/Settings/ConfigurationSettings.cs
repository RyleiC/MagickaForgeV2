using ContentCompiler.Data.Languages;
using System.Text.Json.Serialization;

namespace ContentCompiler.Settings
{
    public class ConfigurationSettings
    {

        [JsonPropertyName("Magicka game path")]
        public string GamePath { get; init; }
        [JsonPropertyName("Localization : Autogenerate localization files")]
        public bool GenerateLanguageFiles { get; init; }
        [JsonPropertyName("Localization : Localization file name")]
        public string LanguageFileName { get; init; }
        [JsonConverter(typeof(JsonStringEnumConverter<Languages>))]
        [JsonPropertyName("Localization : Localization Language")]
        public Languages Language { get; init; }
        [JsonPropertyName("Verification : Verify assets on compile")]
        public bool VerifyXNBs { get; init; }



        //Made to fix the weird Singleton issue

        private readonly static ConfigurationSettings _defaultConfiguration = new ConfigurationSettings()
        {
            GamePath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Magicka\\",
            VerifyXNBs = true,
            GenerateLanguageFiles = true,
            Language = Languages.eng,
            LanguageFileName = "ModdedStrings",
        };

        [JsonIgnore]
        public string LocalizationPath => Path.Combine(GamePath, $"Content\\Languages\\{Language}");

        public static ConfigurationSettings Default => _defaultConfiguration;
    }
}
