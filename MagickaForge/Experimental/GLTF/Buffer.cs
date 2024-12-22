using MagickaForge.Components.Graphics;
using MagickaForge.Components.Levels;
using MagickaForge.Components.Levels.Navigation;
using MagickaForge.Utils.Data.AI;
using MagickaForge.Utils.Structures;

namespace MagickaForge.Experimental.GLTF
{
    public class Buffer
    {
        private Vector3[]? _vertices;
        private Vector3[]? _normals;
        private Vector2[]? _textureCoordinates;
        private Vector3[]? _tangent;
        private short[]? _indices;

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
            var buffer = new VertexBuffer
            {
                Data = new byte[_vertices.Length * 36 + _textureCoordinates.Length * 8]
            };
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
        public NavigationMesh ToNavigationMesh()
        {
            var mesh = new NavigationMesh
            {
                Vertices = _vertices,
                NavigationTriangles = new NavigationTriangle[IndiceCount / 3]
            };
            for (int i = 0; i < mesh.NavigationTriangles.Length; i++)
            {
                mesh.NavigationTriangles[i] = new NavigationTriangle()
                {
                    VertexA = (ushort)_indices[i],
                    VertexB = (ushort)_indices[i + 1],
                    VertexC = (ushort)_indices[i + 2],
                    CostAB = NavigationMesh.CalculateTriangleDistance(_vertices[_indices[i]], _vertices[_indices[i + 1]]),
                    CostBC = NavigationMesh.CalculateTriangleDistance(_vertices[_indices[i + 1]], _vertices[_indices[i + 2]]),
                    CostCA = NavigationMesh.CalculateTriangleDistance(_vertices[_indices[i]], _vertices[_indices[i + 2]]),
                    NeighborA = ushort.MaxValue,
                    NeighborB = ushort.MaxValue, //TEMP WHILE I FIND OTHER WAYS TO CALCULATE
                    NeighborC = ushort.MaxValue,
                    MovementProperty = MovementProperties.Default,
                };
            }
            return mesh;
        }

        public IndexBuffer ToIndexBuffer()
        {
            var buffer = new IndexBuffer
            {
                Is16Bit = true,
                Data = new byte[_indices.Length * 2]
            };
            var stream = new MemoryStream(buffer.Data);

            var binaryWriter = new BinaryWriter(stream); //DirectX is a left-handed system and the index order must be switched to 0 2 1
            for (var i = 0; i < _indices.Length; i += 3)
            {
                binaryWriter.Write(_indices[i]);
                binaryWriter.Write(_indices[i + 2]);
                binaryWriter.Write(_indices[i + 1]);
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