using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Levels.Navigation
{
    public class NavigationMesh
    {
        Vector3[] vertices;
        NavigationTriangle[] navigationTriangles;


        public NavigationMesh(BinaryReader binaryReader)
        {
            vertices = new Vector3[binaryReader.ReadUInt16()];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i] = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
            }
            navigationTriangles = new NavigationTriangle[binaryReader.ReadUInt16()];
            for (int i = 0; i < navigationTriangles.Length; i++)
            {
                navigationTriangles[i] = new NavigationTriangle(binaryReader);
            }

        }
    }
}
