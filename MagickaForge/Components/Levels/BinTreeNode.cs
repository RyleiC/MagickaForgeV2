using MagickaForge.Components.Animations;
using MagickaForge.Components.Graphics.Effects;
using MagickaForge.Components.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Levels
{
    public class BinTreeNode
    {
        private int _primativeCount;
        private int _startIndex;
        private float[] _minBounding;
        private float[] _maxBounding;
        private BinTreeNode _childA;
        private BinTreeNode _childB;

        public BinTreeNode(BinaryReader binaryReader)
        {
            _primativeCount = binaryReader.ReadInt32();

            _startIndex = binaryReader.ReadInt32();
            _minBounding = new float[3];
            for (int i = 0; i < _minBounding.Length; i++)
            {
                _minBounding[i] = binaryReader.ReadSingle();
            }
            _maxBounding = new float[3];
            for (int i = 0; i < _maxBounding.Length; i++)
            {
                _maxBounding[i] = binaryReader.ReadSingle();
            }
            bool hasChildA = binaryReader.ReadBoolean();
            if (hasChildA)
            {
                _childA = new BinTreeNode(binaryReader);
            }
            bool hasChildB = binaryReader.ReadBoolean();
            if (hasChildB)
            {
                _childB = new BinTreeNode(binaryReader);
            }
        }
    }
}
