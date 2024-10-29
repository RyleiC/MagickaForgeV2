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
        public string Name { get; set; }
        public Matrix Transform { get; set; }
        public ModelBone[] Children { get; set; }
        public ModelBone() { }
        public ModelBone(BinaryReader binaryReader)
        {
            binaryReader.ReadByte();
            Name = binaryReader.ReadString();
            Transform = new Matrix(binaryReader);
        }
    }
}
