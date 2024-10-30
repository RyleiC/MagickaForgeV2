using MagickaForge.Components.Graphics.Effects;

namespace MagickaForge.Components.XNB
{
    public class Header
    {
        private readonly byte[] Head = [0x58, 0x4E, 0x42, 0x77, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00];
        public ReaderCache[] readers { get; set; }
        private Dictionary<ReaderType, int> readerTypes;
        public int sharedResources { get; set; }

        public Header() { }
        public Header(BinaryReader binaryReader)
        {
            binaryReader.ReadBytes(Head.Length);
            readerTypes = new Dictionary<ReaderType, int>();

            readers = new ReaderCache[binaryReader.Read7BitEncodedInt()];
            for (int i = 0; i < readers.Length; i++)
            {
                readers[i] = new ReaderCache(binaryReader);
            }
            sharedResources = binaryReader.Read7BitEncodedInt();

            for (int i = 0; i < readers.Length; i++)
            {
                for (int x = 0; x < 13; x++)
                {
                    if ((ReaderType)x == readers[i].Type)
                    {
                        readerTypes.Add(readers[i].Type, i + 1);
                    }
                }
            } //Head ache..
        }

        public void GiveSharedResources(int index)
        {
            ShaderEffect[] shaderEffects = new ShaderEffect[sharedResources];
            for (int i = 0; index < sharedResources; i++)
            {

            }
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Head);
            binaryWriter.Write7BitEncodedInt(readers.Length);
            foreach (var reader in readers)
            {
                binaryWriter.Write(reader.Cache!);
                binaryWriter.Write(reader.Version);
            }
            binaryWriter.Write7BitEncodedInt(sharedResources);
        }

        public int GetReaderIndex(ReaderType readerType)
        {
            if (readerTypes.ContainsKey(readerType))
            {
                return readerTypes[readerType];
            }
            return -1;
        }

        public ReaderType GetReaderType(int value)
        {
            return readers[value].Type;
        }
    }
}
