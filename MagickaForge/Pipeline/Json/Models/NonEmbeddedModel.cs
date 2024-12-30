using MagickaForge.Components.Graphics.Models;
using MagickaForge.Components.XNB;
using MagickaForge.Utils.Helpers;

namespace MagickaForge.Pipeline.Json.Models
{
    public class NonEmbeddedModel : PipelineJsonObject
    {
        public int ReaderIndex { get; set; }
        public Header? Header { get; set; }
        public Model Model { get; set; }
        public SharedContentCache[]? SharedContent { get; set; }

        public override void Import(string inputPath)
        {
            using (var binaryReader = new BinaryReader(XNBHelper.DecompressXNB(inputPath)))
            {

                Header = new Header(binaryReader);
                Model = new Model(binaryReader);
                SharedContent = new SharedContentCache[Header.SharedResources];
                for (var i = 0; i < SharedContent.Length; i++)
                {
                    SharedContent[i] = new SharedContentCache(binaryReader, Header);
                }
            };
        }

        public override void Export(string outputPath)
        {
            using (var binaryWriter = new BinaryWriter(File.Create(outputPath)))
            {

                Header!.Write(binaryWriter);
                Model!.Write(binaryWriter);
                for (var i = 0; i < SharedContent!.Length; i++)
                {
                    SharedContent[i].Write(binaryWriter);
                }
                XNBHelper.WriteFileSize(binaryWriter);
            };
        }
    }
}
