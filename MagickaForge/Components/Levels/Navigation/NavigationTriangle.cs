using MagickaForge.Utils.Definitions.AI;

namespace MagickaForge.Components.Levels.Navigation
{
    public struct NavigationTriangle
    {
        public ushort vertexA;
        public ushort vertexB;
        public ushort vertexC;
        public ushort neighborA;
        public ushort neighborB;
        public ushort neighborC;
        public float costAB;
        public float costBC;
        public float costCA;
        public MovementProperties properties;

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
    }
}
