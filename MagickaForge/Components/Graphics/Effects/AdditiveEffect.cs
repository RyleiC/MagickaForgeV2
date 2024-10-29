using MagickaForge.Components.Events;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Graphics.Effects
{
    public class AdditiveEffect : ShaderEffect
    {
        private float[] _colorTint { get; set; }
        private bool _useVertexColor { get; set; }
        private bool _hasTexture { get; set; }
        private string? _texture { get; set; }
        public AdditiveEffect() { }
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

        public override void Write(BinaryWriter binaryWriter)
        {
            base.Write(binaryWriter);
            for (int i = 0; i < 3; i++)
            {
                binaryWriter.Write(_colorTint[i]);
            }
            binaryWriter.Write(_useVertexColor);
            binaryWriter.Write(_hasTexture);
            binaryWriter.Write(_texture);
        }
    }
}
