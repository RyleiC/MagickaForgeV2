using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Graphics
{
    public class IndexBuffer
    {
        private bool _is16Bit;
        public byte[] _data;

        public IndexBuffer(BinaryReader binaryReader)
        {
            binaryReader.ReadByte();
            _is16Bit = binaryReader.ReadBoolean();
            var count = binaryReader.ReadInt32();
            _data = binaryReader.ReadBytes(count);
        }
    }
}
