namespace MagickaForge.Components.Graphics.Models
{
    public class Model
    {
        public int writerType { get; set; }
        public int Root { get; set; }
        public byte Tag { get; set; }
        public ModelBone[] ModelBones { get; set; }
        public ModelMesh[] Meshes { get; set; }
        public Model() { }
        public VertexDeclaration[] vDeclarations { get; set; }

        public Model(BinaryReader binaryReader)
        {
            writerType = binaryReader.Read7BitEncodedInt();
            ReadBones(binaryReader);
            ReadVertexDeclarations(binaryReader);
            ReadMeshes(binaryReader, vDeclarations);
            Root = ReadBoneReference(binaryReader);
            Tag = binaryReader.ReadByte();
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(writerType);
            binaryWriter.Write(ModelBones.Length);
            var stringReaderIndex = 0;

            foreach (var bone in ModelBones)
            {
                stringReaderIndex = bone.readerType;
                binaryWriter.Write7BitEncodedInt(bone.readerType);
                binaryWriter.Write(bone.Name);
                bone.Transform.Write(binaryWriter);
            }
            foreach (var bone in ModelBones)
            {
                WriteBoneIndexes(binaryWriter, bone.Parent);
                binaryWriter.Write(bone.Children.Length);
                foreach (int child in bone.Children)
                {
                    WriteBoneIndexes(binaryWriter, child);
                }
            }
            binaryWriter.Write(vDeclarations.Length);
            foreach (VertexDeclaration vd in vDeclarations)
            {
                vd.Write(binaryWriter);
            }
            binaryWriter.Write(Meshes.Length);
            for (var i = 0; i < Meshes.Length; i++)
            {
                binaryWriter.Write7BitEncodedInt(stringReaderIndex);
                binaryWriter.Write(Meshes[i].Name);
                WriteBoneIndexes(binaryWriter, Meshes[i].ParentBone);
                Meshes[i].Center.Write(binaryWriter);
                binaryWriter.Write(Meshes[i].Radius);
                Meshes[i].VertexBuffer.Write(binaryWriter);
                Meshes[i].IndexBuffer.Write(binaryWriter);
                binaryWriter.Write7BitEncodedInt(0);
                binaryWriter.Write(Meshes[i].Parts.Length);
                foreach (ModelMeshPart part in Meshes[i].Parts)
                {
                    binaryWriter.Write(part.streamOffset);
                    binaryWriter.Write(part.baseVertex);
                    binaryWriter.Write(part.numVertices);
                    binaryWriter.Write(part.startIndex);
                    binaryWriter.Write(part.primativeCount);
                    binaryWriter.Write(part.vdIndex);
                    binaryWriter.Write(part.Tag); //TAG
                    binaryWriter.Write7BitEncodedInt(part.SharedContentID); //EFFECT 
                }
            }
            WriteBoneIndexes(binaryWriter, Root);
            binaryWriter.Write(Tag);
        }

        public void WriteBoneIndexes(BinaryWriter binaryWriter, int BoneIndex)
        {
            if (ModelBones.Length + 1 <= 255)
            {
                binaryWriter.Write((byte)(BoneIndex + 1));
                return;
            }
            binaryWriter.Write(BoneIndex + 1);
        }

        public void ReadBones(BinaryReader binaryReader)
        {
            int num = binaryReader.ReadInt32();
            ModelBones = new ModelBone[num];
            for (int i = 0; i < ModelBones.Length; i++)
            {
                ModelBones[i] = new ModelBone(binaryReader);
                ModelBones[i].BoneIndex = i;
            }
            for (var i = 0; i < ModelBones.Length; i++)
            {
                int parent = ReadBoneReference(binaryReader);
                int[] children = new int[binaryReader.ReadInt32()];
                for (int k = 0; k < children.Length; k++)
                {
                    children[k] = ReadBoneReference(binaryReader);
                }
                ModelBones[i].Children = children;
                ModelBones[i].Parent = parent;
            }
        }

        public int ReadBoneReference(BinaryReader input)
        {
            var num = ModelBones.Length + 1;
            int index;
            if (num <= 255)
            {
                index = (int)input.ReadByte();
            }
            else
            {
                index = input.ReadInt32();
            }
            if (index != 0)
            {
                return index - 1;
            }
            return -1;
        }

        public void ReadVertexDeclarations(BinaryReader input)
        {
            vDeclarations = new VertexDeclaration[input.ReadInt32()];
            for (int i = 0; i < vDeclarations.Length; i++)
            {
                vDeclarations[i] = new VertexDeclaration(input);
            }
        }

        public void ReadMeshes(BinaryReader input, VertexDeclaration[] vertexDeclarations)
        {
            Meshes = new ModelMesh[input.ReadInt32()];
            for (int i = 0; i < Meshes.Length; i++)
            {
                Meshes[i] = new ModelMesh(input, this, vertexDeclarations);
            }
        }

        public ModelMeshPart[] ReadMeshParts(BinaryReader binaryReader, VertexBuffer vertexBuffer, IndexBuffer indexBuffer, VertexDeclaration[] vertexDeclarations)
        {
            ModelMeshPart[] meshParts = new ModelMeshPart[binaryReader.ReadInt32()];
            for (int i = 0; i < meshParts.Length; i++)
            {
                meshParts[i] = new ModelMeshPart(binaryReader, vertexDeclarations);
            }
            return meshParts;
        }

    }
}
