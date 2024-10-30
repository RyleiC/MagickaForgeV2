using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Graphics.Effects
{
    public class LavaEffect : ShaderEffect
    {
        public float maskDistortion { get; set; }
        public Vector2 speedA { get; set; }
        public Vector2 speedB { get; set; }
        public float lavaHotEmission { get; set; }
        public float lavaColdEmission { get; set; }
        public float lavaSpecAmount { get; set; }
        public float lavaSpecPower { get; set; }
        public float tempFrequency { get; set; }
        public string toneMap { get; set; }
        public string tempMap { get; set; }
        public string maskMap { get; set; }
        public Vector3 rockColor { get; set; }
        public float rockEmission { get; set; }
        public float rockSpecAmount { get; set; }
        public float rockSpecPower { get; set; }
        public float rockNormalPower { get; set; }
        public string rockTexture { get; set; }
        public string rockNormalMap { get; set; }
        public LavaEffect() { }
        public LavaEffect(BinaryReader binaryReader)
        {
            maskDistortion = binaryReader.ReadSingle();
            speedA = new Vector2(binaryReader);
            speedB = new Vector2(binaryReader);
            lavaHotEmission = binaryReader.ReadSingle();
            lavaColdEmission = binaryReader.ReadSingle();
            lavaSpecAmount = binaryReader.ReadSingle();
            lavaSpecPower = binaryReader.ReadSingle();
            tempFrequency = binaryReader.ReadSingle();
            toneMap = binaryReader.ReadString();
            tempMap = binaryReader.ReadString();
            maskMap = binaryReader.ReadString();
            rockColor = new Vector3(binaryReader);
            rockEmission = binaryReader.ReadSingle();
            rockSpecAmount = binaryReader.ReadSingle();
            rockSpecPower = binaryReader.ReadSingle();
            rockNormalPower = binaryReader.ReadSingle();
            rockTexture = binaryReader.ReadString();
            rockNormalMap = binaryReader.ReadString();
        }
        public override void Write(BinaryWriter binaryWriter)
        {
            base.Write(binaryWriter);
            binaryWriter.Write(maskDistortion);
            speedA.Write(binaryWriter);
            speedB.Write(binaryWriter);
            binaryWriter.Write(lavaHotEmission);
            binaryWriter.Write(lavaColdEmission);
            binaryWriter.Write(lavaSpecAmount);
            binaryWriter.Write(lavaSpecPower);
            binaryWriter.Write(tempFrequency);
            binaryWriter.Write(toneMap);
            binaryWriter.Write(tempMap);
            binaryWriter.Write(maskMap);
            rockColor.Write(binaryWriter);
            binaryWriter.Write(rockEmission);
            binaryWriter.Write(rockSpecAmount);
            binaryWriter.Write(rockSpecPower);
            binaryWriter.Write(rockNormalPower);
            binaryWriter.Write(rockTexture);
            binaryWriter.Write(rockNormalMap);
        }
    }
}
