namespace MagickaForge.Components.Levels.LevelEntities
{
    public class PhysicsEntity
    {
        public float[] position;
        public string type;
        public PhysicsEntity(BinaryReader binaryReader)
        {
            position = new float[16];
            for (int i = 0; i < 16; i++)
            {
                position[i] = binaryReader.ReadSingle();
            }
            type = binaryReader.ReadString();
        }
    }
}
