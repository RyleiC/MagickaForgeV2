namespace MagickaForge.Components.Graphics.Models
{
    public class ModelMeshPart
    {
        public int StreamOffset { get; set; }
        public int BaseVertex { get; set; }
        public int VertexCount { get; set; }
        public int StartIndex { get; set; }
        public int PrimativeCount { get; set; }
        public int VertexDeclarationIndex { get; set; }
        public byte Tag { get; set; }
        public int SharedContentID { get; set; }
        public ModelMeshPart() { }
        public ModelMeshPart(BinaryReader binaryReader, VertexDeclaration[] vertexDeclarations)
        {
            StreamOffset = binaryReader.ReadInt32();
            BaseVertex = binaryReader.ReadInt32();
            VertexCount = binaryReader.ReadInt32();
            StartIndex = binaryReader.ReadInt32();
            PrimativeCount = binaryReader.ReadInt32();
            VertexDeclarationIndex = binaryReader.ReadInt32();
            Tag = binaryReader.ReadByte();
            SharedContentID = binaryReader.Read7BitEncodedInt();
        }
    }
}
