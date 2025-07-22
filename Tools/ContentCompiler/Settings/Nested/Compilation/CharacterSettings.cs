using System.Text.Json.Serialization;

namespace ContentCompiler.Settings.Nested.Compilation
{
    public class CharacterSettings
    {
        [JsonPropertyName("Decompile/Compile for Modern Version")]
        public bool UseModern { get; set; }
    }
}
