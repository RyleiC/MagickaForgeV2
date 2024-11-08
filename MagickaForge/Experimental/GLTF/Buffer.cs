using MagickaForge.Components.Graphics;
using MagickaForge.Components.Levels;
using MagickaForge.Utils.Structures;

namespace MagickaForge.Experimental.GLTF
{
    public class Buffer
    {
        private Vector3[] _vertices;
        private Vector3[] _normals;
        private Vector2[] _textureCoordinates;
        private Vector3[] _tangent;
        private short[] _indices;

        public void Read(BinaryReader binaryReader, BufferViewNode[] bufferViews)
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

        /*
         *  This needs modification.
         *  As it stands, the right hand & left hand coordinate systems are fighting
         *  
         *  If I use the same Z coordinates the player will fall through surfaces as if they were solid on the inside but closed on the outside
         *  If I use the corrected -Z coordinates for one then now the collisions are mirrored!
         *  
         *  This needs some studying under JigLibX to see how to resolve this, until then.
         *  ~ Rylei C.
         */
        public TriangleMesh ToTriangleMesh()
        {
            var mesh = new TriangleMesh();
            mesh.Vertices = _vertices;
            for (var i = 0; i < _vertices.Length; i++)
            {
                mesh.Vertices[i] = _vertices[i];
            }
            mesh.Indices = new int[_indices.Length];
            for (var i = 0; i < _indices.Length; i++)
            {
                mesh.Indices[i] = _indices[i];
            }
            return mesh;
        }

        public VertexBuffer ToVertexBuffer()
        {
            var buffer = new VertexBuffer();
            buffer.Data = new byte[_vertices.Length * 36 + _textureCoordinates.Length * 8];
            var stream = new MemoryStream(buffer.Data);

            var binaryWriter = new BinaryWriter(stream);
            for (var i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].Write(binaryWriter);
                _normals[i].Write(binaryWriter);
                _textureCoordinates[i].Write(binaryWriter);
                _tangent[i].Write(binaryWriter);
            }
            binaryWriter.Close();

            return buffer;
        }

        public IndexBuffer ToIndexBuffer()
        {
            var buffer = new IndexBuffer()
            {
                Is16Bit = true
            };
            buffer.Data = new byte[_indices.Length * 2];
            var stream = new MemoryStream(buffer.Data);

            var binaryWriter = new BinaryWriter(stream);
            for (var i = 0; i < _indices.Length; i++)
            {
                binaryWriter.Write(_indices[i]);
            }
            binaryWriter.Close();

            return buffer;
        }

        public int IndiceCount
        {
            get
            {
                return _indices.Length;
            }
        }

        public int VertexCount
        {
            get
            {
                return _vertices.Length;
            }
        }

    }
}
