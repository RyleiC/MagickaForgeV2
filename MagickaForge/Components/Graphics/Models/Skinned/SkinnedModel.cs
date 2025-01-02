namespace MagickaForge.Components.Graphics.Models.Skinned
{
    public class SkinnedModel
    {
        public int ReaderIndex { get; set; }
        public Model Model { get; set; }
        public SkinnedModelBone[] Bones { get; set; }
        public SkinnedModelClip[] Animations { get; set; }
        public int[] SharedBoneReferences { get; set; }
        public int[] SharedClipReferences { get; set; }

        public SkinnedModel() { }
        public SkinnedModel(BinaryReader br)
        {
            ReaderIndex = br.Read7BitEncodedInt();
            Model = new Model(br);
            var numberOfBones = br.ReadInt32();
            Bones = new SkinnedModelBone[numberOfBones];
            SharedBoneReferences = new int[numberOfBones];
            for (int i = 0; i < numberOfBones; i++)
            {
                SharedBoneReferences[i] = br.Read7BitEncodedInt();
            }
            var numberOfClips = br.ReadInt32();
            Animations = new SkinnedModelClip[numberOfClips];
            SharedClipReferences = new int[numberOfClips];
            for (int i = 0; i < numberOfClips; i++)
            {
                SharedClipReferences[i] = br.Read7BitEncodedInt();
            }
        }

        public void ReadXNAnimationContent(BinaryReader binaryReader)
        {
            for (int i = 0; i < Bones.Length; i++)
            {
                Bones[i] = new SkinnedModelBone(binaryReader);
            }
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i] = new SkinnedModelClip(binaryReader);
            }
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(ReaderIndex);
            Model.Write(binaryWriter);
            binaryWriter.Write(Bones.Length);
            foreach (int boneRef in SharedBoneReferences)
            {
                binaryWriter.Write7BitEncodedInt(boneRef);
            }
            binaryWriter.Write(SharedClipReferences.Length);
            foreach (int clipRef in SharedClipReferences)
            {
                binaryWriter.Write7BitEncodedInt(clipRef);
            }
        }

        public void WriteXNAnimationContent(BinaryWriter binaryWriter)
        {
            foreach (var bone in Bones)
            {
                bone.Write(binaryWriter);
            }    
            foreach (var clip in Animations)
            {
                clip.Write(binaryWriter);
            }
        }
    }
}
