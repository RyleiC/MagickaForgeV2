using MagickaForge.Components.Events;
using MagickaForge.Utils.Structures;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Graphics.Effects
{
    public class AdditiveEffect : ShaderEffect
    {
        private Color _colorTint { get; set; }
        private bool _useVertexColor { get; set; }
        private bool _hasTexture { get; set; }
        private string? _texture { get; set; }
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
