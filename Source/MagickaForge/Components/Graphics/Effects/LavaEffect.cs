using MagickaForge.Components.Common;

namespace MagickaForge.Components.Graphics.Effects
{
    public class LavaEffect : ShaderEffect
    {
        public float MaskDistortion { get; set; }
        public Vector2 SpeedA { get; set; }
        public Vector2 SpeedB { get; set; }
        public float HotEmission { get; set; }
        public float ColdEmission { get; set; }
        public float LavaSpecularAmount { get; set; }
        public float LavaSpecularPower { get; set; }
        public float TemperatureFrequency { get; set; }
        public string ToneMap { get; set; }
        public string TemperatureMap { get; set; }
        public string MaskMap { get; set; }
        public Vector3 RockColor { get; set; }
        public float RockEmission { get; set; }
        public float RockSpecularAmount { get; set; }
        public float RockSpecularPower { get; set; }
        public float RockNormalPower { get; set; }
        public string RockTexture { get; set; }
        public string RockNormalMap { get; set; }
        public LavaEffect() { }
        public LavaEffect(BinaryReader binaryReader)
        {
            MaskDistortion = binaryReader.ReadSingle();
            SpeedA = new Vector2(binaryReader);
            SpeedB = new Vector2(binaryReader);
            HotEmission = binaryReader.ReadSingle();
            ColdEmission = binaryReader.ReadSingle();
            LavaSpecularAmount = binaryReader.ReadSingle();
            LavaSpecularPower = binaryReader.ReadSingle();
            TemperatureFrequency = binaryReader.ReadSingle();
            ToneMap = binaryReader.ReadString();
            TemperatureMap = binaryReader.ReadString();
            MaskMap = binaryReader.ReadString();
            RockColor = new Vector3(binaryReader);
            RockEmission = binaryReader.ReadSingle();
            RockSpecularAmount = binaryReader.ReadSingle();
            RockSpecularPower = binaryReader.ReadSingle();
            RockNormalPower = binaryReader.ReadSingle();
            RockTexture = binaryReader.ReadString();
            RockNormalMap = binaryReader.ReadString();
        }
        public override void Write(BinaryWriter binaryWriter)
        {
            base.Write(binaryWriter);
            binaryWriter.Write(MaskDistortion);
            SpeedA.Write(binaryWriter);
            SpeedB.Write(binaryWriter);
            binaryWriter.Write(HotEmission);
            binaryWriter.Write(ColdEmission);
            binaryWriter.Write(LavaSpecularAmount);
            binaryWriter.Write(LavaSpecularPower);
            binaryWriter.Write(TemperatureFrequency);
            binaryWriter.Write(ToneMap);
            binaryWriter.Write(TemperatureMap);
            binaryWriter.Write(MaskMap);
            RockColor.Write(binaryWriter);
            binaryWriter.Write(RockEmission);
            binaryWriter.Write(RockSpecularAmount);
            binaryWriter.Write(RockSpecularPower);
            binaryWriter.Write(RockNormalPower);
            binaryWriter.Write(RockTexture);
            binaryWriter.Write(RockNormalMap);
        }
    }
}
