namespace MagickaForge.Components.Graphics.Models
{
    public class ModelMeshPart
    {
        public int streamOffset { get; set; }
        public int baseVertex { get; set; }
        public int numVertices { get; set; }
        public int startIndex { get; set; }
        public int primativeCount { get; set; }
        public int vdIndex { get; set; }
        public byte Tag { get; set; }
        public int SharedContentID { get; set; }
        public ModelMeshPart() { }
        public ModelMeshPart(BinaryReader binaryReader, VertexDeclaration[] vertexDeclarations)
        {
            streamOffset = binaryReader.ReadInt32();
            baseVertex = binaryReader.ReadInt32();
            numVertices = binaryReader.ReadInt32();
            startIndex = binaryReader.ReadInt32();
            primativeCount = binaryReader.ReadInt32();
            vdIndex = binaryReader.ReadInt32();
            Tag = binaryReader.ReadByte();
            SharedContentID = binaryReader.Read7BitEncodedInt();
        }
    }
}
