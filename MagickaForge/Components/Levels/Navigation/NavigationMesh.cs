using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels.Navigation
{
    public class NavigationMesh
    {
        public Vector3[] vertices { get; set; }
        public NavigationTriangle[] navigationTriangles { get; set; }
        public NavigationMesh() { }

        public NavigationMesh(BinaryReader binaryReader)
        {
            vertices = new Vector3[binaryReader.ReadUInt16()];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i] = new Vector3(binaryReader);
            }
            navigationTriangles = new NavigationTriangle[binaryReader.ReadUInt16()];
            for (int i = 0; i < navigationTriangles.Length; i++)
            {
                navigationTriangles[i] = new NavigationTriangle(binaryReader);
            }

        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write((ushort)vertices.Length);
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].Write(binaryWriter);
            }
            binaryWriter.Write((ushort)navigationTriangles.Length);
            for (var i = 0; i < navigationTriangles.Length; i++)
            {
                navigationTriangles[i].Write(binaryWriter);
            }
        }
    }
}
