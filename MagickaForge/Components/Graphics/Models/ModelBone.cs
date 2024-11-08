using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Graphics.Models
{
    public class ModelBone
    {
        public int ReaderType { get; set; }
        public int BoneIndex { get; set; }
        public string Name { get; set; }
        public Matrix Transform { get; set; }
        public int[] Children { get; set; }
        public int Parent { get; set; }
        public ModelBone() { }
        public ModelBone(BinaryReader binaryReader)
        {
            ReaderType = binaryReader.Read7BitEncodedInt();
            Name = binaryReader.ReadString();
            Transform = new Matrix(binaryReader);
        }
    }
}
