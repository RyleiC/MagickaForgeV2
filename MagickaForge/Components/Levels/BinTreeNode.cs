using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels
{
    public class BinTreeNode
    {
        public int primativeCount { get; set; }
        public int startIndex { get; set; }
        public Vector3 minBounding { get; set; }
        public Vector3 maxBounding { get; set; }
        public BinTreeNode childA { get; set; }
        public BinTreeNode childB { get; set; }
        public BinTreeNode() { }

        public BinTreeNode(BinaryReader binaryReader)
        {
            primativeCount = binaryReader.ReadInt32();

            startIndex = binaryReader.ReadInt32();
            minBounding = new Vector3(binaryReader);
            maxBounding = new Vector3(binaryReader);
            bool hasChildA = binaryReader.ReadBoolean();
            if (hasChildA)
            {
                childA = new BinTreeNode(binaryReader);
            }
            bool hasChildB = binaryReader.ReadBoolean();
            if (hasChildB)
            {
                childB = new BinTreeNode(binaryReader);
            }
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(primativeCount);
            binaryWriter.Write(startIndex);
            minBounding.Write(binaryWriter);
            maxBounding.Write(binaryWriter);

            var hasChildA = childA != null;
            var hasChildB = childB != null;

            binaryWriter.Write(hasChildA);
            if (hasChildA)
            {
                childA.Write(binaryWriter);
            }
            binaryWriter.Write(hasChildB);
            if (hasChildB)
            {
                childB.Write(binaryWriter);
            }
        }
    }
}
