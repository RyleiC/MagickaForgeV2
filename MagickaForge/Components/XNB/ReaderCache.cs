using System.Text.Json.Serialization;

namespace MagickaForge.Components.XNB
{
    public class ReaderCache
    {
        public string? Cache { get; set; }
        public int Version { get; set; }
        private ReaderType type;

        private const string LEVEL_MODEL = "Magicka.ContentReaders.LevelModelReader, Magicka";
        private const string BINARY_TREE = "PolygonHead.Pipeline.BiTreeModelReader, PolygonHead";
        private const string VERTEX_DECL = "Microsoft.Xna.Framework.Content.VertexDeclarationReader";
        private const string VERTEX_BUFF = "Microsoft.Xna.Framework.Content.VertexBufferReader";
        private const string INDEX_BUFF = "Microsoft.Xna.Framework.Content.IndexBufferReader";
        private const string RENDER_DEFERRED = "PolygonHead.Pipeline.RenderDeferredEffectReader, PolygonHead, Version=1.0.0.0, Culture=neutral";
        private const string RENDER_ADDITIVE = "PolygonHead.Pipeline.AdditiveEffectReader, PolygonHead, Version=1.0.0.0, Culture=neutral";
        private const string WATER = "PolygonHead.Pipeline.RenderDeferredLiquidEffectReader, PolygonHead";
        private const string LAVA = "PolygonHead.Pipeline.LavaEffectReader, PolygonHead, Version=1.0.0.0, Culture=neutral";
        private const string MODEL = "Microsoft.Xna.Framework.Content.ModelReader";
        private const string STRING = "Microsoft.Xna.Framework.Content.StringReader";
        private const string VECTOR3_LIST = "Microsoft.Xna.Framework.Content.ListReader`1[[Microsoft.Xna.Framework.Vector3, Microsoft.Xna.Framework, Version=3.1.0.0, Culture=neutral, PublicKeyToken=6d5c3888ef60e27d]]";
        private const string VECTOR3 = "Microsoft.Xna.Framework.Content.Vector3Reader";


        public ReaderCache(BinaryReader binaryReader)
        {
            Cache = binaryReader.ReadString();
            Version = binaryReader.ReadInt32();

            switch (Cache)
            {
                case (LEVEL_MODEL): type = ReaderType.Level; break;
                case (BINARY_TREE): type = ReaderType.BinaryTree; break;
                case (VERTEX_DECL): type = ReaderType.VertexDeclaration; break;
                case (VERTEX_BUFF): type = ReaderType.VertexBuffer; break;
                case (INDEX_BUFF): type = ReaderType.IndexBuffer; break;
                case (RENDER_DEFERRED): type = ReaderType.RenderDeferred; break;
                case (RENDER_ADDITIVE): type = ReaderType.AdditiveEffect; break;
                case (WATER): type = ReaderType.WaterEffect; break;
                case (LAVA): type = ReaderType.LavaEffect; break;
                case (MODEL): type = ReaderType.Model; break;
                case (STRING): type = ReaderType.String; break;
                case (VECTOR3): type = ReaderType.Vector3Reader; break;
                default: type = ReaderType.Vector3ListReader; break;
            }
        }

        [JsonIgnore]
        public ReaderType Type
        {
            get
            {
                return type;
            }
        }

    }
}
