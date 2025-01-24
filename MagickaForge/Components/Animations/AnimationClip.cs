using MagickaForge.Utils.Data.Animations;

namespace MagickaForge.Components.Animations
{
    public class AnimationClip
    {
        public string AnimationType { get; set; }
        public string AnimationKey { get; set; }
        public float AnimationSpeed { get; set; }
        public float BlendTime { get; set; }
        public bool Loop { get; set; }
        public AnimationAction[] AnimationActions { get; set; }

        public AnimationClip() { }
        public AnimationClip(BinaryReader br)
        {
            AnimationType = br.ReadString();
            AnimationKey = br.ReadString();
            AnimationSpeed = br.ReadSingle();
            BlendTime = br.ReadSingle();
            Loop = br.ReadBoolean();
            AnimationActions = new AnimationAction[br.ReadInt32()];
            for (int i = 0; i < AnimationActions.Length; i++)
            {
                ActionType actionType = Enum.Parse<ActionType>(br.ReadString(), true);
                AnimationActions[i] = AnimationAction.GetAction(actionType, br);
            }
        }
        public void Write(BinaryWriter bw)
        {
            bw.Write(AnimationType!);
            bw.Write(AnimationKey!);
            bw.Write(AnimationSpeed);
            bw.Write(BlendTime);
            bw.Write(Loop);
            bw.Write(AnimationActions!.Length);
            foreach (AnimationAction action in AnimationActions)
            {
                action.Write(bw);
            }
        }
    }
}