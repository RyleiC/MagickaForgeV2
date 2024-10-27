using MagickaForge.Components.Graphics;
using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels
{
    public class ForceField
    {
        private Color color;
        private float width;
        private float alphaPower;
        private float alphaFalloffPower;
        private float maxRadius;
        private float rippleDistortion;
        private float mapDistortion;
        private bool vertexColorEnabled;
        private string displacementMap;
        private float timeToLive;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private VertexDeclaration vertexDeclaration;
        private int vertexStride;
        private int vertexCount;
        private int primativeCount;

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
    }
}
