namespace MagickaForge.Components.Graphics
{
    public class IndexBuffer
    {
        public int readerIndex { get; set; }
        public bool _is16Bit { get; set; }
        public byte[] _data { get; set; }
        public IndexBuffer() { }
        public IndexBuffer(BinaryReader binaryReader)
        {
            readerIndex = binaryReader.Read7BitEncodedInt();
            _is16Bit = binaryReader.ReadBoolean();
            var count = binaryReader.ReadInt32();
            _data = binaryReader.ReadBytes(count);
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(readerIndex);
            binaryWriter.Write(_is16Bit);
            binaryWriter.Write(_data.Length);
            binaryWriter.Write(_data);
        }
    }
}
