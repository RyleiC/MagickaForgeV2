namespace MagickaForge.Components.Common
{
    public struct Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3(BinaryReader binaryReader)
        {
            X = binaryReader.ReadSingle();
            Y = binaryReader.ReadSingle();
            Z = binaryReader.ReadSingle();
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(X);
            binaryWriter.Write(Y);
            binaryWriter.Write(Z);
        }
    }
}
