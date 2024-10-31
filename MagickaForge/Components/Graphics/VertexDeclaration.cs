using MagickaForge.Utils.Definitions.Graphics;
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
                    Format = (VertexElementFormat)binaryReader.ReadByte(),
                    Method = (VertexElementMethod)binaryReader.ReadByte(),
                    Usage = (VertexElementUsage)binaryReader.ReadByte(),
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
                binaryWriter.Write((byte)element.Format);
                binaryWriter.Write((byte)element.Method);
                binaryWriter.Write((byte)element.Usage);
                binaryWriter.Write(element.UsageIndex);
            }
        }
    }
}
