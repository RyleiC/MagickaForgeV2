namespace MagickaForge.Utils.Structures
{
    public struct Quaternion
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Quaternion(BinaryReader binaryReader)
        {
            X = binaryReader.ReadSingle();
            Y = binaryReader.ReadSingle();
            Z = binaryReader.ReadSingle();
            W = binaryReader.ReadSingle();
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(X);
            binaryWriter.Write(Y);
            binaryWriter.Write(Z);
            binaryWriter.Write(W);
        }
    }
}
