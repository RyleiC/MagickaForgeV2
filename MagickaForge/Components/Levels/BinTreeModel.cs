using MagickaForge.Components.XNB;

namespace MagickaForge.Components.Levels
{
    public class BinTreeModel
    {
        public int readerIndex { get; set; }
        private Header header;
        public BinTreeRoot[] BinaryTreeRoots { get; set; }
        public BinTreeModel() { }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(readerIndex);
            binaryWriter.Write(BinaryTreeRoots.Length);
            for (int i = 0; i < BinaryTreeRoots.Length; i++)
            {
                BinaryTreeRoots[i].Write(binaryWriter, header);
            }
        }
        public BinTreeModel(BinaryReader binaryReader, Header header)
        {
            readerIndex = binaryReader.Read7BitEncodedInt();
            this.header = header;

            var count = binaryReader.ReadInt32();
            BinaryTreeRoots = new BinTreeRoot[count];
            for (int i = 0; i < count; i++)
            {
                BinaryTreeRoots[i] = new BinTreeRoot(binaryReader, header);
            }
        }
    }
}
