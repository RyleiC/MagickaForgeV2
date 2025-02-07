using MagickaForge.Components.Common;

namespace MagickaForge.Components.Graphics.Models.Skinned
{
    public class SkinnedModelBone
    {
        public int ReaderIndex { get; set; }
        private ushort BoneIndex { get; set; }
        public string Name { get; set; }
        public Vector3 Translation { get; set; }
        public Quaternion Orientation { get; set; }
        public Vector3 Scale { get; set; }
        public Matrix Matrix { get; set; }
        public int ParentReference { get; set; }
        public int[] ChildrenReferences { get; set; }
        public SkinnedModelBone() { }
        public SkinnedModelBone(BinaryReader br)
        {
            ReaderIndex = br.Read7BitEncodedInt();
            BoneIndex = br.ReadUInt16();
            Name = br.ReadString();
            Translation = new Vector3(br);
            Orientation = new Quaternion(br);
            Scale = new Vector3(br);
            Matrix = new Matrix(br);
            ParentReference = br.Read7BitEncodedInt();
            ChildrenReferences = new int[br.ReadInt32()];
            for (int i = 0; i < ChildrenReferences.Length; i++)
            {
                ChildrenReferences[i] = br.Read7BitEncodedInt();
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write7BitEncodedInt(ReaderIndex);
            bw.Write(BoneIndex);
            bw.Write(Name);
            Translation.Write(bw);
            Orientation.Write(bw);
            Scale.Write(bw);
            Matrix.Write(bw);
            bw.Write7BitEncodedInt(ParentReference);
            bw.Write(ChildrenReferences.Length);
            foreach (int child in ChildrenReferences)
            {
                bw.Write7BitEncodedInt(child);
            }
        }
    }
}
