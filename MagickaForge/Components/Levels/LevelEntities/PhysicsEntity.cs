using MagickaForge.Utils.Structures;
namespace MagickaForge.Components.Levels.LevelEntities
{
    public class PhysicsEntity
    {
        public Matrix position;
        public string type;
        public PhysicsEntity(BinaryReader binaryReader)
        {
            position = new Matrix(binaryReader);
            type = binaryReader.ReadString();
        }
    }
}
