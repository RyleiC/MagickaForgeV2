using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Graphics.Effects
{
    public class RenderDeferredLiquidEffect : ShaderEffect
    {
        public string ReflectionMap { get; set; }
        public float WaveHeight { get; set; }
        public Vector2 WaveSpeedA { get; set; }
        public Vector2 WaveSpeedB { get; set; }
        public float WaterReflectiveness { get; set; }
        public Color BottomColor { get; set; }
        public Color DeepColor { get; set; }
        public float WaterEmissiveAmount { get; set; }
        public float WaterSpecularAmount { get; set; }
        public float WaterSpecularPower { get; set; }
        public string BottomTexture { get; set; }
        public string WaterNormalMap { get; set; }
        public float IceReflectiveness { get; set; }
        public Color IceColor { get; set; }
        public float IceEmissiveAmount { get; set; }
        public float IceSpecularAmount { get; set; }
        public float IceSpecularPower { get; set; }
        public string IceDiffuseMap { get; set; }
        public string IceNormalMap { get; set; }
        public RenderDeferredLiquidEffect() { }
        public RenderDeferredLiquidEffect(BinaryReader binaryReader)
        {
            ReflectionMap = binaryReader.ReadString();
            WaveHeight = binaryReader.ReadSingle();
            WaveSpeedA = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
            WaveSpeedB = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
            WaterReflectiveness = binaryReader.ReadSingle();
            BottomColor = new Color(binaryReader);
            DeepColor = new Color(binaryReader);
            WaterEmissiveAmount = binaryReader.ReadSingle();
            WaterSpecularAmount = binaryReader.ReadSingle();
            WaterSpecularPower = binaryReader.ReadSingle();
            BottomTexture = binaryReader.ReadString();
            WaterNormalMap = binaryReader.ReadString();
            IceReflectiveness = binaryReader.ReadSingle();
            IceColor = new Color(binaryReader);
            IceEmissiveAmount = binaryReader.ReadSingle();
            IceSpecularAmount = binaryReader.ReadSingle();
            IceSpecularPower = binaryReader.ReadSingle();
            IceDiffuseMap = binaryReader.ReadString();
            IceNormalMap = binaryReader.ReadString();
        }
        public override void Write(BinaryWriter binaryWriter)
        {
            base.Write(binaryWriter);
            binaryWriter.Write(ReflectionMap);
            binaryWriter.Write(WaveHeight);
            WaveSpeedA.Write(binaryWriter);
            WaveSpeedB.Write(binaryWriter);
            binaryWriter.Write(WaterReflectiveness);
            BottomColor.Write(binaryWriter);
            DeepColor.Write(binaryWriter);
            binaryWriter.Write(WaterEmissiveAmount);
            binaryWriter.Write(WaterSpecularAmount);
            binaryWriter.Write(WaterSpecularPower);
            binaryWriter.Write(BottomTexture);
            binaryWriter.Write(WaterNormalMap);
            binaryWriter.Write(IceReflectiveness);
            IceColor.Write(binaryWriter);
            binaryWriter.Write(IceEmissiveAmount);
            binaryWriter.Write(IceSpecularAmount);
            binaryWriter.Write(IceSpecularPower);
            binaryWriter.Write(IceDiffuseMap);
            binaryWriter.Write(IceNormalMap);
        }
    }
}
