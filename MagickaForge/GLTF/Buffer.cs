using MagickaForge.Utils.Structures;

namespace MagickaForge.GLTF
{
    public class Buffer
    {
        private Vector3[] Vertices;
        private Vector3[] Normals;
        private Vector2[] TextureCoordinates;
        private Vector3[] Tangent;
        private short[] Indexes;
        //private int TotalLength;
        public string uri { get; set; }

        public Buffer() { }

        public void Read(BinaryReader binaryReader, BufferView[] bufferViews)
        {
            Vertices = new Vector3[bufferViews[0].byteLength / 12];
            Normals = new Vector3[Vertices.Length];
            TextureCoordinates = new Vector2[Vertices.Length];
            Tangent = new Vector3[Vertices.Length];
            Indexes = new short[bufferViews[4].byteLength / 2];

            for (var i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = new Vector3(binaryReader);
            }
            for (var i = 0; i < Normals.Length; i++)
            {
                Normals[i] = new Vector3(binaryReader);
            }
            for (var i = 0; i < TextureCoordinates.Length; i++)
            {
                TextureCoordinates[i] = new Vector2(binaryReader);
            }
            for (var i = 0; i < Tangent.Length; i++)
            {
                Tangent[i] = new Vector3(binaryReader);
                binaryReader.ReadSingle();
            }
            for (var i = 0; i < Indexes.Length; i++)
            {
                Indexes[i] = binaryReader.ReadInt16();
            }

        }
        public void ToVertexBuffer(string outputPath)
        {
            BinaryWriter bw = new BinaryWriter(File.Create(outputPath));
            for (var i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Write(bw);
                Normals[i].Write(bw);
                TextureCoordinates[i].Write(bw);
                Tangent[i].Write(bw);
            }
            bw.Close();
        }
        public void ToIndexBuffer(string outputPath)
        {
            BinaryWriter bw = new BinaryWriter(File.Create(outputPath));
            for (var i = 0; i < Indexes.Length; i++)
            {
                bw.Write(Indexes[i]);
            }
            bw.Close();
        }

    }
}
