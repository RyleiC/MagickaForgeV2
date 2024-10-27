namespace MagickaForge.Utils.Structures
{
    public struct Color
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public Color(float x, float y, float z)
        {
            R = x;
            G = y;
            B = z;
        }
        public Color(BinaryReader binaryReader)
        {
            R = binaryReader.ReadSingle();
            G = binaryReader.ReadSingle();
            B = binaryReader.ReadSingle();
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(R);
            binaryWriter.Write(G);
            binaryWriter.Write(B);
        }
    }
}
