using MagickaForge.Components.XNB;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Graphics.Effects
{

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "_EffectType")]
    [JsonDerivedType(typeof(AdditiveEffect), typeDiscriminator: "Additive")]
    [JsonDerivedType(typeof(LavaEffect), typeDiscriminator: "Lava")]
    [JsonDerivedType(typeof(RenderDeferredEffect), typeDiscriminator: "DeferredRender")]
    [JsonDerivedType(typeof(RenderDeferredLiquidEffect), typeDiscriminator: "Water")]
    [JsonDerivedType(typeof(BasicSkinnedEffect), typeDiscriminator: "SkinnedModel")]
    public abstract class ShaderEffect
    {
        public int ReaderType { get; set; }
        public static ShaderEffect GetEffect(BinaryReader binaryReader, Header header)
        {
            int type = binaryReader.Read7BitEncodedInt();
            var effectReader = header.GetReaderType(type);
            return header.GetReaderType(type) switch
            {
                (XNB.ReaderType.RenderDeferred) => new RenderDeferredEffect(binaryReader) { ReaderType = type },
                (XNB.ReaderType.AdditiveEffect) => new AdditiveEffect(binaryReader) { ReaderType = type },
                (XNB.ReaderType.WaterEffect) => new RenderDeferredLiquidEffect(binaryReader) { ReaderType = type },
                (XNB.ReaderType.LavaEffect) => new LavaEffect(binaryReader) { ReaderType = type },
                (XNB.ReaderType.BasicSkinned) => new BasicSkinnedEffect(binaryReader) { ReaderType = type },
                _ => throw new ArgumentException($"{type} does not point to the index of an effect reader!"),
            };
        }

        public virtual void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(ReaderType);
        }
    }
}
