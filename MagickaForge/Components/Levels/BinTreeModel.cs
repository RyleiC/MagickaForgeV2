using MagickaForge.Components.XNB;

namespace MagickaForge.Components.Levels
{
    public class BinTreeModel
    {
        private Header header;
        public BinTreeRoot[] BinaryTreeRoots { get; set; }

        public BinTreeModel(BinaryReader binaryReader, Header header)
        {
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
