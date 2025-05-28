using MagickaForge.Components.Graphics.Models;
using MagickaForge.Components.XNB;

namespace MagickaForge.Pipeline.Json.Models
{
    public class NonEmbeddedModel : PipelineJsonObject
    {
        public int ReaderIndex { get; set; }
        public DynamicHeader Header { get; set; }
        public Model Model { get; set; }
        public SharedContentCache[] SharedContent { get; set; }

        protected override void MidImport(BinaryReader binaryReader)
        {
            Header = new DynamicHeader(binaryReader);
            Model = new Model(binaryReader);
            SharedContent = new SharedContentCache[Header.SharedResources];
            for (var i = 0; i < SharedContent.Length; i++)
            {
                SharedContent[i] = new SharedContentCache(binaryReader, Header);
            }
        }

        protected override void MidExport(BinaryWriter binaryWriter)
        {
            Header!.Write(binaryWriter);
            Model!.Write(binaryWriter);
            for (var i = 0; i < SharedContent!.Length; i++)
            {
                SharedContent[i].Write(binaryWriter);
            }
        }
    }
}
