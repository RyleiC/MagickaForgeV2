using ContentCompiler.Settings.Nested.Compilation;
using System.Text.Json.Serialization;

namespace ContentCompiler.Settings.Nested
{
    public class CompilationSettings
    {
        [JsonPropertyName("Verify content on compile")]
        public bool VerifyXNBs { get; init; }

        public CharacterSettings Characters { get; init; }
    }
}
