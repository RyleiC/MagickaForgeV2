using MagickaForge.Utils.Definitions.Graphics;
using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class Light
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public LightType Type { get; set; }
        public LightVariationType Variation { get; set; }
        public float Distance { get; set; }
        public bool UseAttenuation { get; set; }
        public float CutoffAngle { get; set; }
        public float Sharpness { get; set; }
        public Color DiffuseColor { get; set; }
        public Color AmbientColor { get; set; }
        public float SpecularAmount { get; set; }
        public float VariationSpeed { get; set; }
        public float VariationAmount { get; set; }
        public int ShadowSize { get; set; }
        public bool CastShadows { get; set; }
        public Light() { }
        public Light(BinaryReader binaryReader)
        {
            Name = binaryReader.ReadString();
            Position = new Vector3(binaryReader);
            Direction = new Vector3(binaryReader);
            Type = (LightType)binaryReader.ReadInt32();
            Variation = (LightVariationType)binaryReader.ReadInt32();
            Distance = binaryReader.ReadSingle();
            UseAttenuation = binaryReader.ReadBoolean();
            CutoffAngle = binaryReader.ReadSingle();
            Sharpness = binaryReader.ReadSingle();
            DiffuseColor = new Color(binaryReader);
            AmbientColor = new Color(binaryReader);
            SpecularAmount = binaryReader.ReadSingle();
            VariationSpeed = binaryReader.ReadSingle();
            VariationAmount = binaryReader.ReadSingle();
            ShadowSize = binaryReader.ReadInt32();
            CastShadows = binaryReader.ReadBoolean();
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Name);
            Position.Write(binaryWriter);
            Direction.Write(binaryWriter);
            binaryWriter.Write((int)Type);
            binaryWriter.Write((int)Variation);
            binaryWriter.Write(Distance);
            binaryWriter.Write(UseAttenuation);
            binaryWriter.Write(CutoffAngle);
            binaryWriter.Write(Sharpness);
            DiffuseColor.Write(binaryWriter);
            AmbientColor.Write(binaryWriter);
            binaryWriter.Write(SpecularAmount);
            binaryWriter.Write(VariationSpeed);
            binaryWriter.Write(VariationAmount);
            binaryWriter.Write(ShadowSize);
            binaryWriter.Write(CastShadows);
        }
    }
}
