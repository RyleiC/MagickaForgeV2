using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagickaForge.Components.Levels;
using MagickaForge.Components.Levels.LevelEntities;

namespace MagickaForge.Forges.Levels
{
    /*
     * TODO:
     * Implement non-static header & remove hardcodings
     * Implement Effect options (LavaEffect & RenderDefferredLiquid), test AdditiveEffect
     * Implement Liquids class
     * Implement Model class
     * Implement Force Fields class
     * Implement Animated Objects
     * JSON Serializable?
     * Blender Exporter + blah blah blah
     */
    public class Level
    {
        private BinTreeModel model;
        private Light[] lights;
        private Effect[] effects;
        private PhysicsEntity[] physicsEntities;
        private TriangleMesh[] collisionMeshes;
        private TriangleMesh cameraMesh;
        private TriggerArea[] triggerAreas;
        private Locator[] locators;
        
        public void XNBToLevel(string inputPath)
        {
            BinaryReader br = new(File.Open(inputPath, FileMode.Open));
            br.ReadByte();
            br.ReadBytes(617); //to replace, hardcoded for now

            model = new BinTreeModel(br); //BINARY TREE

            br.ReadInt32(); //ANIMATED LEVEL PARTS

            lights = new Light[br.ReadInt32()]; //LIGHTS
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i] = new Light(br);
            }
            effects = new Effect[br.ReadInt32()];
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i] = new Effect(br);
            }
            physicsEntities = new PhysicsEntity[br.ReadInt32()];
            for (int i = 0; i< physicsEntities.Length; i++)
            {
                physicsEntities[i] = new PhysicsEntity(br);
            }

            br.ReadInt32(); //LIQUIDS
            br.ReadInt32(); //FORCE FIELDS

            collisionMeshes = new TriangleMesh[10];
            for (int i = 0; i < 10; i++)
            {
                collisionMeshes[i] = new TriangleMesh();
                if (br.ReadBoolean())
                {
                    collisionMeshes[i] = new TriangleMesh(br);
                }
            }

            var hasCameraMesh = br.ReadBoolean();
            if (hasCameraMesh)
            {
                cameraMesh = new TriangleMesh(br);
            }
            triggerAreas = new TriggerArea[br.ReadInt32()];
            for (int i = 0;i < triggerAreas.Length; i++)
            {
                triggerAreas[i] = new TriggerArea(br);
            }
            locators = new Locator[br.ReadInt32()];
            for ( int i = 0; i < locators.Length; i++)
            {
                locators[i] = new Locator(br);
            }
            Console.WriteLine("YOU DID IT AND DIDN'T DIE YAY!!");
            br.Close();
        }
        public void LevelToXNB(string inputPath)
        {

        }
    }
}
