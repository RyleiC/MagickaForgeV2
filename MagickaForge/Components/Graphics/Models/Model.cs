using MagickaForge.Components.Levels.LevelEntities;
using MagickaForge.Components.XNB;
using MagickaForge.Utils.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Graphics.Models
{
    public class Model
    {
        public int writerType { get; set; }
        public ModelBone Root { get; set; }
        public byte Tag { get; set; }
        public ModelBone[] ModelBones { get; set; }
        public ModelMesh[] Meshes { get; set; }
        public Model() { }
        public VertexDeclaration[] vDeclarations { get; set; }

        public Model(BinaryReader binaryReader)
        {
            binaryReader.ReadByte();
            ReadBones(binaryReader);
            VertexDeclaration[] vertexDeclarations = ReadVertexDeclarations(binaryReader);
            ReadMeshes(binaryReader, vertexDeclarations);
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
                binaryWriter.Write(bone.Parent);
                binaryWriter.Write(bone.Children.Length);
                foreach (int child in bone.Children)
                {
                    binaryWriter.Write(child);
                }
            }
            binaryWriter.Write(vDeclarations.Length);
            foreach (VertexDeclaration vd in vDeclarations)
            {
                vd.Write(binaryWriter);
            }
            int value = 0;
            foreach (ModelMesh ms in Meshes)
            {
                binaryWriter.Write(stringReaderIndex);
                binaryWriter.Write(ms.Name);
                binaryWriter.Write(ms.ParentBone.BoneIndex);
                ms.Center.Write(binaryWriter);
                binaryWriter.Write(ms.Radius);
                ms.VertexBuffer.Write(binaryWriter);
                ms.IndexBuffer.Write(binaryWriter);
                binaryWriter.Write7BitEncodedInt(0);
                binaryWriter.Write(ms.Parts.Length);
                foreach (ModelMeshPart part in ms.Parts)
                {
                    binaryWriter.Write(part.streamOffset);
                    binaryWriter.Write(part.baseVertex);
                    binaryWriter.Write(part.numVertices);
                    binaryWriter.Write(part.startIndex);
                    binaryWriter.Write(part.primativeCount);
                    binaryWriter.Write(part.vdIndex);
                    binaryWriter.Write7BitEncodedInt(0);

                }
                value++;
            }
        }
        private void WriteBoneIndex(BinaryWriter binaryWriter, ModelBone bone)
        {
            int num = ModelBones.Length + 1;
            int num2 = (bone == null) ? 0 : (bone.BoneIndex + 1);
            if (num <= 255)
            {
                binaryWriter.Write((byte)num2);
                return;
            }
            binaryWriter.Write(num2);
        }

        private void ReadBones(BinaryReader binaryReader)
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
                ModelBone parent = ReadBoneReference(binaryReader);
                ModelBone[] children = new ModelBone[binaryReader.ReadInt32()];
                for (int k = 0; k < children.Length; k++)
                {
                    children[k] = ReadBoneReference(binaryReader);
                }
                ModelBones[i].Children = new int[children.Length];
                for (int k = 0; k < children.Length; k++)
                {
                    ModelBones[i].Children[k] = children[k].BoneIndex;
                }
                if (parent != null)
                ModelBones[i].Parent = parent.BoneIndex;
            }
        }

        public ModelBone ReadBoneReference(BinaryReader input)
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
                return ModelBones[index - 1];
            }
            return null;
        }

        public VertexDeclaration[] ReadVertexDeclarations(BinaryReader input)
        {
            int num = input.ReadInt32();
            VertexDeclaration[] array = new VertexDeclaration[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = new VertexDeclaration(input);
            }
            vDeclarations = array;
            return array;
        }

        public void ReadMeshes(BinaryReader input, VertexDeclaration[] vertexDeclarations)
        {
            int num = input.ReadInt32();
            ModelMesh[] array = new ModelMesh[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = new ModelMesh(input, this, vertexDeclarations);
            }
            Meshes = array;
        }

        public ModelMeshPart[] ReadMeshParts(BinaryReader binaryReader, VertexBuffer vertexBuffer, IndexBuffer indexBuffer, VertexDeclaration[] vertexDeclarations)
        {
            int num = binaryReader.ReadInt32();
            ModelMeshPart[] meshParts = new ModelMeshPart[num];
            for (int i = 0; i < num; i++)
            {
                meshParts[i] = new ModelMeshPart(binaryReader, vertexDeclarations);
            }
            return meshParts;
        }

    }
}
