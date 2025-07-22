using MagickaForge.Components;
using MagickaForge.Components.Animations;
using MagickaForge.Components.Characters;
using MagickaForge.Components.Common;
using MagickaForge.Components.Events;
using MagickaForge.Components.Graphics.Models;
using MagickaForge.Components.Levels;
using MagickaForge.Components.XNB;
using MagickaForge.Utils.Data;

namespace MagickaForge.Pipeline.Json.PhysicsEntities
{
    public class PhysicsEntity : PipelineJsonObject
    {
        public const int MaxAnimationSets = 27;
        public const int MaxLights = 0;

        public DynamicHeader Header { get; set; }
        public int ReaderIndex { get; set; }
        public bool Movable { get; set; }
        public bool Pushable { get; set; }
        public bool Solid { get; set; }
        public float Mass { get; set; }
        public int MaxHitpoints { get; set; }
        public int GraphicsDeviceCode { get; set; }
        public Resistance[] Resistances { get; set; }
        public Gib[] Gibs { get; set; }
        public string HitEffect { get; set; }
        public string HitSound { get; set; }
        public string GibTrailEffect { get; set; }
        public Model Model { get; set; }
        public bool UsesMesh { get; set; }
        public TriangleMesh Mesh { get; set; }
        public Box AreaBox { get; set; }
        public VisualEffect[] Effects { get; set; }
        public ConditionCollection[] Conditions { get; set; }
        public string ID { get; set; }
        public float Radius { get; set; }
        public CharacterModel[] SkinnedModels { get; set; }
        public string PrimaryModel { get; set; }
        public BonedEffect[] AttachedEffects { get; set; }
        public AnimationSet[] Animations { get; set; }
        public SharedContentCache[] SharedContent { get; set; }

        protected override void MidExport(BinaryWriter binaryWriter)
        {
            Header.Write(binaryWriter);
            binaryWriter.Write7BitEncodedInt(ReaderIndex);
            binaryWriter.Write(Movable);
            binaryWriter.Write(Pushable);
            binaryWriter.Write(Solid);
            binaryWriter.Write(Mass);
            binaryWriter.Write(MaxHitpoints);
            binaryWriter.Write7BitEncodedInt(GraphicsDeviceCode);
            binaryWriter.Write(Resistances.Length);
            foreach (var resistance in Resistances)
            {
                binaryWriter.Write((int)resistance.Element);
                binaryWriter.Write(resistance.Multiplier);
                binaryWriter.Write(resistance.Modifier);
            }
            binaryWriter.Write(Gibs.Length);
            foreach (var gib in Gibs)
            {
                gib.Write(binaryWriter);
            }
            binaryWriter.Write(HitEffect);
            binaryWriter.Write(HitSound);
            binaryWriter.Write(GibTrailEffect);
            Model.Write(binaryWriter);
            binaryWriter.Write(UsesMesh);
            if (UsesMesh)
            {
                Mesh.Write(binaryWriter);
            }

            var writeBoxes = AreaBox == null ? 0 : 1;
            binaryWriter.Write(writeBoxes);
            if (AreaBox != null)
            {
                binaryWriter.Write("bow_tower0");
                AreaBox.Write(binaryWriter);
            }

            binaryWriter.Write(Effects.Length);
            foreach (var effect in Effects)
            {
                effect.Write(binaryWriter);
            }

            binaryWriter.Write(MaxLights);
            binaryWriter.Write(Conditions.Length);
            foreach (var condition in Conditions)
            {
                condition.Write(binaryWriter);
            }
            binaryWriter.Write(ID);

            var writeSkinnedModels = Radius != 0;
            binaryWriter.Write(writeSkinnedModels);

            if (writeSkinnedModels)
            {
                binaryWriter.Write(Radius);
                if (SkinnedModels == null)
                {
                    binaryWriter.Write(0);
                }
                else
                {
                    binaryWriter.Write(SkinnedModels.Length);
                    foreach (var skinnedModel in SkinnedModels)
                    {
                        binaryWriter.Write(skinnedModel.Model);
                        binaryWriter.Write(skinnedModel.Scale);
                        skinnedModel.Tint.Write(binaryWriter);
                    }
                }
                var hasPrimaryModel = PrimaryModel != null;

                binaryWriter.Write(hasPrimaryModel);
                if (hasPrimaryModel)
                {
                    binaryWriter.Write(PrimaryModel);
                }

                var hasAttachedEffects = AttachedEffects != null && AttachedEffects.Length > 0;

                if (hasAttachedEffects)
                {
                    binaryWriter.Write(AttachedEffects.Length);
                    foreach (var attachedEffect in AttachedEffects)
                    {
                        binaryWriter.Write(attachedEffect.Bone);
                        binaryWriter.Write(attachedEffect.Effect);
                    }
                }
                else
                {
                    binaryWriter.Write(0);
                }

                foreach (var animationSet in Animations)
                {
                    animationSet.Write(binaryWriter);
                }
            }
            for (var i = 0; i < SharedContent!.Length; i++)
            {
                SharedContent[i].Write(binaryWriter);
            }

        }

