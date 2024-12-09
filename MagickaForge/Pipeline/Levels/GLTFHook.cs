using MagickaForge.Components.Graphics;
using MagickaForge.Components.Levels;
using MagickaForge.Experimental.GLTF;
using System.Text.Json;

namespace MagickaForge.Pipeline.Levels
{
    //Temp file for testing GLTF embedding
    public class GLTFHook
    {
        private const string _path = "C:\\Users\\Rylei\\Downloads\\Toyue\\untitled.gltf";
        private const string _decl = "C:\\Users\\Rylei\\Downloads\\Toyue\\vertexbase.json";

        public GLTFHook()
        {
            ModelNode modelNode = ModelNode.LoadGLTFModel(_path);
            modelNode.InitializeBuffer();

            Collision = modelNode.Buffer.ToTriangleMesh();
            Collision.ReaderType = 10;

            Root = JsonSerializer.Deserialize<BinTreeRoot>(File.ReadAllText(_decl));

            Root.VertexCount = modelNode.Buffer.VertexCount;
            Root.PrimativeCount = modelNode.Buffer.VertexCount * 8;
            Root.IndexBuffer.Data = modelNode.Buffer.ToIndexBuffer().Data;
            Root.VertexBuffer.Data = modelNode.Buffer.ToVertexBuffer().Data;

        }

        public TriangleMesh Collision { get; set; }

        public BinTreeRoot Root { get; set; }
    }
}
