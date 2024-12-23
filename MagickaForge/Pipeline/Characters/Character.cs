using MagickaForge.Components;
using MagickaForge.Components.Abilities;
using MagickaForge.Components.Animations;
using MagickaForge.Components.Auras;
using MagickaForge.Components.Characters;
using MagickaForge.Components.Events;
using MagickaForge.Components.Lights;
using MagickaForge.Utils.Data;
using MagickaForge.Utils.Data.Abilities;
using MagickaForge.Utils.Data.AI;
using MagickaForge.Utils.Data.Graphics;
using MagickaForge.Utils.Helpers;
using MagickaForge.Utils.Structures;
using System.Text.Json.Serialization;

namespace MagickaForge.Pipeline.Characters
{
    public class Character : PipelineObject
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

        public override void WriteToXNB(string outputPath)
        {
            BinaryWriter bw = new(File.Create(outputPath));
            bw.Write(XNBHelper.XNBHeader);
            bw.Write(0); //File size placeholder, we go back after the file is written and put in the file size.
            bw.Write(ReaderHeader);

            bw.Write(Name!);
            bw.Write(LocalizedName!);
            bw.Write((int)Faction);
            bw.Write((int)BloodType);
            bw.Write(IsEthereal);
            bw.Write(LooksEthereal);
            bw.Write(Fearless);
            bw.Write(Uncharmable);
            bw.Write(Nonslippery);
            bw.Write(HasFairy);
            bw.Write(CanSeeInvisible);
            bw.Write(Sounds!.Length);
            foreach (Sound sound in Sounds)
            {
                sound.Write(bw);
            }
            bw.Write(Gibs!.Length);
            foreach (Gib gib in Gibs)
            {
                gib.Write(bw);
            }
            if (Lights!.Length > MaxLights)
            {
                throw new CantLoadInMagickaException("Characters may only have 4 up to lights!");
            }
            bw.Write(Lights!.Length);
            foreach (BonedLight light in Lights)
            {
                bw.Write(light.Bone);
                bw.Write(light.Light.Radius);
                light.Light.DiffuseColor.Write(bw);
                light.Light.AmbientColor.Write(bw);
                bw.Write(light.Light.SpecularAmount);
                bw.Write((byte)light.Light.LightVariationType);
                bw.Write(light.Light.VariationAmount);
                bw.Write(light.Light.VariationSpeed);
            }
            bw.Write(MaxHitpoints);
            bw.Write(NumberOfHealthbars);
            bw.Write(Undying);
            bw.Write(UndieTime);
            bw.Write(UndieHitpoints);
            bw.Write(PainTolerance);
            bw.Write(KnockdownTolerance);
            bw.Write(ScoreValue);
            if (CompileForModernMagicka)
            {
                bw.Write(XPValue);
                bw.Write(RewardOnKill);
                bw.Write(RewardOnOverkill);
            }
            bw.Write(Regeneration);
            bw.Write(MaxPanic);
            bw.Write(ZapModifier);
            bw.Write(Length);
            bw.Write(Radius);
            bw.Write(Mass);
            bw.Write(Speed);
            bw.Write(TurnSpeed);
            bw.Write(BleedRate);
            bw.Write(StunTime);
            bw.Write((int)SummonElementBank);
            bw.Write(SummonElementCue!);
            bw.Write(Resistances!.Length);
            foreach (Resistance r in Resistances)
            {
                bw.Write((int)r.Element);
                bw.Write(r.Multiplier);
                bw.Write(r.Modifier);
                bw.Write(r.StatusImmunity);
            }
            bw.Write(Models!.Length);
            foreach (CharacterModel model in Models)
            {
                bw.Write(model.Model!);
                bw.Write(model.Scale);
                model.Tint.Write(bw);
            }
            bw.Write(AnimationSkeleton!);
            bw.Write(Effects!.Length);
            foreach (BonedEffect effect in Effects)
            {
                bw.Write(effect.Bone);
                bw.Write(effect.Effect);
            }
            foreach (AnimationSet animationSet in Animations!)
            {
                animationSet.Write(bw);
            }
            if (Equipment!.Length > MaxEquipmentSlots)
            {
                throw new CantLoadInMagickaException("Characters may only have up to 8 equipment slots!");
            }
            bw.Write(Equipment!.Length);
            foreach (Attachment attachment in Equipment)
            {
                bw.Write(attachment.Slot);
                bw.Write(attachment.Bone);
                attachment.Rotation.Write(bw);
                bw.Write(attachment.Item);
            }
            bw.Write(Conditions!.Length);
            for (var i = 0; i < Conditions.Length; i++)
            {
                Conditions[i].Write(bw);
            }
            bw.Write(AlertRadius);
            bw.Write(GroupChase);
            bw.Write(GroupSeperation);
            bw.Write(GroupCohesion);
            bw.Write(GroupAlignment);
            bw.Write(GroupWander);
            bw.Write(FriendlyAvoidance);
            bw.Write(EnemyAvoidance);
            bw.Write(SightAvoidance);
            bw.Write(DangerAvoidance);
            bw.Write(AngerWeight);
            bw.Write(DistanceWeight);
            bw.Write(HealthWeight);
            bw.Write(Flocking);
            bw.Write(BreakFreeStrength);
            bw.Write(Abilities!.Length);
            foreach (Ability ability in Abilities)
            {
                ability.Write(bw);
            }
            bw.Write(Movement!.Length);
            foreach (Movement move in Movement)
            {
                bw.Write((byte)move.MoveProperty);
                bw.Write(move.Animations!.Length);
                for (int i = 0; i < move.Animations.Length; i++)
                {
                    bw.Write(move.Animations[i]);
                }
            }
            bw.Write(Buffs!.Length);
            foreach (Buff buff in Buffs)
            {
                buff.Write(bw);
            }
            bw.Write(Auras!.Length);
            foreach (Aura aura in Auras)
            {
                aura.Write(bw);
            }

            XNBHelper.WriteFileSize(bw);
            bw.Close();
        }

