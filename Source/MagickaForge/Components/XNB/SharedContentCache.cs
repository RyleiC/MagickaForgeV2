using MagickaForge.Components.Graphics.Effects;

namespace MagickaForge.Components.XNB
{
    public struct SharedContentCache
    {
        public ShaderEffect Effect { get; set; }

        public SharedContentCache(BinaryReader binaryReader, Header header)
        {
            Effect = ShaderEffect.GetEffect(binaryReader, header);
        }

        public void Write(BinaryWriter binaryWriter)
        {
            Effect.Write(binaryWriter);
        }
    }
}
