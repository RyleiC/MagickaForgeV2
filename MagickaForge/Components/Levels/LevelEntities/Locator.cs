namespace MagickaForge.Components.Levels.LevelEntities
{
    public class Locator
    {
        public string Name { get; set; }
        public float[] Transform { get; set; }
        public float Radius { get; set; }

        public Locator(BinaryReader binaryReader)
        {
            Name = binaryReader.ReadString();
            Transform = new float[16];
            for (int i = 0; i < 16; i++)
            {
                Transform[i] = binaryReader.ReadSingle();
            }
            Radius = binaryReader.ReadSingle();
        }
    }
}
