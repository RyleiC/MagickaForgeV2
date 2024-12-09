using MagickaForge.Components.Graphics;
using MagickaForge.Components.Graphics.Effects;
using MagickaForge.Components.Levels;
using MagickaForge.Components.Levels.Navigation;
using MagickaForge.Experimental.GLTF;
using System.Text.Json;

namespace MagickaForge.Pipeline.Levels
{
    //Temp file for testing GLTF embedding
    public class GLTFHook
    {
        public GLTFHook(string path, string declarationBase)
        {
            ModelNode modelNode = ModelNode.LoadGLTFModel(path);
            modelNode.InitializeBuffer();

            Collision = modelNode.Buffer.ToTriangleMesh();
            Collision.ReaderType = 10;

            Root = JsonSerializer.Deserialize<BinTreeRoot>(File.ReadAllText(declarationBase));

            Root.VertexCount = modelNode.Buffer.VertexCount;
            Root.PrimativeCount = modelNode.Buffer.VertexCount * 8;
            Root.IndexBuffer.Data = modelNode.Buffer.ToIndexBuffer().Data;
            Root.VertexBuffer.Data = modelNode.Buffer.ToVertexBuffer().Data;

            NavigationMesh = modelNode.Buffer.ToNavigationMesh();
        }

        public TriangleMesh Collision { get; set; }

        public BinTreeRoot Root { get; set; }

        public NavigationMesh NavigationMesh { get; set; }
    }
}
