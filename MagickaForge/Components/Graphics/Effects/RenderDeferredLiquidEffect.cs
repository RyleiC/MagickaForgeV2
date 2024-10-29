using MagickaForge.Utils.Structures;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Graphics.Effects
{
    public class RenderDeferredLiquidEffect : ShaderEffect
    {
        public string reflectionMap { get; set; }
        public float waveHeight { get; set; }
        public Vector2 waveSpeedA { get; set; }
        public Vector2 waveSpeedB { get; set; }
        public float waterReflectiveness { get; set; }
        public Color bottomColor { get; set; }
        public Color deepColor { get; set; }
        public float waterEmissive { get; set; }
        public float waterSpecAmount { get; set; }
        public float waterSpecPower { get; set; }
        public string bottomTexture { get; set; }
        public string waterNormalMap { get; set; }
        public float iceReflectiveness { get; set; }
        public Color iceColor { get; set; }
        public float iceEmissiveAmount { get; set; }
        public float iceSpecAmount { get; set; }
        public float iceSpecPower { get; set; }
        public string iceDiffuseMap { get; set; }
        public string iceNormalMap { get; set; }
        public RenderDeferredLiquidEffect() { }
        public RenderDeferredLiquidEffect(BinaryReader binaryReader)
        {
            reflectionMap = binaryReader.ReadString();
            waveHeight = binaryReader.ReadSingle();
            waveSpeedA = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
            waveSpeedB = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
            waterReflectiveness = binaryReader.ReadSingle();
            bottomColor = new Color(binaryReader);
            deepColor = new Color(binaryReader);
            waterEmissive = binaryReader.ReadSingle();
            waterSpecAmount = binaryReader.ReadSingle();
            waterSpecPower = binaryReader.ReadSingle();
            bottomTexture = binaryReader.ReadString();
            waterNormalMap = binaryReader.ReadString();
            iceReflectiveness = binaryReader.ReadSingle();
            iceColor = new Color(binaryReader);
            iceEmissiveAmount = binaryReader.ReadSingle();
            iceSpecAmount = binaryReader.ReadSingle();
            iceSpecPower = binaryReader.ReadSingle();
            iceDiffuseMap = binaryReader.ReadString();
            iceNormalMap = binaryReader.ReadString();
        }
        public override void Write(BinaryWriter binaryWriter)
        {
            base.Write(binaryWriter);
            binaryWriter.Write(reflectionMap);
            binaryWriter.Write(waveHeight);
            waveSpeedA.Write(binaryWriter);
            waveSpeedB.Write(binaryWriter);
            binaryWriter.Write(waterReflectiveness);
            bottomColor.Write(binaryWriter);
            deepColor.Write(binaryWriter);
            binaryWriter.Write(waterEmissive);
            binaryWriter.Write(waterSpecAmount);
            binaryWriter.Write(waterSpecPower);
            binaryWriter.Write(bottomTexture);
            binaryWriter.Write(waterNormalMap);
            binaryWriter.Write(iceReflectiveness);
            iceColor.Write(binaryWriter);
            binaryWriter.Write(iceEmissiveAmount);
            binaryWriter.Write(iceSpecAmount);
            binaryWriter.Write(iceSpecPower);
            binaryWriter.Write(iceDiffuseMap);
            binaryWriter.Write(iceNormalMap);
        }
    }
}