        public override void ReadFromXNB(string inputPath)
        {
            BinaryReader br = new(XNBHelper.DecompressXNB(inputPath));
            br.BaseStream.Position += XNBHelper.XNBHeader.Length + ReaderHeader.Length + 4;

            Name = br.ReadString();
            LocalizedName = br.ReadString();
            Faction = (Factions)br.ReadInt32();
            BloodType = (BloodType)br.ReadInt32();
            IsEthereal = br.ReadBoolean();
            LooksEthereal = br.ReadBoolean();
            Fearless = br.ReadBoolean();
            Uncharmable = br.ReadBoolean();
            Nonslippery = br.ReadBoolean();
            HasFairy = br.ReadBoolean();
            CanSeeInvisible = br.ReadBoolean();

            Sounds = new Sound[br.ReadInt32()];
            for (var i = 0; i < Sounds.Length; i++)
            {
                Sounds[i].Cue = br.ReadString();
                Sounds[i].Bank = (Banks)br.ReadInt32();
            }

            Gibs = new Gib[br.ReadInt32()];
            for (var i = 0; i < Gibs.Length; i++)
            {
                Gibs[i].Model = br.ReadString();
                Gibs[i].Mass = br.ReadSingle();
                Gibs[i].Scale = br.ReadSingle();
            }
            Lights = new BonedLight[br.ReadInt32()];
            for (var i = 0; i < Lights.Length; i++)
            {
                Lights[i].Bone = br.ReadString();
                var light = new Light()
                {
                    Radius = br.ReadSingle(),
                    DiffuseColor = new Color(br),
                    AmbientColor = new Color(br),
                    SpecularAmount = br.ReadSingle(),
                    LightVariationType = (LightVariationType)br.ReadByte(),
                    VariationAmount = br.ReadSingle(),
                    VariationSpeed = br.ReadSingle()
                };
                Lights[i].Light = light;
            }
            MaxHitpoints = br.ReadSingle();
            NumberOfHealthbars = br.ReadInt32();
            Undying = br.ReadBoolean();
            UndieTime = br.ReadSingle();
            UndieHitpoints = br.ReadSingle();
            PainTolerance = br.ReadInt32();
            KnockdownTolerance = br.ReadSingle();
            ScoreValue = br.ReadInt32();

            if (CompileForModernMagicka)
            {
                XPValue = br.ReadInt32();
                RewardOnKill = br.ReadBoolean();
                RewardOnOverkill = br.ReadBoolean();
            }

            Regeneration = br.ReadInt32();
            MaxPanic = br.ReadSingle();
            ZapModifier = br.ReadSingle();
            Length = br.ReadSingle();
            Radius = br.ReadSingle();
            Mass = br.ReadSingle();
            Speed = br.ReadSingle();
            TurnSpeed = br.ReadSingle();
            BleedRate = br.ReadSingle();
            StunTime = br.ReadSingle();
            SummonElementBank = (Banks)br.ReadInt32();
            SummonElementCue = br.ReadString();
            Resistances = new Resistance[br.ReadInt32()];
            for (var i = 0; i < Resistances.Length; i++)
            {
                Resistances[i].Element = (Elements)br.ReadInt32();
                Resistances[i].Multiplier = br.ReadSingle();
                Resistances[i].Modifier = br.ReadSingle();
                Resistances[i].StatusImmunity = br.ReadBoolean();
            }
            Models = new CharacterModel[br.ReadInt32()];
            for (var i = 0; i < Models.Length; i++)
            {
                Models[i].Model = br.ReadString();
                Models[i].Scale = br.ReadSingle();
                Models[i].Tint = new Color(br);
            }
            AnimationSkeleton = br.ReadString();
            Effects = new BonedEffect[br.ReadInt32()];
            for (var i = 0; i < Effects.Length; i++)
            {
                Effects[i].Bone = br.ReadString();
                Effects[i].Effect = br.ReadString();
            }
            Animations = new AnimationSet[MaxAnimationSets];
            for (var i = 0; i < Animations.Length; i++)
            {
                Animations[i] = new AnimationSet(br);
            }

            Equipment = new Attachment[br.ReadInt32()];
            for (var i = 0; i < Equipment.Length; i++)
            {
                Equipment[i].Slot = br.ReadInt32();
                Equipment[i].Bone = br.ReadString();
                Equipment[i].Rotation = new Vector3(br);
                Equipment[i].Item = br.ReadString();
            }
            Conditions = new ConditionCollection[br.ReadInt32()];
            for (var i = 0; i < Conditions.Length; i++)
            {
                Conditions[i] = new ConditionCollection(br);
            }
            AlertRadius = br.ReadSingle();
            GroupChase = br.ReadSingle();
            GroupSeperation = br.ReadSingle();
            GroupCohesion = br.ReadSingle();
            GroupAlignment = br.ReadSingle();
            GroupWander = br.ReadSingle();
            FriendlyAvoidance = br.ReadSingle();
            EnemyAvoidance = br.ReadSingle();
            SightAvoidance = br.ReadSingle();
            DangerAvoidance = br.ReadSingle();
            AngerWeight = br.ReadSingle();
            DistanceWeight = br.ReadSingle();
            HealthWeight = br.ReadSingle();
            Flocking = br.ReadBoolean();
            BreakFreeStrength = br.ReadSingle();

            Abilities = new Ability[br.ReadInt32()];
            for (var i = 0; i < Abilities.Length; i++)
            {
                AbilityType type = Enum.Parse<AbilityType>(br.ReadString(), true);
                Abilities[i] = Ability.GetAbility(br, type);
            }

            Movement = new Movement[br.ReadInt32()];
            for (var i = 0; i < Movement.Length; i++)
            {
                Movement[i].MoveProperty = (MovementProperties)br.ReadByte();
                Movement[i].Animations = new string[br.ReadInt32()];
                for (var x = 0; x < Movement[i].Animations!.Length; x++)
                {
                    Movement[i].Animations![x] = br.ReadString();
                }
            }

            Buffs = new Buff[br.ReadInt32()];
            for (var i = 0; i < Buffs.Length; i++)
            {
                Buffs[i] = Buff.GetBuff(br);
            }
            Auras = new Aura[br.ReadInt32()];
            for (var i = 0; i < Auras.Length; i++)
            {
                Auras[i] = Aura.GetAura(br);
            }
            br.Close();
        }

    }
}