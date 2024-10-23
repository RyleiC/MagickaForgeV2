using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class PhysicsEntity
    {
        public float[] position;
        public string type;
        public PhysicsEntity(BinaryReader binaryReader)
        {
            position = new float[16];
            for (int i = 0; i < 16; i++)
            {
                position[i] = binaryReader.ReadSingle();
            }
            type = binaryReader.ReadString();
        }
    }
}
