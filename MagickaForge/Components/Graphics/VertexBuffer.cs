namespace MagickaForge.Components.Graphics
{
    public class VertexBuffer
    {
        public int readerIndex { get; set; }
        public byte[] _data { get; set; }
        public VertexBuffer() { }
        public VertexBuffer(BinaryReader binaryReader)
        {
            readerIndex = binaryReader.Read7BitEncodedInt();
            var count = binaryReader.ReadInt32();
            //Console.WriteLine(count);
            _data = binaryReader.ReadBytes(count);
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(readerIndex);
            binaryWriter.Write(_data.Length);
            binaryWriter.Write(_data);
        }
    }
}
