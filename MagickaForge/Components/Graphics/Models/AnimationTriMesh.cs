using MagickaForge.Components.Levels;
using MagickaForge.Utils.Definitions.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Graphics.Models
{
    public class AnimationTriMesh
    {
        public CollisionMaterial collisionMaterial { get; set; }
        public TriangleMesh triangleMesh { get; set; }
        public AnimationTriMesh() { }
        public AnimationTriMesh(BinaryReader binaryReader)
        {
            collisionMaterial = (CollisionMaterial)binaryReader.ReadByte();
            triangleMesh = new TriangleMesh(binaryReader);
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write((byte)collisionMaterial);
            triangleMesh.Write(binaryWriter);
        }
    }
}
