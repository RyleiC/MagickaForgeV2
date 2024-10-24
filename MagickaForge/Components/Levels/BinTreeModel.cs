using MagickaForge.Components.XNB;

namespace MagickaForge.Components.Levels
{
    public class BinTreeModel
    {
        Header header;
        BinTreeRoot[] binaryTreeRoots;

        public BinTreeModel(BinaryReader binaryReader, Header header)
        {
            this.header = header;

            var count = binaryReader.ReadInt32();
            binaryTreeRoots = new BinTreeRoot[count];
            for (int i = 0; i < count; i++)
            {
                binaryTreeRoots[i] = new BinTreeRoot(binaryReader, header);
            }
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(binaryTreeRoots.Length);
            foreach (var root in binaryTreeRoots)
            {
                root.Write(binaryWriter, header);
            }
        }
    }
}
