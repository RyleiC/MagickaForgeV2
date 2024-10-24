namespace MagickaForge.Components.Graphics
{
    public class VertexDeclaration
    {
        private VertexElement[] _vertexElements;

        public VertexDeclaration(BinaryReader binaryReader)
        {
            binaryReader.ReadByte();
            var count = binaryReader.ReadInt32();
            _vertexElements = new VertexElement[count];
            for (int i = 0; i < count; i++)
            {
                _vertexElements[i] = new VertexElement()
                {
                    Stream = binaryReader.ReadInt16(),
                    Offset = binaryReader.ReadInt16(),
                    Format = binaryReader.ReadByte(),
                    Method = binaryReader.ReadByte(),
                    Usage = binaryReader.ReadByte(),
                    UsageIndex = binaryReader.ReadByte(),
                };
            }
        }
    }
}
