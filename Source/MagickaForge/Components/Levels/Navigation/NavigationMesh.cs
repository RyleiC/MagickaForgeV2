using MagickaForge.Components.Common;

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

        public static float CalculateTriangleDistance(Vector3 positionA, Vector3 positionB)
        {
            float x = positionB.X - positionA.X;
            float y = positionB.Y - positionA.Y;
            float z = positionB.Z - positionA.Z;

            return MathF.Abs(MathF.Sqrt(MathF.Pow(x, x) + MathF.Pow(y, y) + MathF.Pow(z, z)));
        }
    }
}
