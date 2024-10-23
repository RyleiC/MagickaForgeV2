using MagickaForge.Components.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Levels
{
    public class BinTreeModel
    {
        BinTreeRoot[] binaryTreeRoots;

        public BinTreeModel(BinaryReader binaryReader)
        {
            var count = binaryReader.ReadInt32();
            binaryTreeRoots = new BinTreeRoot[count];
            for (int i = 0; i < count; i++)
            {
                binaryTreeRoots[i] = new BinTreeRoot(binaryReader);
            }
        }
    }
}
