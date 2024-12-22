using MagickaForge.Utils.Helpers;

namespace MagickaForge.Components.XNB
{
    public class Header
    {
        public ReaderCache[] Readers { get; set; }

        private Dictionary<ReaderType, int> _readerTypes;

        public int SharedResources { get; set; }
        public Header() { }

        public Header(BinaryReader binaryReader)
        {
            binaryReader.BaseStream.Position += XNBHelper.XNBHeader.Length + 4;

            _readerTypes = new Dictionary<ReaderType, int>();
            Readers = new ReaderCache[binaryReader.Read7BitEncodedInt()];
            for (int i = 0; i < Readers.Length; i++)
            {
                Readers[i] = new ReaderCache(binaryReader);
            }
            SharedResources = binaryReader.Read7BitEncodedInt();

            for (int i = 0; i < Readers.Length; i++)
            {
                for (int x = 0; x < 13; x++)
                {
                    if ((ReaderType)x == Readers[i].Type)
                    {
                        _readerTypes.Add(Readers[i].Type, i + 1);
                    }
                }
            }
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(XNBHelper.XNBHeader);
            binaryWriter.Write(0); //Placeholder file size
            binaryWriter.Write7BitEncodedInt(Readers.Length);
            foreach (var reader in Readers)
            {
                binaryWriter.Write(reader.Cache!);
                binaryWriter.Write(reader.Version);
            }
            binaryWriter.Write7BitEncodedInt(SharedResources);
        }

        public int GetReaderIndex(ReaderType readerType)
        {
            if (_readerTypes.ContainsKey(readerType))
            {
                return _readerTypes[readerType];
            }
            return -1;
        }

    }
}
