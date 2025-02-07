namespace MagickaForge.Components.Graphics
{
    public class IndexBuffer
    {
        public int ReaderIndex { get; set; }
        public bool Is16Bit { get; set; }
        public byte[] Data { get; set; }
        public IndexBuffer() { }
        public IndexBuffer(BinaryReader binaryReader)
        {
            ReaderIndex = binaryReader.Read7BitEncodedInt();
            Is16Bit = binaryReader.ReadBoolean();
            var count = binaryReader.ReadInt32();
            Data = binaryReader.ReadBytes(count);
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(ReaderIndex);
            binaryWriter.Write(Is16Bit);
            binaryWriter.Write(Data.Length);
            binaryWriter.Write(Data);
        }
    }
}
