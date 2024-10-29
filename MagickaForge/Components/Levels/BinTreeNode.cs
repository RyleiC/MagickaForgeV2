using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels
{
    public class BinTreeNode
    {
        public int _primativeCount { get; set; }
        public int _startIndex { get; set; }
        public Vector3 _minBounding { get; set; }
        public Vector3 _maxBounding { get; set; }
        public BinTreeNode _childA { get; set; }
        public BinTreeNode _childB { get; set; }
        public BinTreeNode() { }

        public BinTreeNode(BinaryReader binaryReader)
        {
            _primativeCount = binaryReader.ReadInt32();

            _startIndex = binaryReader.ReadInt32();
            _minBounding = new Vector3(binaryReader);
            _maxBounding = new Vector3(binaryReader);
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
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(_primativeCount);
            binaryWriter.Write(_startIndex);
            _minBounding.Write(binaryWriter);
            _maxBounding.Write(binaryWriter);

            var hasChildA = _childA != null;
            var hasChildB = _childB != null;

            binaryWriter.Write(hasChildA);
            if (hasChildA)
            {
                _childA.Write(binaryWriter);
            }
            binaryWriter.Write(hasChildB);
            if (hasChildB)
            {
                _childB.Write(binaryWriter);
            }
        }
    }
}
