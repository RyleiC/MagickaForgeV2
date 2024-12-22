using MagickaForge.Components.XNB;

namespace MagickaForge.Components.Levels
{
    public class BinTreeModel
    {
        public int ReaderIndex { get; set; }
        public BinTreeRoot[] BinaryTreeRoots { get; set; }
        public BinTreeModel() { }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(ReaderIndex);
            binaryWriter.Write(BinaryTreeRoots.Length);
            for (int i = 0; i < BinaryTreeRoots.Length; i++)
            {
                BinaryTreeRoots[i].Write(binaryWriter);
            }
        }
        public BinTreeModel(BinaryReader binaryReader, Header header)
        {
            ReaderIndex = binaryReader.Read7BitEncodedInt();
            var count = binaryReader.ReadInt32();
            BinaryTreeRoots = new BinTreeRoot[count];
            for (int i = 0; i < count; i++)
            {
                BinaryTreeRoots[i] = new BinTreeRoot(binaryReader, header);
            }
        }
    }
}
