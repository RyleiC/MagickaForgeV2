using MagickaForge.Utils;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class Light
    {
        public string Name { get; set; }
        public float[] Position { get; set; }
        public float[] Direction { get; set; }
        public LightType Type { get; set; }
        public LightVariationType Variation { get; set; }
        public float Distance { get; set; }
        public bool UseAttenuation { get; set; }
        public float CutoffAngle { get; set; }
        public float Sharpness { get; set; }
        public float[] DiffuseColor { get; set; }
        public float[] AmbientColor { get; set; }
        public float SpecularAmount { get; set; }
        public float VariationSpeed { get; set; }
        public float VariationAmount { get; set; }
        public int ShadowSize { get; set; }
        public bool CastShadows { get; set; }

        public Light(BinaryReader binaryReader)
        {
            Name = binaryReader.ReadString();
            Position = new float[3];
            for (int i = 0; i < 3; i++)
            {
                Position[i] = binaryReader.ReadSingle();
            }
            Direction = new float[3];
            for (int i = 0; i < 3; i++)
            {
                Direction[i] = binaryReader.ReadSingle();
            }
            Type = (LightType)binaryReader.ReadInt32();
            Variation = (LightVariationType)binaryReader.ReadInt32();
            Distance = binaryReader.ReadSingle();
            UseAttenuation = binaryReader.ReadBoolean();
            CutoffAngle = binaryReader.ReadSingle();
            Sharpness = binaryReader.ReadSingle();
            DiffuseColor = new float[3];
            for (int i = 0; i < 3; i++)
            {
                DiffuseColor[i] = binaryReader.ReadSingle();
            }
            AmbientColor = new float[3];
            for (int i = 0; i < 3; i++)
            {
                AmbientColor[i] = binaryReader.ReadSingle();
            }
            SpecularAmount = binaryReader.ReadSingle();
            VariationSpeed = binaryReader.ReadSingle();
            VariationSpeed = binaryReader.ReadSingle();
            ShadowSize = binaryReader.ReadInt32();
            CastShadows = binaryReader.ReadBoolean();
        }
    }
}
