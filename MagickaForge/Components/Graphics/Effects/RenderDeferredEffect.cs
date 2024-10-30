using MagickaForge.Utils.Structures;
namespace MagickaForge.Components.Graphics.Effects
{
    public class RenderDeferredEffect : ShaderEffect
    {
        public float _alpha { get; set; }
        public float _sharpness { get; set; }
        public bool _vertexColorEnabled { get; set; }
        public bool _useTextureAsReflectiveness { get; set; }
        public string? _reflectionMap { get; set; }
        public Material _materialA { get; set; }
        public Material _materialB { get; set; }
        public RenderDeferredEffect() { }
        public RenderDeferredEffect(BinaryReader binaryReader)
        {
            _alpha = binaryReader.ReadSingle();
            _sharpness = binaryReader.ReadSingle();
            _vertexColorEnabled = binaryReader.ReadBoolean();
            _useTextureAsReflectiveness = binaryReader.ReadBoolean();
            _reflectionMap = binaryReader.ReadString();
            _materialA = new Material()
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
                _materialB = new Material()
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
            binaryWriter.Write(_alpha);
            binaryWriter.Write(_sharpness);
            binaryWriter.Write(_vertexColorEnabled);
            binaryWriter.Write(_useTextureAsReflectiveness);
            binaryWriter.Write(_reflectionMap);
            _materialA.Write(binaryWriter);
            var hasMaterialB = _materialB != null;
            binaryWriter.Write(hasMaterialB);
            if (hasMaterialB)
            {
                _materialB.Write(binaryWriter);
            }
        }
    }
}
