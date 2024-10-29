using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class TriggerArea
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 SideLengths { get; set; }
        public Quaternion Quaternion { get; set; }
        public TriggerArea() { }
        public TriggerArea(BinaryReader binaryReader)
        {
            Name = binaryReader.ReadString();
            Position = new Vector3(binaryReader);
            SideLengths = new Vector3(binaryReader);
            Quaternion = new Quaternion(binaryReader);
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Name);
            Position.Write(binaryWriter);
            SideLengths.Write(binaryWriter);
            Quaternion.Write(binaryWriter);
        }
    }
}
