namespace MagickaForge.Components.Graphics.Models.Skinned
{
    public class NamedAnimationChannel
    {
        public string Name { get; set; }
        public AnimationChannel Channel { get; set; }

        public NamedAnimationChannel() { }
        public NamedAnimationChannel(BinaryReader br)
        {
            Name = br.ReadString();
            Channel = new AnimationChannel(br);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Name);
            Channel.Write(bw);

        }
    }
}
