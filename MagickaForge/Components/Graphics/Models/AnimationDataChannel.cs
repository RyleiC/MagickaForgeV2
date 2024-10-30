using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Graphics.Models
{
    public struct AnimationDataChannel
    {
        public float Time { get; set; }
        public Vector3 Translation { get; set; }
        public Quaternion Orientation { get; set; }
        public Vector3 Scale { get; set; }

        public AnimationDataChannel(BinaryReader binaryReader)
        {
            Time = binaryReader.ReadSingle();
            Translation = new Vector3(binaryReader);
            Orientation = new Quaternion(binaryReader);
            Scale = new Vector3(binaryReader);
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Time);
            Translation.Write(binaryWriter);
            Orientation.Write(binaryWriter);
            Scale.Write(binaryWriter);
        }
    }
}
