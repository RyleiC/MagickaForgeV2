using MagickaForge.Components.Common;

namespace MagickaForge.Components.Graphics.Effects
{
    public class Material
    {
        public bool DiffuseNoAlpha { get; set; }
        public bool AlphaMaskEnabled { get; set; }
        public Color DiffuseColor { get; set; }
        public float SpecularAmount { get; set; }
        public float SpecularPower { get; set; }
        public float EmissiveAmount { get; set; }
        public float NormalPower { get; set; }
        public float Reflectiveness { get; set; }
        public string? DiffuseTexture { get; set; }
        public string? MaterialTexture { get; set; }
        public string? NormalTexture { get; set; }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(DiffuseNoAlpha);
            binaryWriter.Write(AlphaMaskEnabled);
            DiffuseColor.Write(binaryWriter);
            binaryWriter.Write(SpecularAmount);
            binaryWriter.Write(SpecularPower);
            binaryWriter.Write(EmissiveAmount);
            binaryWriter.Write(NormalPower);
            binaryWriter.Write(Reflectiveness);
            binaryWriter.Write(DiffuseTexture);
            binaryWriter.Write(MaterialTexture);
            binaryWriter.Write(NormalTexture);
        }
    }
}
