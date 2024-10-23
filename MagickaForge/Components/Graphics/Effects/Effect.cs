using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Graphics.Effects
{
    public class Effect
    {
        public static Effect GetEffect(BinaryReader binaryReader)
        {
            byte type = binaryReader.ReadByte();
            if (type == 6)
            {
                return new RenderDeferredEffect(binaryReader);
            }
            else if (type == 7)
            {
                return new AdditiveEffect(binaryReader);
            }
        /*  else if (type == 7)
            {
                return new RenderDeferredLiquidEffect(binaryReader);
            }*/
            throw new ArgumentException();
        }
    }
}
