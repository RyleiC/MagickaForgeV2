namespace MagickaForge.Components.Levels.LevelEntities
{
    public class Effect
    {
        public string Name { get; set; }
        public float[] Position { get; set; }
        public float[] Forward { get; set; }
        public float Range { get; set; }
        public string Type { get; set; }

        public Effect(BinaryReader binaryReader)
        {
            Name = binaryReader.ReadString();
            Position = new float[3];
            for (var i = 0; i < 3; i++)
            {
                Position[i] = binaryReader.ReadSingle();
            }
            Forward = new float[3];
            for (var i = 0; i < 3; i++)
            {
                Forward[i] = binaryReader.ReadSingle();
            }
            Range = binaryReader.ReadSingle();
            Type = binaryReader.ReadString();
        }
    }
}
