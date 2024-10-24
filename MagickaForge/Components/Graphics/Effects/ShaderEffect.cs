using MagickaForge.Components.XNB;

namespace MagickaForge.Components.Graphics.Effects
{
    public class ShaderEffect
    {
        public static ShaderEffect GetEffect(BinaryReader binaryReader, Header header)
        {
            int type = binaryReader.ReadByte();

            if (type == header.GetReaderIndex(ReaderType.RenderDeferred))
            {
                return new RenderDeferredEffect(binaryReader);
            }
            else if (type == header.GetReaderIndex(ReaderType.AdditiveEffect))
            {
                return new AdditiveEffect(binaryReader);
            }
            else if (type == header.GetReaderIndex(ReaderType.WaterEffect))
            {
                return new RenderDeferredLiquidEffect(binaryReader);
            }
            else
            {
                return new LavaEffect(binaryReader);
            }
            throw new ArgumentException();
        }
    }
}
