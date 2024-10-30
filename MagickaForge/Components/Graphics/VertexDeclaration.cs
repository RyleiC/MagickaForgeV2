namespace MagickaForge.Components.Graphics
{
    public class VertexDeclaration
    {
        public VertexElement[] _vertexElements { get; set; }
        public int readerIndex { get; set; }
        public VertexDeclaration() { }
        public VertexDeclaration(BinaryReader binaryReader)
        {
            readerIndex = binaryReader.Read7BitEncodedInt();
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
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(readerIndex);
            binaryWriter.Write(_vertexElements.Length);
            foreach (VertexElement element in _vertexElements)
            {
                binaryWriter.Write(element.Stream);
                binaryWriter.Write(element.Offset);
                binaryWriter.Write(element.Format);
                binaryWriter.Write(element.Method);
                binaryWriter.Write(element.Usage);
                binaryWriter.Write(element.UsageIndex);
            }
        }
    }
}
