using MagickaForge.Utils.Data;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Common
{
    public struct Resistance
    {
        [JsonConverter(typeof(JsonStringEnumConverter<Elements>))]
        public Elements Element { get; set; }
        public float Multiplier { get; set; }
        public float Modifier { get; set; }
        public bool StatusImmunity { get; set; }

        public Resistance(BinaryReader binaryReader)
        {
            Element = (Elements)binaryReader.ReadInt32();
            Multiplier = binaryReader.ReadSingle();
            Modifier = binaryReader.ReadSingle();
            StatusImmunity = binaryReader.ReadBoolean();
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write((int)Element);
            binaryWriter.Write(Multiplier);
            binaryWriter.Write(Modifier);
            binaryWriter.Write(StatusImmunity);
        }
    }
}
