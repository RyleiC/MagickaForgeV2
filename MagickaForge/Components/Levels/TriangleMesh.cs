using MagickaForge.Components.Common;

namespace MagickaForge.Components.Levels
{
    public class TriangleMesh
    {
        public int ReaderType { get; set; }
        public Vector3[] Vertices { get; set; }
        public int[] Indices { get; set; }
        public TriangleMesh() { }
        public TriangleMesh(BinaryReader binaryReader)
        {
            ReaderType = binaryReader.Read7BitEncodedInt();
            Vertices = new Vector3[binaryReader.ReadInt32()];
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = new Vector3(binaryReader);
            }
            Indices = new int[binaryReader.ReadInt32() * 3];
            for (int i = 0; i < Indices.Length; i++)
            {
                Indices[i] = binaryReader.ReadInt32();
            }
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(ReaderType);
            binaryWriter.Write(Vertices.Length);
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Write(binaryWriter);
            }
            binaryWriter.Write(Indices.Length / 3);
            for (int i = 0; i < Indices.Length; i++)
            {
                binaryWriter.Write(Indices[i]);
            }
        }
    }
}
