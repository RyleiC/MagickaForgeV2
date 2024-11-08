using MagickaForge.Components.Graphics;
using MagickaForge.Components.Graphics.Effects;
using MagickaForge.Components.XNB;

namespace MagickaForge.Components.Levels.Liquid
{
    public class LiquidDeclaration
    {
        public ShaderEffect Effect { get; set; }
        public Liquid Liquid { get; set; }
        public LiquidDeclaration() { }
        public LiquidDeclaration(BinaryReader br, Header header)
        {
            Effect = ShaderEffect.GetEffect(br, header);
            Liquid = new Liquid(br);
        }

        public void Write(BinaryWriter binaryWriter)
        {
            Effect.Write(binaryWriter);
            Liquid.Write(binaryWriter);
        }
    }
    public class Liquid
    {
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public VertexDeclaration VertexDeclaration { get; set; }
        public int VertexStride { get; set; }
        public int VertexCount { get; set; }
        public int PrimativeCount { get; set; }
        public bool Collidable { get; set; }
        public bool Freezable { get; set; }
        public bool AutoFreeze { get; set; }
        public Liquid() { }
        public Liquid(BinaryReader br)
        {
            VertexBuffer = new VertexBuffer(br);
            IndexBuffer = new IndexBuffer(br);
            VertexDeclaration = new VertexDeclaration(br);
            VertexStride = br.ReadInt32();
            VertexCount = br.ReadInt32();
            PrimativeCount = br.ReadInt32();
            Collidable = br.ReadBoolean();
            Freezable = br.ReadBoolean();
            AutoFreeze = br.ReadBoolean();
        }
        public void Write(BinaryWriter binaryWriter)
        {
            VertexBuffer.Write(binaryWriter);
            IndexBuffer.Write(binaryWriter);
            VertexDeclaration.Write(binaryWriter);
            binaryWriter.Write(VertexStride);
            binaryWriter.Write(VertexCount);
            binaryWriter.Write(PrimativeCount);
            binaryWriter.Write(Collidable);
            binaryWriter.Write(Freezable);
            binaryWriter.Write(AutoFreeze);
        }
    }
}
