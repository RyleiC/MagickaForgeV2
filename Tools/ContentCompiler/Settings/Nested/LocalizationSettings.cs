using ContentCompiler.Data.Languages;
using System.Text.Json.Serialization;

namespace ContentCompiler.Settings.Nested
{
    public record LocalizationSettings
    {
        [JsonPropertyName("Autogenerate localization files")]
        public bool GenerateLanguageFiles { get; init; }


        [JsonPropertyName("Localization file name")]
        public string LanguageFileName { get; init; }


        [JsonConverter(typeof(JsonStringEnumConverter<Languages>))]
        [JsonPropertyName("Localization Language")]
        public Languages Language { get; init; }
    }
}