        protected override void MidImport(BinaryReader binaryReader)
        {
            Header = new DynamicHeader(binaryReader);
            SharedContent = new SharedContentCache[Header.SharedResources];
            ReaderIndex = binaryReader.Read7BitEncodedInt();
            Movable = binaryReader.ReadBoolean();
            Pushable = binaryReader.ReadBoolean();
            Solid = binaryReader.ReadBoolean();
            Mass = binaryReader.ReadSingle();
            MaxHitpoints = binaryReader.ReadInt32();
            GraphicsDeviceCode = binaryReader.Read7BitEncodedInt();
            Resistances = new Resistance[binaryReader.ReadInt32()];
            for (var i = 0; i < Resistances.Length; i++)
            {
                Resistances[i] = new Resistance()
                {
                    Element = (Elements)binaryReader.ReadInt32(),
                    Multiplier = binaryReader.ReadSingle(),
                    Modifier = binaryReader.ReadSingle()
                };
            }

            Gibs = new Gib[binaryReader.ReadInt32()];
            for (var i = 0; i < Gibs.Length; i++)
            {
                Gibs[i] = new Gib()
                {
                    Model = binaryReader.ReadString(),
                    Mass = binaryReader.ReadSingle(),
                    Scale = binaryReader.ReadSingle()
                };
            }

            HitEffect = binaryReader.ReadString();
            HitSound = binaryReader.ReadString();
            GibTrailEffect = binaryReader.ReadString();
            Model = new Model(binaryReader);
            UsesMesh = binaryReader.ReadBoolean();
            if (UsesMesh)
            {
                Mesh = new TriangleMesh(binaryReader);
            }

            var boxes = binaryReader.ReadInt32();
            if (boxes > 0)
            {
                _ = binaryReader.ReadString();
                AreaBox = new Box(binaryReader);

                for (var i = 1; i < boxes; i++)
                {
                    _ = binaryReader.ReadString();
                    _ = new Box(binaryReader);
                }
            }

            Effects = new VisualEffect[binaryReader.ReadInt32()];
            for (var i = 0; i < Effects.Length; i++)
            {
                Effects[i] = new VisualEffect(binaryReader);
            }

            _ = binaryReader.ReadInt32();
            Conditions = new ConditionCollection[binaryReader.ReadInt32()];
            for (var i = 0; i < Conditions.Length; i++)
            {
                Conditions[i] = new ConditionCollection(binaryReader);
            }

            ID = binaryReader.ReadString();

            var hasSkinnedModels = binaryReader.ReadBoolean();
            if (hasSkinnedModels)
            {
                Radius = binaryReader.ReadSingle();
                var numberOfSkinnedModels = binaryReader.ReadInt32();

                if (numberOfSkinnedModels > 0)
                {
                    SkinnedModels = new CharacterModel[numberOfSkinnedModels];
                    for (var i = 0; i < numberOfSkinnedModels; i++)
                    {
                        SkinnedModels[i] = new CharacterModel()
                        {
                            Model = binaryReader.ReadString(),
                            Scale = binaryReader.ReadSingle(),
                            Tint = new Color(binaryReader)
                        };
                    }
                }

                var hasPrimaryModel = binaryReader.ReadBoolean();
                if (hasPrimaryModel)
                {
                    PrimaryModel = binaryReader.ReadString();
                }

                var attachedEffectCount = binaryReader.ReadInt32();
                if (attachedEffectCount > 0)
                {
                    AttachedEffects = new BonedEffect[attachedEffectCount];
                    for (var i = 0; i < AttachedEffects.Length; i++)
                    {
                        AttachedEffects[i] = new BonedEffect()
                        {
                            Bone = binaryReader.ReadString(),
                            Effect = binaryReader.ReadString(),
                        };
                    }
                }

                Animations = new AnimationSet[MaxAnimationSets];
                for (var i = 0; i < Animations.Length; i++)
                {
                    Animations[i] = new AnimationSet(binaryReader);
                }
            }
            for (var i = 0; i < SharedContent.Length; i++)
            {
                SharedContent[i] = new SharedContentCache(binaryReader, Header);
            }
        }

        public class Box
        {
            public Vector3 Position { get; set; }
            public Vector3 Sides { get; set; }
            public Quaternion Rotation { get; set; }

            public Box() { }

            public Box(BinaryReader binaryReader)
            {
                Position = new Vector3(binaryReader);
                Sides = new Vector3(binaryReader);
                Rotation = new Quaternion(binaryReader);
            }

            public void Write(BinaryWriter binaryWriter)
            {
                Position.Write(binaryWriter);
                Sides.Write(binaryWriter);
                Rotation.Write(binaryWriter);
            }
        }

        public class VisualEffect
        {
            public string Effect { get; set; }
            public Matrix Transform { get; set; }

            public VisualEffect() { }

            public VisualEffect(BinaryReader binaryReader)
            {
                Effect = binaryReader.ReadString();
                Transform = new Matrix(binaryReader);
            }

            public void Write(BinaryWriter binaryWriter)
            {
                binaryWriter.Write(Effect);
                Transform.Write(binaryWriter);
            }
        }
    }
}
