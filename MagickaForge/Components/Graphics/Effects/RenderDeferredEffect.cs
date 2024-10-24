namespace MagickaForge.Components.Graphics.Effects
{
    public class RenderDeferredEffect : ShaderEffect
    {
        private float _alpha;
        private float _sharpness;
        private bool _vertexColorEnabled;
        private bool _useTextureAsReflectiveness;
        private string? _reflectionMap;
        private Material _materialA;
        private Material _materialB;

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
                DiffuseColor = [binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()],
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
                    DiffuseColor = [binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()],
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
    }
}
