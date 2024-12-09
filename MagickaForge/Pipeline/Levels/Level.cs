using MagickaForge.Components.Levels;
using MagickaForge.Components.Levels.LevelEntities;
using MagickaForge.Components.Levels.Liquid;
using MagickaForge.Components.Levels.Navigation;
using MagickaForge.Components.XNB;
using MagickaForge.Utils;
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
     * Blender Exporter + blah blah blah X
     * 
     * 
     * Read code should be cleaned up [ ]
     * Add proper shared content [ ]
     * Implement animated models [ ]
     * Insure that the model parts are properly and losslessly serialized [ ]
     */
    public class Level
    {
        private const int MaxCollisionMeshes = 10;
        public int ReaderIndex { get; set; }
        public Header? Header { get; set; }
        public BinTreeModel? BinaryModel { get; set; }
        public AnimatedLevelPart[]? Animations { get; set; }
        public SceneLight[]? Lights { get; set; }
        public SceneEffect[]? Effects { get; set; }
        public PhysicsEntity[]? PhysicsEntities { get; set; }
        public LiquidDeclaration[]? Liquids { get; set; }
        public ForceField[]? ForceFields { get; set; }
        public TriangleMesh[]? CollisionMeshes { get; set; }
        public TriangleMesh? CameraMesh { get; set; }
        public TriggerArea[]? TriggerAreas { get; set; }
        public Locator[]? Locators { get; set; }
        public NavigationMesh? NavigationMesh { get; set; }
        public SharedContentCache[]? ContentCache { get; set; }

        public void LevelToXNB(string outputPath)
        {
            GLTFHook gLTFHook = new GLTFHook();
            BinaryWriter bw = new BinaryWriter(File.Create(outputPath));
            BinaryModel.BinaryTreeRoots = new BinTreeRoot[] { gLTFHook.Root };
            Header!.Write(bw);
            bw.Write7BitEncodedInt(ReaderIndex);
            BinaryModel!.Write(bw);
            bw.Write(Animations!.Length);
            for (var i = 0; i < Animations.Length; i++)
            {
                Animations[i].Write(bw);
            }
            bw.Write(Lights!.Length);
            for (var i = 0; i < Lights.Length; i++)
            {
                Lights[i].Write(bw);
            }
            bw.Write(Effects!.Length);
            for (var i = 0; i < Effects.Length; i++)
            {
                Effects[i].Write(bw);
            }
            bw.Write(PhysicsEntities!.Length);
            for (var i = 0; i < PhysicsEntities.Length; i++)
            {
                PhysicsEntities[i].Write(bw);
            }
            bw.Write(Liquids!.Length);
            for (var i = 0; i < Liquids.Length; i++)
            {
                Liquids[i].Write(bw);
            }
            bw.Write(ForceFields!.Length);
            for (var i = 0; i < ForceFields.Length; i++)
            {
                ForceFields[i].Write(bw);
            }
            if (CollisionMeshes!.Length > MaxCollisionMeshes)
            {
                throw new ArgumentOutOfRangeException("Levels may only have up to 10 collision meshes!");
            }
            CollisionMeshes[0] = gLTFHook.Collision;
            for (var i = 0; i < 10; i++)
            {
                if (CollisionMeshes[i] == null)
                {
                    bw.Write(false);
                    continue;
                }
                bw.Write(true);
                CollisionMeshes[i].Write(bw);
            }
            var hasCameraMesh = CameraMesh != null;
            bw.Write(hasCameraMesh);
            if (hasCameraMesh)
            {
                CameraMesh!.Write(bw);
            }
            bw.Write(TriggerAreas!.Length);
            for (var i = 0; i < TriggerAreas.Length; i++)
            {
                TriggerAreas[i].Write(bw);
            }
            bw.Write(Locators!.Length);
            for (var i = 0; i < Locators.Length; i++)
            {
                Locators[i].Write(bw);
            }
            NavigationMesh!.Write(bw);
            for (var i = 0; i < ContentCache!.Length; i++)
            {
                ContentCache[i].Write(bw);
            }
            XNBHelper.WriteFileSize(bw);
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
            BinaryReader br = new(XNBDecompressor.DecompressXNB(inputPath));

            Header = new Header(br);

            ReaderIndex = br.Read7BitEncodedInt(); //0 read, will always be the first reader

            BinaryModel = new BinTreeModel(br, Header); //BINARY TREE
            Animations = new AnimatedLevelPart[br.ReadInt32()];
            for (var i = 0; i < Animations.Length; i++)
            {
                Animations[i] = new AnimatedLevelPart(br, Header);
            }
            Lights = new SceneLight[br.ReadInt32()]; //LIGHTS
            for (var i = 0; i < Lights.Length; i++)
            {
                Lights[i] = new SceneLight(br);
            }
            Effects = new SceneEffect[br.ReadInt32()];
            for (var i = 0; i < Effects.Length; i++)
            {
                Effects[i] = new SceneEffect(br);
            }
            PhysicsEntities = new PhysicsEntity[br.ReadInt32()];
            for (var i = 0; i < PhysicsEntities.Length; i++)
            {
                PhysicsEntities[i] = new PhysicsEntity(br);
            }

            Liquids = new LiquidDeclaration[br.ReadInt32()];
            for (var i = 0; i < Liquids.Length; i++)
            {
                Liquids[i] = new LiquidDeclaration(br, Header);
            }
            ForceFields = new ForceField[br.ReadInt32()];
            for (var i = 0; i < ForceFields.Length; i++)
            {
                ForceFields[i] = new ForceField(br);
            }

            CollisionMeshes = new TriangleMesh[10];
            for (var i = 0; i < 10; i++)
            {
                if (br.ReadBoolean())
                {
                    CollisionMeshes[i] = new TriangleMesh(br);
                }
            }

            var hasCameraMesh = br.ReadBoolean();
            if (hasCameraMesh)
            {
                CameraMesh = new TriangleMesh(br);
            }
            TriggerAreas = new TriggerArea[br.ReadInt32()];
            for (var i = 0; i < TriggerAreas.Length; i++)
            {
                TriggerAreas[i] = new TriggerArea(br);
            }
            Locators = new Locator[br.ReadInt32()];
            for (var i = 0; i < Locators.Length; i++)
            {
                Locators[i] = new Locator(br);
            }
            NavigationMesh = new NavigationMesh(br);
            ContentCache = new SharedContentCache[Header.SharedResources];
            for (var i = 0; i < ContentCache.Length; i++)
            {
                ContentCache[i] = new SharedContentCache(br, Header);
            }
            br.Close();
        }
    }
}
