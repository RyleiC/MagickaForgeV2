using MagickaForge.Components;
using MagickaForge.Components.Abilities;
using MagickaForge.Components.Animations;
using MagickaForge.Components.Auras;
using MagickaForge.Components.Characters;
using MagickaForge.Components.Common;
using MagickaForge.Components.Events;
using MagickaForge.Components.Lights;
using MagickaForge.Utils.Data;
using MagickaForge.Utils.Data.Abilities;
using MagickaForge.Utils.Data.AI;
using MagickaForge.Utils.Data.Graphics;
using MagickaForge.Utils.Helpers;
using System.Text.Json.Serialization;

namespace MagickaForge.Pipeline.Json.Characters
{
    public class Character : PipelineJsonObject
    {
        private const int MaxLights = 4;
        private const int MaxAnimationSets = 27;
        private const int MaxEquipmentSlots = 8;

        private readonly byte[] ReaderHeader =
        [
            0x01, 0x59, 0x4D, 0x61, 0x67, 0x69, 0x63, 0x6B, 0x61, 0x2E, 0x43, 0x6F,
            0x6E, 0x74, 0x65, 0x6E, 0x74, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x73,
            0x2E, 0x43, 0x68, 0x61, 0x72, 0x61, 0x63, 0x74, 0x65, 0x72, 0x54, 0x65,
            0x6D, 0x70, 0x6C, 0x61, 0x74, 0x65, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72,
            0x2C, 0x20, 0x4D, 0x61, 0x67, 0x69, 0x63, 0x6B, 0x61, 0x2C, 0x20, 0x56,
            0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x31, 0x2E, 0x30, 0x2E, 0x30,
            0x2E, 0x30, 0x2C, 0x20, 0x43, 0x75, 0x6C, 0x74, 0x75, 0x72, 0x65, 0x3D,
            0x6E, 0x65, 0x75, 0x74, 0x72, 0x61, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x01
        ];

