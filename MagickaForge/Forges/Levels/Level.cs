using MagickaForge.Components.Levels;
using MagickaForge.Components.Levels.LevelEntities;
using MagickaForge.Components.Levels.Liquid;
using MagickaForge.Components.XNB;
using System.IO;

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
        private Header header;
        private BinTreeModel model;
        private Light[] lights;
        private Effect[] effects;
        private PhysicsEntity[] physicsEntities;
        private LiquidDeclaration[] liquids;
        private ForceField[] forceFields;
        private TriangleMesh[] collisionMeshes;
        private TriangleMesh cameraMesh;
        private TriggerArea[] triggerAreas;
        private Locator[] locators;

        public void LevelToXNB(string outputPath)
        {
         /*   BinaryWriter bw = new(File.Open(outputPath, FileMode.Create));

            header.Write(bw);
            bw.Write7BitEncodedInt(header.GetReaderIndex(ReaderType.Level));
            bw.Write(header.GetReaderIndex(ReaderType.BinaryTree));
            model.Write(bw);

            bw.Close();*/
        }
        public void XNBToLevel(string inputPath)
        {
            BinaryReader br = new(File.Open(inputPath, FileMode.Open));

            header = new Header(br);

            br.ReadByte(); //0 read, will always be the first reader

            for (int i = 0; i < 13; i++) //DEBUGGING
            {
                Console.WriteLine($"{(ReaderType)i}: {header.GetReaderIndex((ReaderType)i)}");
            }

            br.ReadByte(); //GraphicsDevice useless read
            model = new BinTreeModel(br, header); //BINARY TREE

            if (br.ReadInt32() != 0) //ANIMATED LEVEL PARTS
            {
                throw new NotImplementedException("No animated objects hooked up! (yet, come back later!)");
            }
            

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
            for (int i = 0; i < physicsEntities.Length; i++)
            {
                physicsEntities[i] = new PhysicsEntity(br);
            }

            liquids = new LiquidDeclaration[br.ReadInt32()];
            for (int i = 0; i < liquids.Length; i++)
            {
                liquids[i] = new LiquidDeclaration(br, header);
            }
            forceFields = new ForceField[br.ReadInt32()];
            for (int i = 0; i < forceFields.Length; i++)
            {
                forceFields[i] = new ForceField(br);
            }

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
            for (int i = 0; i < triggerAreas.Length; i++)
            {
                triggerAreas[i] = new TriggerArea(br);
            }
            locators = new Locator[br.ReadInt32()];
            for (int i = 0; i < locators.Length; i++)
            {
                locators[i] = new Locator(br);
            }
            Console.WriteLine("YOU DID IT AND DIDN'T DIE YAY!!");
            br.Close();
            LevelToXNB(inputPath.Replace(".xnb", ".mlvl"));
        }
    }
}
