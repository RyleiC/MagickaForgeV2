using MagickaForge.Components.Common;

namespace MagickaForge.Components.Graphics.Effects
{
    public class BasicSkinnedEffect : ShaderEffect
    {
        public byte Method { get; set; }
        public float EmissiveAmount { get; set; }
        public Color DiffuseColor { get; set; }
        public float SpecularAmount { get; set; }
        public float SpecularPower { get; set; }
        public float DiffuseAlpha { get; set; }
        public bool UseSoftLightBlend { get; set; }
        public bool Map0DiffuseEnabled { get; set; }
        public bool Map1DiffuseEnabled { get; set; }
        public bool SpecularMapEnabled { get; set; }
        public bool Map0DamageEnabled { get; set; }
        public bool Map1DamageEnabled { get; set; }
        public bool NormalMapEnabled { get; set; }
        public int ExternalRefReaderIndex { get; set; }
        public string Map0Diffuse { get; set; }
        public string Map1Diffuse { get; set; }
        public string SpecularMap { get; set; }
        public string Map0Damage { get; set; }
        public string Map1Damage { get; set; }
        public string NormalMap { get; set; }
        public BasicSkinnedEffect() { }
        public BasicSkinnedEffect(BinaryReader binaryReader)
        {
            Method = binaryReader.ReadByte();
            EmissiveAmount = binaryReader.ReadSingle();
            DiffuseColor = new Color(binaryReader);
            SpecularAmount = binaryReader.ReadSingle();
            SpecularPower = binaryReader.ReadSingle();
            DiffuseAlpha = binaryReader.ReadSingle();
            UseSoftLightBlend = binaryReader.ReadBoolean();
            Map0DiffuseEnabled = binaryReader.ReadBoolean();
            Map1DiffuseEnabled = binaryReader.ReadBoolean();
            SpecularMapEnabled = binaryReader.ReadBoolean();
            Map0DamageEnabled = binaryReader.ReadBoolean();
            Map1DamageEnabled = binaryReader.ReadBoolean();
            NormalMapEnabled = binaryReader.ReadBoolean();
            if (Map0DiffuseEnabled)
            {
                ExternalRefReaderIndex = binaryReader.Read7BitEncodedInt();
            }
            Map0Diffuse = binaryReader.ReadString();
            if (Map1DiffuseEnabled)
            {
                ExternalRefReaderIndex = binaryReader.Read7BitEncodedInt();
            }
            Map1Diffuse = binaryReader.ReadString();
            if (SpecularMapEnabled)
            {
                ExternalRefReaderIndex = binaryReader.Read7BitEncodedInt();
            }
            SpecularMap = binaryReader.ReadString();
            if (Map0DamageEnabled)
            {
                ExternalRefReaderIndex = binaryReader.Read7BitEncodedInt();
            }
            Map0Damage = binaryReader.ReadString();
            if (Map1DamageEnabled)
            {
                ExternalRefReaderIndex = binaryReader.Read7BitEncodedInt();
            }
            Map1Damage = binaryReader.ReadString();
            if (NormalMapEnabled)
            {
                ExternalRefReaderIndex = binaryReader.Read7BitEncodedInt();
            }
            NormalMap = binaryReader.ReadString();
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Method);
            bw.Write(EmissiveAmount);
            DiffuseColor.Write(bw);
            bw.Write(SpecularAmount);
            bw.Write(SpecularPower);
            bw.Write(DiffuseAlpha);
            bw.Write(UseSoftLightBlend);
            bw.Write(Map0DiffuseEnabled);
            bw.Write(Map1DiffuseEnabled);
            bw.Write(SpecularMapEnabled);
            bw.Write(Map0DamageEnabled);
            bw.Write(Map1DamageEnabled);
            bw.Write(NormalMapEnabled);
            if (Map0DiffuseEnabled)
            {
                bw.Write7BitEncodedInt(ExternalRefReaderIndex);
            }
            bw.Write(Map0Diffuse);
            if (Map1DiffuseEnabled)
            {
                bw.Write7BitEncodedInt(ExternalRefReaderIndex);
            }
            bw.Write(Map1Diffuse);
            if (SpecularMapEnabled)
            {
                bw.Write7BitEncodedInt(ExternalRefReaderIndex);
            }
            bw.Write(SpecularMap);
            if (Map0DamageEnabled)
            {
                bw.Write7BitEncodedInt(ExternalRefReaderIndex);
            }
            bw.Write(Map0Damage);
            if (Map1DamageEnabled)
            {
                bw.Write7BitEncodedInt(ExternalRefReaderIndex);
            }
            bw.Write(Map1Damage);
            if (NormalMapEnabled)
            {
                bw.Write7BitEncodedInt(ExternalRefReaderIndex);
            }
            bw.Write(NormalMap);
        }
    }
}
