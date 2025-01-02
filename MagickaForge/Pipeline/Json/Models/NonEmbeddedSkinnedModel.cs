using MagickaForge.Components.Graphics.Models.Skinned;
using MagickaForge.Components.XNB;
using MagickaForge.Utils.Helpers;

namespace MagickaForge.Pipeline.Json.Models
{
    public class NonEmbeddedSkinnedModel : PipelineJsonObject
    {
        public Header? Header { get; set; }
        public SkinnedModel SkinnedModel { get; set; }
        public SharedContentCache[]? SharedContent { get; set; }

        public override void Import(string inputPath)
        {
            using (var binaryReader = new BinaryReader(XNBHelper.DecompressXNB(inputPath)))
            {
                Header = new Header(binaryReader);
                SkinnedModel = new SkinnedModel(binaryReader);
                SharedContent = new SharedContentCache[Header.SharedResources - (SkinnedModel.SharedClipReferences.Length + SkinnedModel.SharedBoneReferences.Length)];
                for (var i = 0; i < SharedContent.Length; i++)
                {
                    SharedContent[i] = new SharedContentCache(binaryReader, Header);
                }
                SkinnedModel.ReadXNAnimationContent(binaryReader);
            };
        }

        public override void Export(string outputPath)
        {
            using (var binaryWriter = new BinaryWriter(File.Create(outputPath)))
            {
                Header!.Write(binaryWriter);
                SkinnedModel!.Write(binaryWriter);
                foreach (SharedContentCache content in SharedContent)
                {
                    content.Write(binaryWriter);
                }
                SkinnedModel?.WriteXNAnimationContent(binaryWriter);
                XNBHelper.WriteFileSize(binaryWriter);
            };
        }
    }
}
