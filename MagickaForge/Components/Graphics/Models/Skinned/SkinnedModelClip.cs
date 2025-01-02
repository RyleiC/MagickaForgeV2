namespace MagickaForge.Components.Graphics.Models.Skinned
{
    public class SkinnedModelClip
    {
        public int ReaderType { get; set; }
        public string Name { get; set; }
        public float Duration { get; set; }
        public NamedAnimationChannel[] NamedChannels { get; set; }
        public SkinnedModelClip() { }
        public SkinnedModelClip(BinaryReader br)
        {
            ReaderType = br.Read7BitEncodedInt();
            Name = br.ReadString();
            Duration = br.ReadSingle();
            NamedChannels = new NamedAnimationChannel[br.ReadInt32()];
            for (int i = 0; i < NamedChannels.Length; i++)
            {
                NamedChannels[i] = new NamedAnimationChannel(br);
            }
        }
        public void Write(BinaryWriter bw)
        {
            bw.Write7BitEncodedInt(ReaderType);
            bw.Write(Name);
            bw.Write(Duration);
            bw.Write(NamedChannels.Length);
            foreach (var channel in NamedChannels)
            {
                channel.Write(bw);
            }
        }
    }
}
