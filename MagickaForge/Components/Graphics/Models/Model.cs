using MagickaForge.Components.Levels.LevelEntities;
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
        public ModelBone Root { get; set; }
        public byte Tag { get; set; }
        public ModelBone[] ModelBones { get; set; }
        public ModelMesh[] Meshes { get; set; }
        public Model() { }

        public Model(BinaryReader binaryReader)
        {
            binaryReader.ReadByte();
            ReadBones(binaryReader);
            VertexDeclaration[] vertexDeclarations = ReadVertexDeclarations(binaryReader);
            ReadMeshes(binaryReader, vertexDeclarations);
            Root = ReadBoneReference(binaryReader);
            Tag = binaryReader.ReadByte();
        }

        private void ReadBones(BinaryReader binaryReader)
        {
            int num = binaryReader.ReadInt32();
            ModelBones = new ModelBone[num];
            for (int i = 0; i < ModelBones.Length; i++)
            {
                ModelBones[i] = new ModelBone(binaryReader);
            }
            foreach (ModelBone modelBone in ModelBones)
            {
                ModelBone parent = ReadBoneReference(binaryReader);
                ModelBone[] children = new ModelBone[binaryReader.ReadInt32()];
                for (int k = 0; k < children.Length; k++)
                {
                    children[k] = ReadBoneReference(binaryReader);
                }
                if (parent != null)
                parent.Children = children;
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
