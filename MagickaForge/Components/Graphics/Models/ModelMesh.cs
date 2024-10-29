using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Graphics.Models
{
    public class ModelMesh
    {
        public string Name { get; set; }
        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        public ModelBone ParentBone { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public byte Tag { get; set; }
        public ModelMeshPart[] Parts { get; set; }
        public ModelMesh() { }
        public ModelMesh(BinaryReader reader, Model model, VertexDeclaration[] vertexDeclarations)
        {
            reader.ReadByte();
            Name = reader.ReadString();
            ParentBone = model.ReadBoneReference(reader);
            Center = new Vector3(reader);
            Radius = reader.ReadSingle();
            VertexBuffer = new VertexBuffer(reader);
            IndexBuffer = new IndexBuffer(reader);
            Tag = reader.ReadByte();
            Parts = model.ReadMeshParts(reader, VertexBuffer, IndexBuffer, vertexDeclarations);
        }
    }
}
