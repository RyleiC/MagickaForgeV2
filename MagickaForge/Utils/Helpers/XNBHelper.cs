using XNBDecomp;

namespace MagickaForge.Utils.Helpers
{
    public static class XNBHelper
    {
        public static readonly byte[] XNBHeader =
        [
            0x58, //X
            0x4E, //N
            0x42, //B
            0x77, //w
            0x04, //3.1
            0x00 //Decompressed Flag
        ];

        public static void WriteFileSize(BinaryWriter binaryWriter)
        {
            var fileSize = (int)binaryWriter.BaseStream.Position;
            binaryWriter.BaseStream.Position = XNBHeader.Length;
            binaryWriter.Write(fileSize);
        }

        public static Stream DecompressXNB(string path)
        {
            var stream = new MemoryStream();
            var contentReader = ContentReader.Create(path);

            var contentWriter = new ContentWriter(stream, false, contentReader.fileVersion, contentReader.graphicsProfile);
            for (int i = 0; i < contentReader.fileSize; i++)
            {
                contentWriter.Write(contentReader.ReadByte());
            }

            contentReader.Close();
            contentWriter.FlushOutput();
            contentWriter.Close();

            stream.Position = 0;

            return stream;
        }
    }
}