        [JsonIgnore]
        public bool CompileForModernMagicka { get; set; }
        public string? Name { get; set; }
        public string? LocalizedName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Factions>))]
        public Factions Faction { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<BloodType>))]
        public BloodType BloodType { get; set; }
        public bool IsEthereal { get; set; }
        public bool LooksEthereal { get; set; }
        public bool Fearless { get; set; }
        public bool Uncharmable { get; set; }
        public bool Nonslippery { get; set; }
        public bool HasFairy { get; set; }
        public bool CanSeeInvisible { get; set; }
        public Sound[]? Sounds { get; set; }
        public Gib[]? Gibs { get; set; }
        public BonedLight[]? Lights { get; set; }
        public float MaxHitpoints { get; set; }
        public int NumberOfHealthbars { get; set; }
        public bool Undying { get; set; }
        public float UndieTime { get; set; }
        public float UndieHitpoints { get; set; }
        public int PainTolerance { get; set; }
        public float KnockdownTolerance { get; set; }
        public int ScoreValue { get; set; }

        // Modern Values
        public int XPValue { get; set; }
        public bool RewardOnKill { get; set; }
        public bool RewardOnOverkill { get; set; }

        public int Regeneration { get; set; }
        public float MaxPanic { get; set; }
        public float ZapModifier { get; set; }
        public float Length { get; set; }
        public float Radius { get; set; }
        public float Mass { get; set; }
        public float Speed { get; set; }
        public float TurnSpeed { get; set; }
        public float BleedRate { get; set; }
        public float StunTime { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Banks>))]
        public Banks SummonElementBank { get; set; }
        public string? SummonElementCue { get; set; }
        public Resistance[]? Resistances { get; set; }
        public CharacterModel[]? Models { get; set; }
        public string? AnimationSkeleton { get; set; }
        public BonedEffect[]? Effects { get; set; }
        public AnimationSet[]? Animations { get; set; }
        public Attachment[]? Equipment { get; set; }
        public ConditionCollection[]? Conditions { get; set; }
        public float AlertRadius { get; set; }
        public float GroupChase { get; set; }
        public float GroupSeperation { get; set; }
        public float GroupCohesion { get; set; }
        public float GroupAlignment { get; set; }
        public float GroupWander { get; set; }
        public float FriendlyAvoidance { get; set; }
        public float EnemyAvoidance { get; set; }
        public float SightAvoidance { get; set; }
        public float DangerAvoidance { get; set; }
        public float AngerWeight { get; set; }
        public float DistanceWeight { get; set; }
        public float HealthWeight { get; set; }
        public bool Flocking { get; set; }
        public float BreakFreeStrength { get; set; }
        public Ability[]? Abilities { get; set; }
        public Movement[]? Movement { get; set; }
        public Buff[]? Buffs { get; set; }
        public Aura[]? Auras { get; set; }

        public override void Export(string outputPath)
        {
            using BinaryWriter binaryWriter = new(File.Create(outputPath));
            {
                binaryWriter.Write(XNBHelper.XNBHeader);
                binaryWriter.Write(0); //File size placeholder, we go back after the file is written and put in the file size.
                binaryWriter.Write(ReaderHeader);

                binaryWriter.Write(Name!);
                binaryWriter.Write(LocalizedName!);
                binaryWriter.Write((int)Faction);
                binaryWriter.Write((int)BloodType);
                binaryWriter.Write(IsEthereal);
                binaryWriter.Write(LooksEthereal);
                binaryWriter.Write(Fearless);
                binaryWriter.Write(Uncharmable);
                binaryWriter.Write(Nonslippery);
                binaryWriter.Write(HasFairy);
                binaryWriter.Write(CanSeeInvisible);
                binaryWriter.Write(Sounds!.Length);
                foreach (Sound sound in Sounds)
                {
                    sound.Write(binaryWriter);
                }
                binaryWriter.Write(Gibs!.Length);
                foreach (Gib gib in Gibs)
                {
                    gib.Write(binaryWriter);
                }
                if (Lights!.Length > MaxLights)
                {
                    throw new CantLoadInMagickaException("Characters may only have 4 up to lights!");
                }
                binaryWriter.Write(Lights!.Length);
                foreach (BonedLight light in Lights)
                {
                    binaryWriter.Write(light.Bone);
                    binaryWriter.Write(light.Light.Radius);
                    light.Light.DiffuseColor.Write(binaryWriter);
                    light.Light.AmbientColor.Write(binaryWriter);
                    binaryWriter.Write(light.Light.SpecularAmount);
                    binaryWriter.Write((byte)light.Light.LightVariationType);
                    binaryWriter.Write(light.Light.VariationAmount);
                    binaryWriter.Write(light.Light.VariationSpeed);
                }
                binaryWriter.Write(MaxHitpoints);
                binaryWriter.Write(NumberOfHealthbars);
                binaryWriter.Write(Undying);
                binaryWriter.Write(UndieTime);
                binaryWriter.Write(UndieHitpoints);
                binaryWriter.Write(PainTolerance);
                binaryWriter.Write(KnockdownTolerance);
                binaryWriter.Write(ScoreValue);
                if (CompileForModernMagicka)
                {
                    binaryWriter.Write(XPValue);
                    binaryWriter.Write(RewardOnKill);
                    binaryWriter.Write(RewardOnOverkill);
                }
                binaryWriter.Write(Regeneration);
                binaryWriter.Write(MaxPanic);
                binaryWriter.Write(ZapModifier);
                binaryWriter.Write(Length);
                binaryWriter.Write(Radius);
                binaryWriter.Write(Mass);
                binaryWriter.Write(Speed);
                binaryWriter.Write(TurnSpeed);
                binaryWriter.Write(BleedRate);
                binaryWriter.Write(StunTime);
                binaryWriter.Write((int)SummonElementBank);
                binaryWriter.Write(SummonElementCue!);
                binaryWriter.Write(Resistances!.Length);
                foreach (Resistance r in Resistances)
                {
                    binaryWriter.Write((int)r.Element);
                    binaryWriter.Write(r.Multiplier);
                    binaryWriter.Write(r.Modifier);
                    binaryWriter.Write(r.StatusImmunity);
                }
                binaryWriter.Write(Models!.Length);
                foreach (CharacterModel model in Models)
                {
                    binaryWriter.Write(model.Model!);
                    binaryWriter.Write(model.Scale);
                    model.Tint.Write(binaryWriter);
                }
                binaryWriter.Write(AnimationSkeleton!);
                binaryWriter.Write(Effects!.Length);
                foreach (BonedEffect effect in Effects)
                {
                    binaryWriter.Write(effect.Bone);
                    binaryWriter.Write(effect.Effect);
                }
                foreach (AnimationSet animationSet in Animations!)
                {
                    animationSet.Write(binaryWriter);
                }
                if (Equipment!.Length > MaxEquipmentSlots)
                {
                    throw new CantLoadInMagickaException("Characters may only have up to 8 equipment slots!");
                }
                binaryWriter.Write(Equipment!.Length);
                foreach (Attachment attachment in Equipment)
                {
                    binaryWriter.Write(attachment.Slot);
                    binaryWriter.Write(attachment.Bone);
                    attachment.Rotation.Write(binaryWriter);
                    binaryWriter.Write(attachment.Item);
                }
                binaryWriter.Write(Conditions!.Length);
                for (var i = 0; i < Conditions.Length; i++)
                {
                    Conditions[i].Write(binaryWriter);
                }
                binaryWriter.Write(AlertRadius);
                binaryWriter.Write(GroupChase);
                binaryWriter.Write(GroupSeperation);
                binaryWriter.Write(GroupCohesion);
                binaryWriter.Write(GroupAlignment);
                binaryWriter.Write(GroupWander);
                binaryWriter.Write(FriendlyAvoidance);
                binaryWriter.Write(EnemyAvoidance);
                binaryWriter.Write(SightAvoidance);
                binaryWriter.Write(DangerAvoidance);
                binaryWriter.Write(AngerWeight);
                binaryWriter.Write(DistanceWeight);
                binaryWriter.Write(HealthWeight);
                binaryWriter.Write(Flocking);
                binaryWriter.Write(BreakFreeStrength);
                binaryWriter.Write(Abilities!.Length);
                foreach (Ability ability in Abilities)
                {
                    ability.Write(binaryWriter);
                }
                binaryWriter.Write(Movement!.Length);
                foreach (Movement move in Movement)
                {
                    binaryWriter.Write((byte)move.MoveProperty);
                    binaryWriter.Write(move.Animations!.Length);
                    for (int i = 0; i < move.Animations.Length; i++)
                    {
                        binaryWriter.Write(move.Animations[i]);
                    }
                }
                binaryWriter.Write(Buffs!.Length);
                foreach (Buff buff in Buffs)
                {
                    buff.Write(binaryWriter);
                }
                binaryWriter.Write(Auras!.Length);
                foreach (Aura aura in Auras)
                {
                    aura.Write(binaryWriter);
                }

                XNBHelper.WriteFileSize(binaryWriter);
            };
        }

        public override void Import(string inputPath)
        {
            using (BinaryReader binaryReader = new(XNBHelper.DecompressXNB(inputPath)))
            {
                binaryReader.BaseStream.Position += XNBHelper.XNBHeader.Length + ReaderHeader.Length + 4;

                Name = binaryReader.ReadString();
                LocalizedName = binaryReader.ReadString();
                Faction = (Factions)binaryReader.ReadInt32();
                BloodType = (BloodType)binaryReader.ReadInt32();
                IsEthereal = binaryReader.ReadBoolean();
                LooksEthereal = binaryReader.ReadBoolean();
                Fearless = binaryReader.ReadBoolean();
                Uncharmable = binaryReader.ReadBoolean();
                Nonslippery = binaryReader.ReadBoolean();
                HasFairy = binaryReader.ReadBoolean();
                CanSeeInvisible = binaryReader.ReadBoolean();

                Sounds = new Sound[binaryReader.ReadInt32()];
                for (var i = 0; i < Sounds.Length; i++)
                {
                    Sounds[i].Cue = binaryReader.ReadString();
                    Sounds[i].Bank = (Banks)binaryReader.ReadInt32();
                }

                Gibs = new Gib[binaryReader.ReadInt32()];
                for (var i = 0; i < Gibs.Length; i++)
                {
                    Gibs[i].Model = binaryReader.ReadString();
                    Gibs[i].Mass = binaryReader.ReadSingle();
                    Gibs[i].Scale = binaryReader.ReadSingle();
                }
                Lights = new BonedLight[binaryReader.ReadInt32()];
                for (var i = 0; i < Lights.Length; i++)
                {
                    Lights[i].Bone = binaryReader.ReadString();
                    var light = new Light()
                    {
                        Radius = binaryReader.ReadSingle(),
                        DiffuseColor = new Color(binaryReader),
                        AmbientColor = new Color(binaryReader),
                        SpecularAmount = binaryReader.ReadSingle(),
                        LightVariationType = (LightVariationType)binaryReader.ReadByte(),
                        VariationAmount = binaryReader.ReadSingle(),
                        VariationSpeed = binaryReader.ReadSingle()
                    };
                    Lights[i].Light = light;
                }
                MaxHitpoints = binaryReader.ReadSingle();
                NumberOfHealthbars = binaryReader.ReadInt32();
                Undying = binaryReader.ReadBoolean();
                UndieTime = binaryReader.ReadSingle();
                UndieHitpoints = binaryReader.ReadSingle();
                PainTolerance = binaryReader.ReadInt32();
                KnockdownTolerance = binaryReader.ReadSingle();
                ScoreValue = binaryReader.ReadInt32();

                if (CompileForModernMagicka)
                {
                    XPValue = binaryReader.ReadInt32();
                    RewardOnKill = binaryReader.ReadBoolean();
                    RewardOnOverkill = binaryReader.ReadBoolean();
                }

                Regeneration = binaryReader.ReadInt32();
                MaxPanic = binaryReader.ReadSingle();
                ZapModifier = binaryReader.ReadSingle();
                Length = binaryReader.ReadSingle();
                Radius = binaryReader.ReadSingle();
                Mass = binaryReader.ReadSingle();
                Speed = binaryReader.ReadSingle();
                TurnSpeed = binaryReader.ReadSingle();
                BleedRate = binaryReader.ReadSingle();
                StunTime = binaryReader.ReadSingle();
                SummonElementBank = (Banks)binaryReader.ReadInt32();
                SummonElementCue = binaryReader.ReadString();
                Resistances = new Resistance[binaryReader.ReadInt32()];
                for (var i = 0; i < Resistances.Length; i++)
                {
                    Resistances[i].Element = (Elements)binaryReader.ReadInt32();
                    Resistances[i].Multiplier = binaryReader.ReadSingle();
                    Resistances[i].Modifier = binaryReader.ReadSingle();
                    Resistances[i].StatusImmunity = binaryReader.ReadBoolean();
                }
                Models = new CharacterModel[binaryReader.ReadInt32()];
                for (var i = 0; i < Models.Length; i++)
                {
                    Models[i].Model = binaryReader.ReadString();
                    Models[i].Scale = binaryReader.ReadSingle();
                    Models[i].Tint = new Color(binaryReader);
                }
                AnimationSkeleton = binaryReader.ReadString();
                Effects = new BonedEffect[binaryReader.ReadInt32()];
                for (var i = 0; i < Effects.Length; i++)
                {
                    Effects[i].Bone = binaryReader.ReadString();
                    Effects[i].Effect = binaryReader.ReadString();
                }
                Animations = new AnimationSet[MaxAnimationSets];
                for (var i = 0; i < Animations.Length; i++)
                {
                    Animations[i] = new AnimationSet(binaryReader);
                }

                Equipment = new Attachment[binaryReader.ReadInt32()];
                for (var i = 0; i < Equipment.Length; i++)
                {
                    Equipment[i].Slot = binaryReader.ReadInt32();
                    Equipment[i].Bone = binaryReader.ReadString();
                    Equipment[i].Rotation = new Vector3(binaryReader);
                    Equipment[i].Item = binaryReader.ReadString();
                }
                Conditions = new ConditionCollection[binaryReader.ReadInt32()];
                for (var i = 0; i < Conditions.Length; i++)
                {
                    Conditions[i] = new ConditionCollection(binaryReader);
                }
                AlertRadius = binaryReader.ReadSingle();
                GroupChase = binaryReader.ReadSingle();
                GroupSeperation = binaryReader.ReadSingle();
                GroupCohesion = binaryReader.ReadSingle();
                GroupAlignment = binaryReader.ReadSingle();
                GroupWander = binaryReader.ReadSingle();
                FriendlyAvoidance = binaryReader.ReadSingle();
                EnemyAvoidance = binaryReader.ReadSingle();
                SightAvoidance = binaryReader.ReadSingle();
                DangerAvoidance = binaryReader.ReadSingle();
                AngerWeight = binaryReader.ReadSingle();
                DistanceWeight = binaryReader.ReadSingle();
                HealthWeight = binaryReader.ReadSingle();
                Flocking = binaryReader.ReadBoolean();
                BreakFreeStrength = binaryReader.ReadSingle();

                Abilities = new Ability[binaryReader.ReadInt32()];
                for (var i = 0; i < Abilities.Length; i++)
                {
                    AbilityType type = Enum.Parse<AbilityType>(binaryReader.ReadString(), true);
                    Abilities[i] = Ability.GetAbility(binaryReader, type);
                }

                Movement = new Movement[binaryReader.ReadInt32()];
                for (var i = 0; i < Movement.Length; i++)
                {
                    Movement[i].MoveProperty = (MovementProperties)binaryReader.ReadByte();
                    Movement[i].Animations = new string[binaryReader.ReadInt32()];
                    for (var x = 0; x < Movement[i].Animations!.Length; x++)
                    {
                        Movement[i].Animations![x] = binaryReader.ReadString();
                    }
                }

                Buffs = new Buff[binaryReader.ReadInt32()];
                for (var i = 0; i < Buffs.Length; i++)
                {
                    Buffs[i] = Buff.GetBuff(binaryReader);
                }
                Auras = new Aura[binaryReader.ReadInt32()];
                for (var i = 0; i < Auras.Length; i++)
                {
                    Auras[i] = Aura.GetAura(binaryReader);
                }
            };
        }
    }
}