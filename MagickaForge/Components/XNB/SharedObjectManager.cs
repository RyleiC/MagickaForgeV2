using MagickaForge.Components.Graphics.Models;
using MagickaForge.Components.Graphics.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.XNB
{
    public static class SharedObjectManager
    {
        public static List<SharedObjectTicket> Queue = new List<SharedObjectTicket>();
        public static List<ShaderEffect> Effects = new List<ShaderEffect>();
        public static void RequestSharedObject(SharedObjectTicket ticket)
        {
            Queue.Add(ticket);
        }
        public static void DistributeSharedObjects(BinaryReader binaryReader, Header header)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects.Add(ShaderEffect.GetEffect(binaryReader, header));
            }
            foreach (SharedObjectTicket ticket in Queue)
            {
                ticket.ModelMeshPart.effect = Effects[ticket.Index];
            }
        }

        public static void WriteSharedObject()
        {
            for (int i = 0; i < Queue.Count; i++)
            {
                
            }
        }
    }
}
