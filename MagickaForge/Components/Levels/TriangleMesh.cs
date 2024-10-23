using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Levels
{
    public class TriangleMesh
    {
        private Vector3[] vertices;
        private int[] indices;

        public TriangleMesh() { }
        public TriangleMesh(BinaryReader binaryReader)
        {
            binaryReader.ReadByte();
            vertices = new Vector3[binaryReader.ReadInt32()];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
            }
            indices = new int[binaryReader.ReadInt32() * 3];
            for (int i = 0;i < indices.Length; i++)
            {
                indices[i] = binaryReader.ReadInt32();
            }
        }
    }
}
