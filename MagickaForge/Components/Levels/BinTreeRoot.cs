using MagickaForge.Components.Common;
using MagickaForge.Components.Graphics;
using MagickaForge.Components.Graphics.Effects;
using MagickaForge.Components.XNB;

namespace MagickaForge.Components.Levels
{
    public class BinTreeRoot
    {
        public bool Visible { get; set; }
        public bool CastShadows { get; set; }
        public float Sway { get; set; }
        public float EntityInfluence { get; set; }
        public float GroundLevel { get; set; }
        public int VertexCount { get; set; }
        public int VertexStride { get; set; }

        public VertexDeclaration VertexDeclaration { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public ShaderEffect Effect { get; set; }

        public int PrimativeCount { get; set; }
        public int StartIndex { get; set; }
        public Vector3 BoundingBoxMax { get; set; }
        public Vector3 BoundingBoxMin { get; set; }

        public BinTreeNode? ChildA { get; set; }
        public BinTreeNode? ChildB { get; set; }
        public BinTreeRoot() { }
        public BinTreeRoot(BinaryReader binaryReader, Header header)
        {
            Visible = binaryReader.ReadBoolean();
            CastShadows = binaryReader.ReadBoolean();
            Sway = binaryReader.ReadSingle();
            EntityInfluence = binaryReader.ReadSingle();
            GroundLevel = binaryReader.ReadSingle();
            VertexCount = binaryReader.ReadInt32();
            VertexStride = binaryReader.ReadInt32();

            VertexDeclaration = new VertexDeclaration(binaryReader);
            VertexBuffer = new VertexBuffer(binaryReader);
            IndexBuffer = new IndexBuffer(binaryReader);
            Effect = ShaderEffect.GetEffect(binaryReader, header);
            PrimativeCount = binaryReader.ReadInt32();
            StartIndex = binaryReader.ReadInt32();
            BoundingBoxMax = new Vector3(binaryReader);
            BoundingBoxMin = new Vector3(binaryReader);
            bool hasChildA = binaryReader.ReadBoolean();
            if (hasChildA)
            {
                ChildA = new BinTreeNode(binaryReader);
            }
            bool hasChildB = binaryReader.ReadBoolean();
            if (hasChildB)
            {
                ChildB = new BinTreeNode(binaryReader);
            }
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Visible);
            binaryWriter.Write(CastShadows);
            binaryWriter.Write(Sway);
            binaryWriter.Write(EntityInfluence);
            binaryWriter.Write(GroundLevel);
            binaryWriter.Write(VertexCount);
            binaryWriter.Write(VertexStride);

            VertexDeclaration.Write(binaryWriter);
            VertexBuffer.Write(binaryWriter);
            IndexBuffer.Write(binaryWriter);
            Effect.Write(binaryWriter);
            binaryWriter.Write(PrimativeCount);
            binaryWriter.Write(StartIndex);
            BoundingBoxMax.Write(binaryWriter);
            BoundingBoxMin.Write(binaryWriter);

            var hasChildA = ChildA != null;
            var hasChildB = ChildB != null;

            binaryWriter.Write(hasChildA);
            if (hasChildA)
            {
                ChildA.Write(binaryWriter);
            }
            binaryWriter.Write(hasChildB);
            if (hasChildB)
            {
                ChildB.Write(binaryWriter);
            }
        }
    }
}
