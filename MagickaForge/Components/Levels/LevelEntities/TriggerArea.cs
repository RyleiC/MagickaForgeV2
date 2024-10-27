using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class TriggerArea
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 SideLenghts { get; set; }
        public Quaternion Quaternion { get; set; }
        public TriggerArea(BinaryReader binaryReader)
        {
            Name = binaryReader.ReadString();
            Position = new Vector3(binaryReader);
            SideLenghts = new Vector3(binaryReader);
            Quaternion = new Quaternion(binaryReader);
        }
    }
}
