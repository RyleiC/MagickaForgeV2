using MagickaForge.Components.Graphics.Models;
using MagickaForge.Components.XNB;
using MagickaForge.Utils.Helpers;

namespace MagickaForge.Pipeline.Models
{
    public class NonEmbeddedModel : PipelineObject
    {
        public int ReaderIndex { get; set; }
        public Header? Header { get; set; }
        public Model Model { get; set; }
        public SharedContentCache[]? SharedContent { get; set; }

        public override void ReadFromXNB(string inputPath)
        {
            BinaryReader br = new(XNBHelper.DecompressXNB(inputPath));

            Header = new Header(br);
            Model = new Model(br);
            SharedContent = new SharedContentCache[Header.SharedResources];
            for (var i = 0; i < SharedContent.Length; i++)
            {
                SharedContent[i] = new SharedContentCache(br, Header);
            }
        }

        public override void WriteToXNB(string outputPath)
        {
            var binaryWriter = new BinaryWriter(File.Create(outputPath));

            Header!.Write(binaryWriter);
            Model!.Write(binaryWriter);
            for (var i = 0; i < SharedContent!.Length; i++)
            {
                SharedContent[i].Write(binaryWriter);
            }
            XNBHelper.WriteFileSize(binaryWriter);
            binaryWriter.Close();
        }
    }
}
