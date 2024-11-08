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
        public int ReaderType { get; set; }
        public static ShaderEffect GetEffect(BinaryReader binaryReader, Header header)
        {
            int type = binaryReader.Read7BitEncodedInt();

            if (type == header.GetReaderIndex(XNB.ReaderType.RenderDeferred))
            {
                return new RenderDeferredEffect(binaryReader) { ReaderType = type };
            }
            else if (type == header.GetReaderIndex(XNB.ReaderType.AdditiveEffect))
            {
                return new AdditiveEffect(binaryReader) { ReaderType = type };
            }
            else if (type == header.GetReaderIndex(XNB.ReaderType.WaterEffect))
            {
                return new RenderDeferredLiquidEffect(binaryReader) { ReaderType = type };
            }
            else
            {
                return new LavaEffect(binaryReader) { ReaderType = type };
            }
            throw new ArgumentException();
        }
        public virtual void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(ReaderType);
        }
    }
}
