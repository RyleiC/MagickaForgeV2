using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Graphics.Effects
{
    public class AdditiveEffect : ShaderEffect
    {
        public Color _colorTint { get; set; }
        public bool _useVertexColor { get; set; }
        public bool _hasTexture { get; set; }
        public string? _texture { get; set; }
        public AdditiveEffect() { }
        public AdditiveEffect(BinaryReader binaryReader)
        {
            _colorTint = new Color(binaryReader);
            _useVertexColor = binaryReader.ReadBoolean();
            _hasTexture = binaryReader.ReadBoolean();
            _texture = binaryReader.ReadString();
        }

        public override void Write(BinaryWriter binaryWriter)
        {
            base.Write(binaryWriter);
            _colorTint.Write(binaryWriter);
            binaryWriter.Write(_useVertexColor);
            binaryWriter.Write(_hasTexture);
            binaryWriter.Write(_texture);
        }
    }
}
