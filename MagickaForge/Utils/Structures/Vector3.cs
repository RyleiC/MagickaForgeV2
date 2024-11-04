namespace MagickaForge.Utils.Structures
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

        public override bool Equals(object obj)
        {
            if (obj is not Vector3)
            {
                return false;
            }

            Vector3 other = (Vector3)obj;

            if (X == other.X && Y == other.Y && Z == other.Z)
            {
                return true;
            }

            return false;

        }
    }
}
