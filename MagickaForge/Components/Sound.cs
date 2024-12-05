using MagickaForge.Utils.Definitions;
using System.Text.Json.Serialization;

namespace MagickaForge.Components
{
    public struct Sound
    {
        public string? Cue { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Banks>))]
        public Banks Bank { get; set; }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Cue);
            binaryWriter.Write((int)Bank);
        }
    }
}
