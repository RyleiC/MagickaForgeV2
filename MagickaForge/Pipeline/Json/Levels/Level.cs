using MagickaForge.Components.Levels;
using MagickaForge.Components.Levels.LevelEntities;
using MagickaForge.Components.Levels.Liquid;
using MagickaForge.Components.Levels.Navigation;
using MagickaForge.Components.XNB;
using MagickaForge.Utils.Helpers;

namespace MagickaForge.Pipeline.Json.Levels
{
    public class Level : PipelineJsonObject
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
        public SharedContentCache[]? SharedContent { get; set; }

        public override void Export(string outputPath)
        {
            using (var binaryWriter = new BinaryWriter(File.Create(outputPath)))
            {
                Header!.Write(binaryWriter);
                binaryWriter.Write7BitEncodedInt(ReaderIndex);
                BinaryModel!.Write(binaryWriter);
                binaryWriter.Write(Animations!.Length);
                for (var i = 0; i < Animations.Length; i++)
                {
                    Animations[i].Write(binaryWriter);
                }
                binaryWriter.Write(Lights!.Length);
                for (var i = 0; i < Lights.Length; i++)
                {
                    Lights[i].Write(binaryWriter);
                }
                binaryWriter.Write(Effects!.Length);
                for (var i = 0; i < Effects.Length; i++)
                {
                    Effects[i].Write(binaryWriter);
                }
                binaryWriter.Write(PhysicsEntities!.Length);
                for (var i = 0; i < PhysicsEntities.Length; i++)
                {
                    PhysicsEntities[i].Write(binaryWriter);
                }
                binaryWriter.Write(Liquids!.Length);
                for (var i = 0; i < Liquids.Length; i++)
                {
                    Liquids[i].Write(binaryWriter);
                }
                binaryWriter.Write(ForceFields!.Length);
                for (var i = 0; i < ForceFields.Length; i++)
                {
                    ForceFields[i].Write(binaryWriter);
                }
                if (CollisionMeshes!.Length > MaxCollisionMeshes)
                {
                    throw new CantLoadInMagickaException("Levels may only have up to 10 collision meshes!");
                }
                for (var i = 0; i < 10; i++)
                {
                    if (CollisionMeshes[i] == null)
                    {
                        binaryWriter.Write(false);
                        continue;
                    }
                    binaryWriter.Write(true);
                    CollisionMeshes[i].Write(binaryWriter);
                }
                var hasCameraMesh = CameraMesh != null;
                binaryWriter.Write(hasCameraMesh);
                if (hasCameraMesh)
                {
                    CameraMesh!.Write(binaryWriter);
                }
                binaryWriter.Write(TriggerAreas!.Length);
                for (var i = 0; i < TriggerAreas.Length; i++)
                {
                    TriggerAreas[i].Write(binaryWriter);
                }
                binaryWriter.Write(Locators!.Length);
                for (var i = 0; i < Locators.Length; i++)
                {
                    Locators[i].Write(binaryWriter);
                }
                NavigationMesh!.Write(binaryWriter);
                for (var i = 0; i < SharedContent!.Length; i++)
                {
                    SharedContent[i].Write(binaryWriter);
                }
                XNBHelper.WriteFileSize(binaryWriter);
            };
        }

        public override void Import(string inputPath)
        {
            using (var binaryReader = new BinaryReader(XNBHelper.DecompressXNB(inputPath)))
            {
                Header = new Header(binaryReader);

                ReaderIndex = binaryReader.Read7BitEncodedInt(); //0 read, will always be the first reader

                BinaryModel = new BinTreeModel(binaryReader, Header); //BINARY TREE
                Animations = new AnimatedLevelPart[binaryReader.ReadInt32()];
                for (var i = 0; i < Animations.Length; i++)
                {
                    Animations[i] = new AnimatedLevelPart(binaryReader, Header);
                }
                Lights = new SceneLight[binaryReader.ReadInt32()]; //LIGHTS
                for (var i = 0; i < Lights.Length; i++)
                {
                    Lights[i] = new SceneLight(binaryReader);
                }
                Effects = new SceneEffect[binaryReader.ReadInt32()];
                for (var i = 0; i < Effects.Length; i++)
                {
                    Effects[i] = new SceneEffect(binaryReader);
                }
                PhysicsEntities = new PhysicsEntity[binaryReader.ReadInt32()];
                for (var i = 0; i < PhysicsEntities.Length; i++)
                {
                    PhysicsEntities[i] = new PhysicsEntity(binaryReader);
                }

                Liquids = new LiquidDeclaration[binaryReader.ReadInt32()];
                for (var i = 0; i < Liquids.Length; i++)
                {
                    Liquids[i] = new LiquidDeclaration(binaryReader, Header);
                }
                ForceFields = new ForceField[binaryReader.ReadInt32()];
                for (var i = 0; i < ForceFields.Length; i++)
                {
                    ForceFields[i] = new ForceField(binaryReader);
                }

                CollisionMeshes = new TriangleMesh[10];
                for (var i = 0; i < 10; i++)
                {
                    if (binaryReader.ReadBoolean())
                    {
                        CollisionMeshes[i] = new TriangleMesh(binaryReader);
                    }
                }

                var hasCameraMesh = binaryReader.ReadBoolean();
                if (hasCameraMesh)
                {
                    CameraMesh = new TriangleMesh(binaryReader);
                }
                TriggerAreas = new TriggerArea[binaryReader.ReadInt32()];
                for (var i = 0; i < TriggerAreas.Length; i++)
                {
                    TriggerAreas[i] = new TriggerArea(binaryReader);
                }
                Locators = new Locator[binaryReader.ReadInt32()];
                for (var i = 0; i < Locators.Length; i++)
                {
                    Locators[i] = new Locator(binaryReader);
                }
                NavigationMesh = new NavigationMesh(binaryReader);
                SharedContent = new SharedContentCache[Header.SharedResources];
                for (var i = 0; i < SharedContent.Length; i++)
                {
                    SharedContent[i] = new SharedContentCache(binaryReader, Header);
                }
            };
        }
    }
}
