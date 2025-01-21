namespace MagickaForge.Components.Graphics.Models.Skinned
{
    public class AnimationChannel
    {
        public AnimationDataChannel[] DataChannels { get; set; }

        public AnimationChannel() { }
        public AnimationChannel(BinaryReader br)
        {
            DataChannels = new AnimationDataChannel[br.ReadInt32()];
            for (int i = 0; i < DataChannels.Length; i++)
            {
                DataChannels[i] = new AnimationDataChannel(br);
            }
        }
        public void Write(BinaryWriter bw, bool doubleMode)
        {
            bw.Write(DataChannels.Length);
            foreach (AnimationDataChannel channel in DataChannels)
            {
                channel.Write(bw);
            }
        }
    }
}
