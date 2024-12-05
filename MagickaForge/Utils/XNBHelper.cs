namespace MagickaForge.Utils
{
    public static class XNBHelper
    {
        public static readonly byte[] XNBHeader =
        {
            0x58, //X
            0x4E, //N
            0x42, //B
            0x77, //w
            0x04, //3.1
            0x00 //Decompressed Flag
        };

        public static void WriteFileSize(BinaryWriter binaryWriter)
        {
            var fileSize = (int)binaryWriter.BaseStream.Position;
            binaryWriter.BaseStream.Position = XNBHeader.Length;
            binaryWriter.Write(fileSize);
        }
    }
}
