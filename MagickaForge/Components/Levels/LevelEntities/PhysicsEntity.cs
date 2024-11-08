using MagickaForge.Utils.Structures;
namespace MagickaForge.Components.Levels.LevelEntities
{
    public class PhysicsEntity
    {
        public Matrix Position { get; set; }
        public string Type { get; set; }
        public PhysicsEntity() { }
        public PhysicsEntity(BinaryReader binaryReader)
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
