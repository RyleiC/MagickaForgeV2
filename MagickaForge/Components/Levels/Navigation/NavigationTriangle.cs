using MagickaForge.Utils.Definitions.AI;

namespace MagickaForge.Components.Levels.Navigation
{
    public struct NavigationTriangle
    {
        public ushort vertexA { get; set; }
        public ushort vertexB { get; set; }
        public ushort vertexC { get; set; }
        public ushort neighborA { get; set; }
        public ushort neighborB { get; set; }
        public ushort neighborC { get; set; }
        public float costAB { get; set; }
        public float costBC { get; set; }
        public float costCA { get; set; }
        public MovementProperties properties { get; set; }

        public NavigationTriangle(BinaryReader br)
        {
            vertexA = br.ReadUInt16();
            vertexB = br.ReadUInt16();
            vertexC = br.ReadUInt16();
            neighborA = br.ReadUInt16();
            neighborB = br.ReadUInt16();
            neighborC = br.ReadUInt16();
            costAB = br.ReadSingle();
            costBC = br.ReadSingle();
            costCA = br.ReadSingle();
            properties = (MovementProperties)br.ReadByte();
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(vertexA);
            binaryWriter.Write(vertexB);
            binaryWriter.Write(vertexC);
            binaryWriter.Write(neighborA);
            binaryWriter.Write(neighborB);
            binaryWriter.Write(neighborC);
            binaryWriter.Write(costAB);
            binaryWriter.Write(costBC);
            binaryWriter.Write(costCA);
            binaryWriter.Write((byte)properties);
        }
    }
}
