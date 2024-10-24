using MagickaForge.Components.Graphics;
using MagickaForge.Components.Graphics.Effects;
using MagickaForge.Components.XNB;

namespace MagickaForge.Components.Levels
{
    public class BinTreeRoot
    {
        private bool _visible;
        private bool _castShadows;
        private float _sway;
        private float _entityInfluence;
        private float _groundLevel;
        private int _vertexCount;
        private int _vertexStride;

        private VertexDeclaration _vertexDeclaration;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private ShaderEffect _effect;

        private int _primativeCount;
        private int _startIndex;
        private float[] _minBounding;
        private float[] _maxBounding;

        private BinTreeNode _childA;
        private BinTreeNode _childB;

        public BinTreeRoot(BinaryReader binaryReader, Header header)
        {
            _visible = binaryReader.ReadBoolean();
            _castShadows = binaryReader.ReadBoolean();
            _sway = binaryReader.ReadSingle();
            _entityInfluence = binaryReader.ReadSingle();
            _groundLevel = binaryReader.ReadSingle();
            _vertexCount = binaryReader.ReadInt32();
            _vertexStride = binaryReader.ReadInt32();

            _vertexDeclaration = new VertexDeclaration(binaryReader);
            _vertexBuffer = new VertexBuffer(binaryReader);
            _indexBuffer = new IndexBuffer(binaryReader);
            _effect = ShaderEffect.GetEffect(binaryReader, header);

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
        public void Write(BinaryWriter binaryWriter, Header header)
        {
            binaryWriter.Write(_visible);
            binaryWriter.Write(_castShadows);
            binaryWriter.Write(_sway);
            binaryWriter.Write(_entityInfluence);
            binaryWriter.Write(_groundLevel);
            binaryWriter.Write(_vertexCount);
            binaryWriter.Write(_vertexStride);
      /*      binaryWriter.Write(header.GetReaderIndex(ReaderType.VertexDeclaration));
            binaryWriter.Write(header.GetReaderIndex(ReaderType.VertexBuffer));
            binaryWriter.Write(header.GetReaderIndex(ReaderType.IndexBuffer));*/
        }
    }
}
