using MagickaForge.Components.Common;

namespace MagickaForge.Components.Levels
{
    public class BinTreeNode
    {
        public int PrimativeCount { get; set; }
        public int StartIndex { get; set; }
        public Vector3 BoundingBoxMin { get; set; }
        public Vector3 BoundingBoxMax { get; set; }
        public BinTreeNode ChildA { get; set; }
        public BinTreeNode ChildB { get; set; }
        public BinTreeNode() { }

        public BinTreeNode(BinaryReader binaryReader)
        {
            PrimativeCount = binaryReader.ReadInt32();
            StartIndex = binaryReader.ReadInt32();
            BoundingBoxMin = new Vector3(binaryReader);
            BoundingBoxMax = new Vector3(binaryReader);
            bool hasChildA = binaryReader.ReadBoolean();
            if (hasChildA)
            {
                ChildA = new BinTreeNode(binaryReader);
            }
            bool hasChildB = binaryReader.ReadBoolean();
            if (hasChildB)
            {
                ChildB = new BinTreeNode(binaryReader);
            }
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(PrimativeCount);
            binaryWriter.Write(StartIndex);
            BoundingBoxMin.Write(binaryWriter);
            BoundingBoxMax.Write(binaryWriter);

            var hasChildA = ChildA != null;
            var hasChildB = ChildB != null;

            binaryWriter.Write(hasChildA);
            if (hasChildA)
            {
                ChildA.Write(binaryWriter);
            }
            binaryWriter.Write(hasChildB);
            if (hasChildB)
            {
                ChildB.Write(binaryWriter);
            }
        }
    }
}
