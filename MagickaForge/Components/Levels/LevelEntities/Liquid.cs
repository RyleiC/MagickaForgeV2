using MagickaForge.Components.Graphics;
using MagickaForge.Components.Graphics.Effects;
using MagickaForge.Components.XNB;

namespace MagickaForge.Components.Levels.Liquid
{
    public class LiquidDeclaration
    {
        public ShaderEffect effect { get; set; }
        public Liquid liquid { get; set; }
        public LiquidDeclaration() { }
        public LiquidDeclaration(BinaryReader br, Header header)
        {
            effect = ShaderEffect.GetEffect(br, header);
            liquid = new Liquid(br);
        }

        public void Write(BinaryWriter binaryWriter)
        {
            effect.Write(binaryWriter);
            liquid.Write(binaryWriter);
        }
    }
    public class Liquid
    {
        public VertexBuffer vertexBuffer { get; set; }
        public IndexBuffer indexBuffer { get; set; }
        public VertexDeclaration vertexDeclaration { get; set; }
        public int vertexStride { get; set; }
        public int vertexCount { get; set; }
        public int primativeCount { get; set; }
        public bool collidable { get; set; }
        public bool freezable { get; set; }
        public bool autoFreeze { get; set; }
        public Liquid() { }
        public Liquid(BinaryReader br)
        {
            vertexBuffer = new VertexBuffer(br);
            indexBuffer = new IndexBuffer(br);
            vertexDeclaration = new VertexDeclaration(br);
            vertexStride = br.ReadInt32();
            vertexCount = br.ReadInt32();
            primativeCount = br.ReadInt32();
            collidable = br.ReadBoolean();
            freezable = br.ReadBoolean();
            autoFreeze = br.ReadBoolean();
        }
        public void Write(BinaryWriter binaryWriter)
        {
            vertexBuffer.Write(binaryWriter);
            indexBuffer.Write(binaryWriter);
            vertexDeclaration.Write(binaryWriter);
            binaryWriter.Write(vertexStride);
            binaryWriter.Write(vertexCount);
            binaryWriter.Write(primativeCount);
            binaryWriter.Write(collidable);
            binaryWriter.Write(freezable);
            binaryWriter.Write(autoFreeze);
        }
    }
}
