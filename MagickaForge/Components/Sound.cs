using MagickaForge.Utils;
using System.Text.Json.Serialization;

namespace MagickaForge.Components
{
    public struct Sound
    {
        public string? Cue { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Banks>))]
        public Banks Bank { get; set; }
    }
}
