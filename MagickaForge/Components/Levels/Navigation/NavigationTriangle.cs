using MagickaForge.Utils.Data.AI;

namespace MagickaForge.Components.Levels.Navigation
{
    public struct NavigationTriangle
    {
        public ushort VertexA { get; set; }
        public ushort VertexB { get; set; }
        public ushort VertexC { get; set; }
        public ushort NeighborA { get; set; }
        public ushort NeighborB { get; set; }
        public ushort NeighborC { get; set; }
        public float CostAB { get; set; }
        public float CostBC { get; set; }
        public float CostCA { get; set; }
        public MovementProperties MovementProperty { get; set; }

        public NavigationTriangle(BinaryReader br)
        {
            VertexA = br.ReadUInt16();
            VertexB = br.ReadUInt16();
            VertexC = br.ReadUInt16();
            NeighborA = br.ReadUInt16();
            NeighborB = br.ReadUInt16();
            NeighborC = br.ReadUInt16();
            CostAB = br.ReadSingle();
            CostBC = br.ReadSingle();
            CostCA = br.ReadSingle();
            MovementProperty = (MovementProperties)br.ReadByte();
        }
        public readonly void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(VertexA);
            binaryWriter.Write(VertexB);
            binaryWriter.Write(VertexC);
            binaryWriter.Write(NeighborA);
            binaryWriter.Write(NeighborB);
            binaryWriter.Write(NeighborC);
            binaryWriter.Write(CostAB);
            binaryWriter.Write(CostBC);
            binaryWriter.Write(CostCA);
            binaryWriter.Write((byte)MovementProperty);
        }
    }
}
