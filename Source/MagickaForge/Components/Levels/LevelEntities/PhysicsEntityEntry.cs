using MagickaForge.Components.Common;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class PhysicsEntityEntry
    {
        public Matrix Position { get; set; }
        public string Type { get; set; }

        public PhysicsEntityEntry() { }

        public PhysicsEntityEntry(BinaryReader binaryReader)
        {
            Position = new Matrix(binaryReader);
            Type = binaryReader.ReadString();
        }

        public void Write(BinaryWriter binaryWriter)
        {
            Position.Write(binaryWriter);
            binaryWriter.Write(Type);
        }
    }
}
