using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels
{
    public class TriangleMesh
    {
        public int readerType { get; set; }
        public Vector3[] vertices { get; set; }
        public int[] indices { get; set; }
        public TriangleMesh() { }
        public TriangleMesh(BinaryReader binaryReader)
        {
            readerType = binaryReader.Read7BitEncodedInt();
            vertices = new Vector3[binaryReader.ReadInt32()];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(binaryReader);
            }
            indices = new int[binaryReader.ReadInt32() * 3];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = binaryReader.ReadInt32();
            }
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(readerType);
            binaryWriter.Write(vertices.Length);
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Write(binaryWriter);
            }
            binaryWriter.Write(indices.Length / 3);
            for (int i = 0; i < indices.Length; i++)
            {
                binaryWriter.Write(indices[i]);
            }
        }
    }
}
