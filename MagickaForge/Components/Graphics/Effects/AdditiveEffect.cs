namespace MagickaForge.Components.Graphics.Effects
{
    public class AdditiveEffect : ShaderEffect
    {
        private float[] _colorTint;
        private bool _useVertexColor;
        private bool _hasTexture;
        private string? _texture;

        public AdditiveEffect(BinaryReader binaryReader)
        {
            _colorTint = new float[4];
            for (int i = 0; i < 3; i++)
            {
                _colorTint[i] = binaryReader.ReadSingle();
            }
            _colorTint[3] = 1f;
            _useVertexColor = binaryReader.ReadBoolean();
            _hasTexture = binaryReader.ReadBoolean();
            _texture = binaryReader.ReadString();
        }
    }
}
