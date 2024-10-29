using MagickaForge.Utils.Structures;
namespace MagickaForge.Components.Levels.LevelEntities
{
    public class PhysicsEntity
    {
        public Matrix position { get; set; }
        public string type { get; set; }
        public PhysicsEntity() { }
        public PhysicsEntity(BinaryReader binaryReader)
        {
            position = new Matrix(binaryReader);
            type = binaryReader.ReadString();
        }
        public void Write(BinaryWriter binaryWriter)
        {
            position.Write(binaryWriter);
            binaryWriter.Write(type);
        }
    }
}
