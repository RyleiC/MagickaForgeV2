namespace MagickaForge.Utils.Structures
{
    public class Matrix
    {
        private const int MatrixLength = 16;
        public float[] Values { get; set; }
        public Matrix() { }
        public Matrix(BinaryReader reader)
        {
            Values = new float[MatrixLength];
            for (int i = 0; i < MatrixLength; i++)
            {
                Values[i] = reader.ReadSingle();
            }
        }
        public void Write(BinaryWriter binaryWriter)
        {
            for (var i = 0; i < MatrixLength; i++)
            {
                binaryWriter.Write(Values[i]);
            }
        }
    }
}
