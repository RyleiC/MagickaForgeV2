using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Graphics
{
    public struct VertexElement
    {
        public short Stream;
        public short Offset;

        public byte Format;
        public byte Method;
        public byte Usage;

        public byte UsageIndex;
    }
}
