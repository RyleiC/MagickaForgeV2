using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Graphics
{
    public class VertexBuffer
    {
        private byte[] _data;

        public VertexBuffer(BinaryReader binaryReader)
        {
            var count = binaryReader.ReadInt32();
            //Console.WriteLine(count);
            _data = binaryReader.ReadBytes(count);
        }
    }
}
