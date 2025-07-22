using System.Text.Json.Serialization;

namespace MagickaForge.Components.XNB
{
    public class ReaderCache
    {
        private readonly ReaderType _type;

        private const string RenderDeferred = "PolygonHead.Pipeline.RenderDeferredEffectReader, PolygonHead, Version=1.0.0.0, Culture=neutral";
        private const string RenderAdditive = "PolygonHead.Pipeline.AdditiveEffectReader, PolygonHead, Version=1.0.0.0, Culture=neutral";
        private const string RenderDeferredLiquid = "PolygonHead.Pipeline.RenderDeferredLiquidEffectReader, PolygonHead";
        private const string Lava = "PolygonHead.Pipeline.LavaEffectReader, PolygonHead, Version=1.0.0.0, Culture=neutral";
        private const string BasicSkinnedModel = "XNAnimation.Pipeline.SkinnedModelBasicEffectReader, XNAnimation, Version=0.7.0.0, Culture=neutral";

        public string ReaderName { get; set; }
        public int Version { get; set; }

        public ReaderCache() { }
        public ReaderCache(BinaryReader binaryReader)
        {
            ReaderName = binaryReader.ReadString();
            Version = binaryReader.ReadInt32();

            switch (ReaderName)
            {
                case RenderDeferred:
                    _type = ReaderType.RenderDeferred;
                    break;
                case RenderAdditive:
                    _type = ReaderType.AdditiveEffect;
                    break;
                case RenderDeferredLiquid:
                    _type = ReaderType.WaterEffect;
                    break;
                case Lava:
                    _type = ReaderType.LavaEffect;
                    break;
                case BasicSkinnedModel:
                    _type = ReaderType.BasicSkinned;
                    break;
            }
        }

        [JsonIgnore]
        public ReaderType Type
        {
            get
            {
                return _type;
            }
        }

    }
}
