using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Graphics.Effects
{
    public class LavaEffect : ShaderEffect
    {
        private float maskDistortion;
        private Vector2 speedA;
        private Vector2 speedB;
        private float lavaHotEmission;
        private float lavaColdEmission;
        private float lavaSpecAmount;
        private float lavaSpecPower;
        private float tempFrequency;
        private string toneMap;
        private string tempMap;
        private string maskMap;
        private Vector3 rockColor;
        private float rockEmission;
        private float rockSpecAmount;
        private float rockSpecPower;
        private float rockNormalPower;
        private string rockTexture;
        private string rockNormalMap;

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
            rockColor = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
            rockEmission = binaryReader.ReadSingle();
            rockSpecAmount = binaryReader.ReadSingle();
            rockSpecPower = binaryReader.ReadSingle();
            rockNormalPower = binaryReader.ReadSingle();
            rockTexture = binaryReader.ReadString();
            rockNormalMap = binaryReader.ReadString();
        }
    }
}
