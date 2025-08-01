﻿using MagickaForge.Components.Levels;
using MagickaForge.Components.Levels.LevelEntities;
using MagickaForge.Components.Levels.Liquid;
using MagickaForge.Components.Levels.Navigation;
using MagickaForge.Components.XNB;

namespace MagickaForge.Pipeline.Json.Levels
{
    public class Level : PipelineJsonObject
    {
        public const int MaxCollisionMeshes = 10;

        public int ReaderIndex { get; set; }
        public DynamicHeader Header { get; set; }
        public BinTreeModel BinaryModel { get; set; }
        public AnimatedLevelPart[] Animations { get; set; }
        public SceneLight[] Lights { get; set; }
        public SceneEffect[] Effects { get; set; }
        public PhysicsEntityEntry[] PhysicsEntities { get; set; }
        public LiquidDeclaration[] Liquids { get; set; }
        public ForceField[] ForceFields { get; set; }
        public TriangleMesh[] CollisionMeshes { get; set; }
        public TriangleMesh CameraMesh { get; set; }
        public TriggerArea[] TriggerAreas { get; set; }
        public Locator[] Locators { get; set; }
        public NavigationMesh NavigationMesh { get; set; }
        public SharedContentCache[] SharedContent { get; set; }

        protected override void MidExport(BinaryWriter binaryWriter)
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
            for (var i = 0; i < MaxCollisionMeshes; i++)
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
        }

        protected override void MidImport(BinaryReader binaryReader)
        {
            Header = new DynamicHeader(binaryReader);

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
            PhysicsEntities = new PhysicsEntityEntry[binaryReader.ReadInt32()];
            for (var i = 0; i < PhysicsEntities.Length; i++)
            {
                PhysicsEntities[i] = new PhysicsEntityEntry(binaryReader);
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

            CollisionMeshes = new TriangleMesh[MaxCollisionMeshes];
            for (var i = 0; i < MaxCollisionMeshes; i++)
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
        }
    }
}
