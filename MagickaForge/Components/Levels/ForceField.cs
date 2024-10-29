using MagickaForge.Components.Graphics;
using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels
{
    public class ForceField
    {
        public Color color { get; set; }
        public float width { get; set; }
        public float alphaPower { get; set; }
        public float alphaFalloffPower { get; set; }
        public float maxRadius { get; set; }
        public float rippleDistortion { get; set; }
        public float mapDistortion { get; set; }
        public bool vertexColorEnabled { get; set; }
        public string displacementMap { get; set; }
        public float timeToLive { get; set; }
        public VertexBuffer vertexBuffer { get; set; }
        public IndexBuffer indexBuffer { get; set; }
        public VertexDeclaration vertexDeclaration { get; set; }
        public int vertexStride { get; set; }
        public int vertexCount { get; set; }
        public int primativeCount { get; set; }

        public ForceField(BinaryReader binaryReader)
        {
            color = new Color(binaryReader);
            width = binaryReader.ReadSingle();
            alphaPower = binaryReader.ReadSingle();
            alphaFalloffPower = binaryReader.ReadSingle();
            maxRadius = binaryReader.ReadSingle();
            rippleDistortion = binaryReader.ReadSingle();
            mapDistortion = binaryReader.ReadSingle();
            vertexColorEnabled = binaryReader.ReadBoolean();
            displacementMap = binaryReader.ReadString();
            timeToLive = binaryReader.ReadSingle();
            vertexBuffer = new VertexBuffer(binaryReader);
            indexBuffer = new IndexBuffer(binaryReader);
            vertexDeclaration = new VertexDeclaration(binaryReader);
            vertexStride = binaryReader.ReadInt32();
            vertexCount = binaryReader.ReadInt32();
            primativeCount = binaryReader.ReadInt32();
        }

        public void Write(BinaryWriter binaryWriter)
        {
            color.Write(binaryWriter);
            binaryWriter.Write(width);
            binaryWriter.Write(alphaPower);
            binaryWriter.Write(alphaFalloffPower);
            binaryWriter.Write(maxRadius);
            binaryWriter.Write(rippleDistortion);
            binaryWriter.Write(mapDistortion);
            binaryWriter.Write(vertexColorEnabled);
            binaryWriter.Write(displacementMap);
            binaryWriter.Write(timeToLive);
            vertexBuffer.Write(binaryWriter);
            indexBuffer.Write(binaryWriter);
            vertexDeclaration.Write(binaryWriter);
            binaryWriter.Write(vertexStride);
            binaryWriter.Write(vertexCount);
            binaryWriter.Write(primativeCount);
        }
    }
}
