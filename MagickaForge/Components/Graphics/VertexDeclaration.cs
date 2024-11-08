using MagickaForge.Utils.Definitions.Graphics;
namespace MagickaForge.Components.Graphics
{
    public class VertexDeclaration
    {
        public VertexElement[] VertexElements { get; set; }
        public int ReaderIndex { get; set; }
        public VertexDeclaration() { }
        public VertexDeclaration(BinaryReader binaryReader)
        {
            ReaderIndex = binaryReader.Read7BitEncodedInt();
            var count = binaryReader.ReadInt32();
            VertexElements = new VertexElement[count];
            for (int i = 0; i < count; i++)
            {
                VertexElements[i] = new VertexElement()
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
            binaryWriter.Write7BitEncodedInt(ReaderIndex);
            binaryWriter.Write(VertexElements.Length);
            foreach (VertexElement element in VertexElements)
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
