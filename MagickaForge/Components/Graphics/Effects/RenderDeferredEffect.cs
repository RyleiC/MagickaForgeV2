using MagickaForge.Utils.Structures;
namespace MagickaForge.Components.Graphics.Effects
{
    public class RenderDeferredEffect : ShaderEffect
    {
        public float Alpha { get; set; }
        public float Sharpness { get; set; }
        public bool VertexColorEnabled { get; set; }
        public bool UseTextureAsReflectiveness { get; set; }
        public string? ReflectionMap { get; set; }
        public Material MaterialA { get; set; }
        public Material MaterialB { get; set; }
        public RenderDeferredEffect() { }
        public RenderDeferredEffect(BinaryReader binaryReader)
        {
            Alpha = binaryReader.ReadSingle();
            Sharpness = binaryReader.ReadSingle();
            VertexColorEnabled = binaryReader.ReadBoolean();
            UseTextureAsReflectiveness = binaryReader.ReadBoolean();
            ReflectionMap = binaryReader.ReadString();
            MaterialA = new Material()
            {
                DiffuseNoAlpha = binaryReader.ReadBoolean(),
                AlphaMaskEnabled = binaryReader.ReadBoolean(),
                DiffuseColor = new Color(binaryReader),
                SpecularAmount = binaryReader.ReadSingle(),
                SpecularPower = binaryReader.ReadSingle(),
                EmissiveAmount = binaryReader.ReadSingle(),
                NormalPower = binaryReader.ReadSingle(),
                Reflectiveness = binaryReader.ReadSingle(),
                DiffuseTexture = binaryReader.ReadString(),
                MaterialTexture = binaryReader.ReadString(),
                NormalTexture = binaryReader.ReadString(),
            };
            var hasMaterialB = binaryReader.ReadBoolean();
            if (hasMaterialB)
            {
                MaterialB = new Material()
                {
                    DiffuseNoAlpha = binaryReader.ReadBoolean(),
                    AlphaMaskEnabled = binaryReader.ReadBoolean(),
                    DiffuseColor = new Color(binaryReader),
                    SpecularAmount = binaryReader.ReadSingle(),
                    SpecularPower = binaryReader.ReadSingle(),
                    EmissiveAmount = binaryReader.ReadSingle(),
                    NormalPower = binaryReader.ReadSingle(),
                    Reflectiveness = binaryReader.ReadSingle(),
                    DiffuseTexture = binaryReader.ReadString(),
                    MaterialTexture = binaryReader.ReadString(),
                    NormalTexture = binaryReader.ReadString(),
                };
            }
        }

        public override void Write(BinaryWriter binaryWriter)
        {
            base.Write(binaryWriter);
            binaryWriter.Write(Alpha);
            binaryWriter.Write(Sharpness);
            binaryWriter.Write(VertexColorEnabled);
            binaryWriter.Write(UseTextureAsReflectiveness);
            binaryWriter.Write(ReflectionMap);
            MaterialA.Write(binaryWriter);
            var hasMaterialB = MaterialB != null;
            binaryWriter.Write(hasMaterialB);
            if (hasMaterialB)
            {
                MaterialB.Write(binaryWriter);
            }
        }
    }
}
