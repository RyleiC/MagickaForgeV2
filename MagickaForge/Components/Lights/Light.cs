using MagickaForge.Components.Common;
using MagickaForge.Utils.Data.Graphics;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Lights
{
    public struct Light
    {
        public float Radius { get; set; }
        public Color DiffuseColor { get; set; }
        public Color AmbientColor { get; set; }
        public float SpecularAmount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<LightVariationType>))]
        public LightVariationType LightVariationType { get; set; }
        public float VariationAmount { get; set; }
        public float VariationSpeed { get; set; }

        public Light(BinaryReader binaryReader)
        {
            Radius = binaryReader.ReadSingle();
            DiffuseColor = new Color(binaryReader);
            AmbientColor = new Color(binaryReader);
            SpecularAmount = binaryReader.ReadSingle();
            LightVariationType = (LightVariationType)binaryReader.ReadByte();
            VariationAmount = binaryReader.ReadSingle();
            VariationSpeed = binaryReader.ReadSingle();
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Radius);
            DiffuseColor.Write(binaryWriter);
            AmbientColor.Write(binaryWriter);
            binaryWriter.Write(SpecularAmount);
            binaryWriter.Write((byte)LightVariationType);
            binaryWriter.Write(VariationAmount);
            binaryWriter.Write(VariationSpeed);
        }
    }

}
