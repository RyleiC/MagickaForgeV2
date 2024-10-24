using MagickaForge.Components.Graphics.Effects;
using MagickaForge.Components.XNB;
using MagickaForge.Components.Levels.LevelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagickaForge.Components.Graphics;
using System.IO;

namespace MagickaForge.Components.Levels.Liquid
{
    public class LiquidDeclaration
    {
        private ShaderEffect effect;
        private Liquid liquid;

        public LiquidDeclaration(BinaryReader br, Header header)
        {
            effect = ShaderEffect.GetEffect(br, header);
            liquid = new Liquid(br);
        }
    }
    public class Liquid
    {
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private VertexDeclaration vertexDeclaration;
        private int vertexStride;
        private int vertexCount;
        private int primativeCount;
        private bool collidable;
        private bool freezable;
        private bool autoFreeze;


        public Liquid(BinaryReader br)
        {
            vertexBuffer = new VertexBuffer(br);
            indexBuffer = new IndexBuffer(br);
            vertexDeclaration = new VertexDeclaration(br);
            vertexStride = br.ReadInt32();
            vertexCount = br.ReadInt32();
            primativeCount = br.ReadInt32();
            collidable = br.ReadBoolean();
            freezable = br.ReadBoolean();
            autoFreeze = br.ReadBoolean();
        }
    }
}
