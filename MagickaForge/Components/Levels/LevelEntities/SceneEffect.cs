using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class SceneEffect
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Forward { get; set; }
        public float Range { get; set; }
        public string Type { get; set; }
        public SceneEffect() { }
        public SceneEffect(BinaryReader binaryReader)
        {
            Name = binaryReader.ReadString();
            Position = new Vector3(binaryReader);
            Forward = new Vector3(binaryReader);
            Range = binaryReader.ReadSingle();
            Type = binaryReader.ReadString();
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Name);
            Position.Write(binaryWriter);
            Forward.Write(binaryWriter);
            binaryWriter.Write(Range);
            binaryWriter.Write(Type);
        }
    }
}
