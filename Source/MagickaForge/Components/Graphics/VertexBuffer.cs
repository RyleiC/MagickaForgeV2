namespace MagickaForge.Components.Graphics
{
    public class VertexBuffer
    {
        public int ReaderIndex { get; set; }
        public byte[] Data { get; set; }
        public VertexBuffer() { }
        public VertexBuffer(BinaryReader binaryReader)
        {
            ReaderIndex = binaryReader.Read7BitEncodedInt();
            var count = binaryReader.ReadInt32();
            Data = binaryReader.ReadBytes(count);
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(ReaderIndex);
            binaryWriter.Write(Data.Length);
            binaryWriter.Write(Data);
        }
    }
}
