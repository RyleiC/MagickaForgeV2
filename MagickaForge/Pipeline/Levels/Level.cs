using MagickaForge.Components.Levels;
using MagickaForge.Components.Levels.LevelEntities;
using MagickaForge.Components.Levels.Liquid;
using MagickaForge.Components.Levels.Navigation;
using MagickaForge.Components.XNB;
using System.Text.Json;

namespace MagickaForge.Pipeline.Levels
{
    /*
     * TODO:
     * Implement non-static header & remove hardcodings X
     * Implement Effect options (LavaEffect & RenderDefferredLiquid), test AdditiveEffect X
     * Implement Liquids class X
     * Implement Model class X
     * Implement Force Fields class X
     * Implement Animated Objects X WOOH
     * JSON Serializable? X
     * Blender Exporter + blah blah blah
     * 
     * 
     * Read code should be cleaned up [ ]
     * Add proper shared content [ ]
     * Implement animated models [ ]
     * Insure that the model parts are properly and losslessly serialized [ ]
     */
    public class Level
    {
        public int readerIndex { get; set; }
        public Header header { get; set; }
        public BinTreeModel model { get; set; }
        public AnimatedLevelPart[] animations { get; set; }
        public Light[] lights { get; set; }
        public Effect[] effects { get; set; }
        public PhysicsEntity[] physicsEntities { get; set; }
        public LiquidDeclaration[] liquids { get; set; }
        public ForceField[] forceFields { get; set; }
        public TriangleMesh[] collisionMeshes { get; set; }
        public TriangleMesh cameraMesh { get; set; }
        public TriggerArea[] triggerAreas { get; set; }
        public Locator[] locators { get; set; }
        public NavigationMesh navigationMesh { get; set; }
        public SharedContentCache[] contentCache { get; set; }

        public void LevelToXNB(string outputPath)
        {
            BinaryWriter bw = new BinaryWriter(File.Create(outputPath));
            header.Write(bw);
            bw.Write7BitEncodedInt(readerIndex);
            model.Write(bw);
            bw.Write(animations.Length);
            for (var i = 0; i < animations.Length; i++)
            {
                animations[i].Write(bw);
            }
            bw.Write(lights.Length);
            for (var i = 0; i < lights.Length; i++)
            {
                lights[i].Write(bw);
            }
            bw.Write(effects.Length);
            for (var i = 0; i < effects.Length; i++)
            {
                effects[i].Write(bw);
            }
            bw.Write(physicsEntities.Length);
            for (var i = 0; i < physicsEntities.Length; i++)
            {
                physicsEntities[i].Write(bw);
            }
            bw.Write(liquids.Length);
            for (var i = 0; i < liquids.Length; i++)
            {
                liquids[i].Write(bw);
            }
            bw.Write(forceFields.Length);
            for (var i = 0; i < forceFields.Length; i++)
            {
                forceFields[i].Write(bw);
            }
            for (int i = 0; i < 10; i++)
            {
                if (collisionMeshes[i] == null)
                {
                    bw.Write(false);
                    continue;
                }
                bw.Write(true);
                collisionMeshes[i].Write(bw);
            }
            var hasCameraMesh = cameraMesh != null;
            bw.Write(hasCameraMesh);
            if (hasCameraMesh)
            {
                cameraMesh.Write(bw);
            }
            bw.Write(triggerAreas.Length);
            for (var i = 0; i < triggerAreas.Length; i++)
            {
                triggerAreas[i].Write(bw);
            }
            bw.Write(locators.Length);
            for (var i = 0; i < locators.Length; i++)
            {
                locators[i].Write(bw);
            }
            navigationMesh.Write(bw);
            for (var i = 0; i < contentCache.Length; i++)
            {
                contentCache[i].Write(bw);
            }
            bw.Close();
        }


        public static void WriteToJson(string outputPath, Level level)
        {
            StreamWriter sw = new(outputPath);
            sw.Write(JsonSerializer.Serialize(level, new JsonSerializerOptions { WriteIndented = true, }));
            sw.Close();
        }

        public static Level LoadFromJson(string inputPath)
        {
            StreamReader sr = new(inputPath);
            string json = sr.ReadToEnd();
            sr.Close();
            return JsonSerializer.Deserialize<Level>(json)!;
        }

        public void XNBToLevel(string inputPath)
        {
            BinaryReader br = new(File.Open(inputPath, FileMode.Open));

            header = new Header(br);

            readerIndex = br.Read7BitEncodedInt(); //0 read, will always be the first reader

            model = new BinTreeModel(br, header); //BINARY TREE

            animations = new AnimatedLevelPart[br.ReadInt32()];
            for (int i = 0; i < animations.Length; i++)
            {
                animations[i] = new AnimatedLevelPart(br, header);
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
            navigationMesh = new NavigationMesh(br);
            contentCache = new SharedContentCache[header.sharedResources];
            for (int i = 0; i < contentCache.Length; i++)
            {
                contentCache[i] = new SharedContentCache(br, header);
            }
            br.Close();
        }
    }
}
