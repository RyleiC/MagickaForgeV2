using MagickaForge.Components.Levels;
using MagickaForge.Components.Levels.LevelEntities;
using MagickaForge.Components.Levels.Liquid;
using MagickaForge.Components.Levels.Navigation;
using MagickaForge.Components.XNB;
using MagickaForge.Utils.Helpers;

namespace MagickaForge.Pipeline.Levels
{
    public class Level : PipelineObject
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

        public override void WriteToXNB(string outputPath)
        {
            var binaryWriter = new BinaryWriter(File.Create(outputPath));

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
            binaryWriter.Close();
        }

        public override void ReadFromXNB(string inputPath)
        {
            BinaryReader br = new(XNBHelper.DecompressXNB(inputPath));

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
            SharedContent = new SharedContentCache[Header.SharedResources];
            for (var i = 0; i < SharedContent.Length; i++)
            {
                SharedContent[i] = new SharedContentCache(br, Header);
            }
            br.Close();
        }
    }
}
