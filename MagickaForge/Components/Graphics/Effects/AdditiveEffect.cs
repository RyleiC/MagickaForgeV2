using MagickaForge.Components.Common;

namespace MagickaForge.Components.Graphics.Effects
{
    public class AdditiveEffect : ShaderEffect
    {
        public Color ColorTint { get; set; }
        public bool UseVertexColor { get; set; }
        public bool HasTexture { get; set; }
        public string? Texture { get; set; }
        public AdditiveEffect() { }
        public AdditiveEffect(BinaryReader binaryReader)
        {
            ColorTint = new Color(binaryReader);
            UseVertexColor = binaryReader.ReadBoolean();
            HasTexture = binaryReader.ReadBoolean();
            Texture = binaryReader.ReadString();
        }

        public override void Write(BinaryWriter binaryWriter)
        {
            base.Write(binaryWriter);
            ColorTint.Write(binaryWriter);
            binaryWriter.Write(UseVertexColor);
            binaryWriter.Write(HasTexture);
            binaryWriter.Write(Texture);
        }
    }
}
