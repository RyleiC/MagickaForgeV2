using MagickaForge.Utils.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Graphics.Models
{
    public class ModelBone
    {
        public int readerType { get; set; }
        public int BoneIndex { get; set; }
        public string Name { get; set; }
        public Matrix Transform { get; set; }
        public int[] Children { get; set; }
        public int Parent { get; set; }
        public ModelBone() { }
        public ModelBone(BinaryReader binaryReader)
        {
            readerType = binaryReader.Read7BitEncodedInt();
            Name = binaryReader.ReadString();
            Transform = new Matrix(binaryReader);
        }
    }
}
