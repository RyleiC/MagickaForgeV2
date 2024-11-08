using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels.Navigation
{
    public class NavigationMesh
    {
        public Vector3[] Vertices { get; set; }
        public NavigationTriangle[] NavigationTriangles { get; set; }
        public NavigationMesh() { }

        public NavigationMesh(BinaryReader binaryReader)
        {
            Vertices = new Vector3[binaryReader.ReadUInt16()];
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = Vertices[i] = new Vector3(binaryReader);
            }
            NavigationTriangles = new NavigationTriangle[binaryReader.ReadUInt16()];
            for (int i = 0; i < NavigationTriangles.Length; i++)
            {
                NavigationTriangles[i] = new NavigationTriangle(binaryReader);
            }

        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write((ushort)Vertices.Length);
            foreach (var vertex in Vertices)
            {
                vertex.Write(binaryWriter);
            }
            binaryWriter.Write((ushort)NavigationTriangles.Length);
            foreach (var navigationTriangles in NavigationTriangles)
            {
                navigationTriangles.Write(binaryWriter);
            }
        }
    }
}
