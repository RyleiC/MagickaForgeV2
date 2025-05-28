using MagickaForge.Components.Graphics.Effects;

namespace MagickaForge.Components.XNB
{
    public struct SharedContentCache
    {
        public ShaderEffect Effect { get; set; }

        public SharedContentCache(BinaryReader binaryReader, DynamicHeader header)
        {
            Effect = ShaderEffect.GetEffect(binaryReader, header);
        }

        public readonly void Write(BinaryWriter binaryWriter)
        {
            Effect.Write(binaryWriter);
        }
    }
}
