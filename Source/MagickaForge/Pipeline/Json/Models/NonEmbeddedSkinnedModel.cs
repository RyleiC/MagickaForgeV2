using MagickaForge.Components.Graphics.Models.Skinned;
using MagickaForge.Components.XNB;

namespace MagickaForge.Pipeline.Json.Models
{
    public class NonEmbeddedSkinnedModel : PipelineJsonObject
    {
        public DynamicHeader Header { get; set; }
        public SkinnedModel SkinnedModel { get; set; }
        public SharedContentCache[] SharedContent { get; set; }

        protected override void MidImport(BinaryReader binaryReader)
        {
            Header = new DynamicHeader(binaryReader);
            SkinnedModel = new SkinnedModel(binaryReader);
            SharedContent = new SharedContentCache[Header.SharedResources - (SkinnedModel.SharedClipReferences.Length + SkinnedModel.SharedBoneReferences.Length)];
            for (var i = 0; i < SharedContent.Length; i++)
            {
                SharedContent[i] = new SharedContentCache(binaryReader, Header);
            }
            SkinnedModel.ReadXNAnimationContent(binaryReader);
        }

        protected override void MidExport(BinaryWriter binaryWriter)
        {
            Header!.Write(binaryWriter);
            SkinnedModel!.Write(binaryWriter);
            foreach (SharedContentCache content in SharedContent)
            {
                content.Write(binaryWriter);
            }
            SkinnedModel?.WriteXNAnimationContent(binaryWriter);
        }
    }
}
