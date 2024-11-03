using MagickaForge.Utils.Structures;

namespace MagickaForge.GLTF
{
    public class Buffer
    {
        private Vector3[] _vertices;
        private Vector3[] _normals;
        private Vector2[] _textureCoordinates;
        private Vector3[] _tangent;
        private short[] _indices;

        public Buffer() { }

        public void Read(BinaryReader binaryReader, BufferView[] bufferViews)
        {
            _vertices = new Vector3[bufferViews[0].byteLength / 12];
            _normals = new Vector3[_vertices.Length];
            _textureCoordinates = new Vector2[_vertices.Length];
            _tangent = new Vector3[_vertices.Length];
            _indices = new short[bufferViews[4].byteLength / 2];

            for (var i = 0; i < _vertices.Length; i++)
            {
                _vertices[i] = new Vector3(binaryReader);
            }
            for (var i = 0; i < _normals.Length; i++)
            {
                _normals[i] = new Vector3(binaryReader);
            }
            for (var i = 0; i < _textureCoordinates.Length; i++)
            {
                _textureCoordinates[i] = new Vector2(binaryReader);
            }
            for (var i = 0; i < _tangent.Length; i++)
            {
                _tangent[i] = new Vector3(binaryReader);
                binaryReader.ReadSingle();
            }
            for (var i = 0; i < _indices.Length; i++)
            {
                _indices[i] = binaryReader.ReadInt16();
            }

        }
        public void ToVertexBuffer(string outputPath)
        {
            BinaryWriter bw = new BinaryWriter(File.Create(outputPath));
            for (var i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].Write(bw);
                _normals[i].Write(bw);
                _textureCoordinates[i].Write(bw);
                _tangent[i].Write(bw);
            }
            bw.Close();
        }
        public void ToIndexBuffer(string outputPath)
        {
            BinaryWriter bw = new BinaryWriter(File.Create(outputPath));
            for (var i = 0; i < _indices.Length; i++)
            {
                bw.Write(_indices[i]);
            }
            bw.Close();
        }

    }
}
