using System.Numerics;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class TriggerArea
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 SideLenghts { get; set; }
        public Quaternion Quaternion { get; set; }
        public float[] Orientation { get; set; }
        public TriggerArea(BinaryReader binaryReader)
        {
            Name = binaryReader.ReadString();
            Position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
            SideLenghts = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
            Quaternion = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }
    }
}
