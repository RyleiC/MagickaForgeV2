using MagickaForge.Components.XNB;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Graphics.Effects
{

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "_EffectType")]
    [JsonDerivedType(typeof(AdditiveEffect), typeDiscriminator: "Additive")]
    [JsonDerivedType(typeof(LavaEffect), typeDiscriminator: "Lava")]
    [JsonDerivedType(typeof(RenderDeferredEffect), typeDiscriminator: "DeferredRender")]
    [JsonDerivedType(typeof(RenderDeferredLiquidEffect), typeDiscriminator: "Water")]
    public class ShaderEffect
    {
        public int readerType { get; set; }
        public static ShaderEffect GetEffect(BinaryReader binaryReader, Header header)
        {
            int type = binaryReader.Read7BitEncodedInt();

            if (type == header.GetReaderIndex(ReaderType.RenderDeferred))
            {
                return new RenderDeferredEffect(binaryReader) { readerType = type };
            }
            else if (type == header.GetReaderIndex(ReaderType.AdditiveEffect))
            {
                return new AdditiveEffect(binaryReader) { readerType = type };
            }
            else if (type == header.GetReaderIndex(ReaderType.WaterEffect))
            {
                return new RenderDeferredLiquidEffect(binaryReader) { readerType = type };
            }
            else
            {
                return new LavaEffect(binaryReader) { readerType = type };
            }
            throw new ArgumentException();
        }
        public virtual void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(readerType);
        }
    }
}
