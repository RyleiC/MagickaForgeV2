using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class Effect
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Forward { get; set; }
        public float Range { get; set; }
        public string Type { get; set; }

        public Effect(BinaryReader binaryReader)
        {
            Name = binaryReader.ReadString();
            Position = new Vector3(binaryReader);
            Forward = new Vector3(binaryReader);
            Range = binaryReader.ReadSingle();
            Type = binaryReader.ReadString();
        }
    }
}
