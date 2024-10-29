using MagickaForge.Components.Graphics.Effects;
using MagickaForge.Components.XNB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Graphics.Models
{
    public class ModelMeshPart
    {
        public int streamOffset { get; set; }
        public int baseVertex { get; set; }
        public int numVertices { get; set; }
        public int startIndex { get; set; }
        public int primativeCount { get; set; }
        public int vdIndex { get; set; }
        public ShaderEffect effect { get; set; }
        public ModelMeshPart() { }
        public ModelMeshPart(BinaryReader binaryReader, VertexDeclaration[] vertexDeclarations)
        {
            int streamOffset = binaryReader.ReadInt32();
            int baseVertex = binaryReader.ReadInt32();
            int numVertices = binaryReader.ReadInt32();
            int startIndex = binaryReader.ReadInt32();
            int primitiveCount = binaryReader.ReadInt32();
            vdIndex = binaryReader.ReadInt32();
            object obj = binaryReader.ReadByte();
            binaryReader.ReadByte();
     
        }
    }
}
