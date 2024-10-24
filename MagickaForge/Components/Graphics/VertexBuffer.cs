namespace MagickaForge.Components.Graphics
{
    public class VertexBuffer
    {
        private byte[] _data;

        public VertexBuffer(BinaryReader binaryReader)
        {
            binaryReader.ReadByte();
            var count = binaryReader.ReadInt32();
            //Console.WriteLine(count);
            _data = binaryReader.ReadBytes(count);
        }
    }
}
