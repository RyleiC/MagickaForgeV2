using MagickaForge.Components.Graphics;
using MagickaForge.Components.Graphics.Effects;
using MagickaForge.Components.XNB;
using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels
{
    public class BinTreeRoot
    {
        public bool _visible { get; set; }
        public bool _castShadows { get; set; }
        public float _sway { get; set; }
        public float _entityInfluence { get; set; }
        public float _groundLevel { get; set; }
        public int _vertexCount { get; set; }
        public int _vertexStride { get; set; }

        public VertexDeclaration _vertexDeclaration { get; set; }
        public VertexBuffer _vertexBuffer { get; set; }
        public IndexBuffer _indexBuffer { get; set; }
        public ShaderEffect _effect { get; set; }

        public int _primativeCount { get; set; }
        public int _startIndex { get; set; }
        public Vector3 _minBounding { get; set; }
        public Vector3 _maxBounding { get; set; }

        public BinTreeNode? _childA { get; set; }
        public BinTreeNode? _childB { get; set; }
        public BinTreeRoot() { }
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
        public void Write(BinaryWriter binaryWriter, Header header)
        {
            binaryWriter.Write(_visible);
            binaryWriter.Write(_castShadows);
            binaryWriter.Write(_sway);
            binaryWriter.Write(_entityInfluence);
            binaryWriter.Write(_groundLevel);
            binaryWriter.Write(_vertexCount);
            binaryWriter.Write(_vertexStride);

            _vertexDeclaration.Write(binaryWriter);
            _vertexBuffer.Write(binaryWriter);
            _indexBuffer.Write(binaryWriter);
            _effect.Write(binaryWriter);
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
