using MagickaForge.Components.Common;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class Locator
    {
        public string Name { get; set; }
        public Matrix Transform { get; set; }
        public float Radius { get; set; }
        public Locator() { }
        public Locator(BinaryReader binaryReader)
        {
            Name = binaryReader.ReadString();
            Transform = new Matrix(binaryReader);
            Radius = binaryReader.ReadSingle();
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Name);
            Transform.Write(binaryWriter);
            binaryWriter.Write(Radius);
        }
    }
}
