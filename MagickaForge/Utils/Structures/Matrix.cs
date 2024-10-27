namespace MagickaForge.Utils.Structures
{
    public class Matrix
    {
        public float[] values { get; set; }

        public Matrix(BinaryReader reader)
        {
            values = new float[16];
            for (int i = 0; i < 16; i++)
            {
                values[i] = reader.ReadSingle();
            }
        }
    }
}
