namespace MagickaForge.Components
{
    public struct Gib
    {
        public string Model { get; set; }
        public float Mass { get; set; }
        public float Scale { get; set; }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Model);
            binaryWriter.Write(Mass);
            binaryWriter.Write(Scale);
        }
    }
}
