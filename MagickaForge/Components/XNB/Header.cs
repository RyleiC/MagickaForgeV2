using MagickaForge.Utils.Helpers;

namespace MagickaForge.Components.XNB
{
    public class Header
    {
        public ReaderCache[] Readers { get; set; }
        public int SharedResources { get; set; }
        public Header() { }

        public Header(BinaryReader binaryReader)
        {
            binaryReader.BaseStream.Position += XNBHelper.XNBHeader.Length + 4;
            Readers = new ReaderCache[binaryReader.Read7BitEncodedInt()];
            for (int i = 0; i < Readers.Length; i++)
            {
                Readers[i] = new ReaderCache(binaryReader);
            }
            SharedResources = binaryReader.Read7BitEncodedInt();
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(XNBHelper.XNBHeader);
            binaryWriter.Write(0); //Placeholder file size
            binaryWriter.Write7BitEncodedInt(Readers.Length);
            foreach (var reader in Readers)
            {
                binaryWriter.Write(reader.ReaderName!);
                binaryWriter.Write(reader.Version);
            }
            binaryWriter.Write7BitEncodedInt(SharedResources);
        }

        public ReaderType GetReaderType(int readerIndex)
        {
            return Readers[readerIndex - 1].Type;
        }

    }
}
