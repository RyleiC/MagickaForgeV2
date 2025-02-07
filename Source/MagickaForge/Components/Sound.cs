using MagickaForge.Utils.Data;
using System.Text.Json.Serialization;

namespace MagickaForge.Components
{
    public struct Sound
    {
        public string Cue { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Banks>))]
        public Banks Bank { get; set; }

        public Sound(BinaryReader binaryReader)
        {
            Cue = binaryReader.ReadString();
            Bank = (Banks)binaryReader.ReadInt32();
        }

        public readonly void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Cue);
            binaryWriter.Write((int)Bank);
        }
    }
}
