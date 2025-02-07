using MagickaForge.Components.Common;

namespace MagickaForge.Components.Graphics.Models
{
    public class ModelMesh
    {
        public string Name { get; set; }
        public int ReaderType { get; set; }
        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        public int ParentBone { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public byte Tag { get; set; }
        public ModelMeshPart[] MeshParts { get; set; }
        public ModelMesh() { }
        public ModelMesh(BinaryReader reader, Model model, VertexDeclaration[] vertexDeclarations)
        {
            ReaderType = reader.Read7BitEncodedInt();
            Name = reader.ReadString();
            ParentBone = model.ReadBoneReference(reader);
            Center = new Vector3(reader);
            Radius = reader.ReadSingle();
            VertexBuffer = new VertexBuffer(reader);
            IndexBuffer = new IndexBuffer(reader);
            Tag = reader.ReadByte();
            MeshParts = model.ReadMeshParts(reader, VertexBuffer, IndexBuffer, vertexDeclarations);
        }
    }
}
