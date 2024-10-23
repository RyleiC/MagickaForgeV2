using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Graphics.Effects
{
    public struct Material
    {
        public bool DiffuseNoAlpha;
        public bool AlphaMaskEnabled;
        public float[] DiffuseColor;
        public float SpecularAmount;
        public float SpecularPower;
        public float EmissiveAmount;
        public float NormalPower;
        public float Reflectiveness;
        public string? DiffuseTexture;
        public string? MaterialTexture;
        public string? NormalTexture;
    }
}
