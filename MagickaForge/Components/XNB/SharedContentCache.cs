using MagickaForge.Components.Graphics.Effects;
using MagickaForge.Components.Levels.LevelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.XNB
{
    public class SharedContentCache
    {
        public ShaderEffect effect { get; set; }
        public SharedContentCache() { }
        public SharedContentCache(BinaryReader binaryReader, Header header)
        {
            effect = ShaderEffect.GetEffect(binaryReader, header);
        }
        public void Write(BinaryWriter binaryWriter)
        {
            effect.Write(binaryWriter);
        }
    }
}
