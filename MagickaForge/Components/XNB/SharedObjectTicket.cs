using MagickaForge.Components.Graphics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.XNB
{
    public class SharedObjectTicket
    {
        public ModelMeshPart ModelMeshPart { get; set; }
        public int Index;
        public SharedObjectTicket(ModelMeshPart mesh, int Index)
        {
            ModelMeshPart = mesh;
            this.Index = Index;
        }
    }
}
