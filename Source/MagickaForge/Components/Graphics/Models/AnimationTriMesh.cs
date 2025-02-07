using MagickaForge.Components.Levels;
using MagickaForge.Utils.Data.Graphics;

namespace MagickaForge.Components.Graphics.Models
{
    public class AnimationTriMesh
    {
        public CollisionMaterial CollisionMaterial { get; set; }
        public TriangleMesh TriangleMesh { get; set; }
        public AnimationTriMesh() { }
        public AnimationTriMesh(BinaryReader binaryReader)
        {
            CollisionMaterial = (CollisionMaterial)binaryReader.ReadByte();
            TriangleMesh = new TriangleMesh(binaryReader);
        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write((byte)CollisionMaterial);
            TriangleMesh.Write(binaryWriter);
        }
    }
}
