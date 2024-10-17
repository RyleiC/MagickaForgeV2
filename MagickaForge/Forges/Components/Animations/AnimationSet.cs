namespace MagickaForge.Forges.Components.Animations
{
    public class AnimationSet
    {
        public AnimationClip[]? AnimationClips { get; set; }
        public AnimationSet() { }

        public AnimationSet(BinaryReader br)
        {
            AnimationClips = new AnimationClip[br.ReadInt32()];
            for (int i = 0; i < AnimationClips.Length; i++)
            {
                AnimationClips[i] = new AnimationClip(br);
            }
        }
        public void Write(BinaryWriter bw)
        {
            bw.Write(AnimationClips!.Length);
            foreach (AnimationClip clip in AnimationClips)
            {
                clip.Write(bw);
            }
        }
    }
}