using MagickaForge.Components.Levels;
using MagickaForge.Utils.Definitions.Graphics;

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
