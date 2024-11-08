using MagickaForge.Components.Graphics;
using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels
{
    public class ForceField
    {
        public Color Color { get; set; }
        public float Width { get; set; }
        public float AlphaPower { get; set; }
        public float AlphaFalloffPower { get; set; }
        public float MaxRadius { get; set; }
        public float RippleDistortion { get; set; }
        public float MapDistortion { get; set; }
        public bool VertexColorEnabled { get; set; }
        public string DisplacementMap { get; set; }
        public float Duration { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public VertexDeclaration VertexDeclaration { get; set; }
        public int VertexStride { get; set; }
        public int VertexCount { get; set; }
        public int PrimativeCount { get; set; }

        public ForceField(BinaryReader binaryReader)
        {
            Color = new Color(binaryReader);
            Width = binaryReader.ReadSingle();
            AlphaPower = binaryReader.ReadSingle();
            AlphaFalloffPower = binaryReader.ReadSingle();
            MaxRadius = binaryReader.ReadSingle();
            RippleDistortion = binaryReader.ReadSingle();
            MapDistortion = binaryReader.ReadSingle();
            VertexColorEnabled = binaryReader.ReadBoolean();
            DisplacementMap = binaryReader.ReadString();
            Duration = binaryReader.ReadSingle();
            VertexBuffer = new VertexBuffer(binaryReader);
            IndexBuffer = new IndexBuffer(binaryReader);
            VertexDeclaration = new VertexDeclaration(binaryReader);
            VertexStride = binaryReader.ReadInt32();
            VertexCount = binaryReader.ReadInt32();
            PrimativeCount = binaryReader.ReadInt32();
        }

        public void Write(BinaryWriter binaryWriter)
        {
            Color.Write(binaryWriter);
            binaryWriter.Write(Width);
            binaryWriter.Write(AlphaPower);
            binaryWriter.Write(AlphaFalloffPower);
            binaryWriter.Write(MaxRadius);
            binaryWriter.Write(RippleDistortion);
            binaryWriter.Write(MapDistortion);
            binaryWriter.Write(VertexColorEnabled);
            binaryWriter.Write(DisplacementMap);
            binaryWriter.Write(Duration);
            VertexBuffer.Write(binaryWriter);
            IndexBuffer.Write(binaryWriter);
            VertexDeclaration.Write(binaryWriter);
            binaryWriter.Write(VertexStride);
            binaryWriter.Write(VertexCount);
            binaryWriter.Write(PrimativeCount);
        }
    }
}
