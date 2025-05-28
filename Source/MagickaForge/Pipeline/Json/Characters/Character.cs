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
using System.Text.Json.Serialization;

namespace MagickaForge.Pipeline.Json.Characters
{
    public class Character : PipelineJsonObject
    {
        /// <summary>
        /// Maximum number of lights attached lights a character can have. This is set to 4 as per the original Magicka game limitations.
        /// </summary>
        public const int MaxLights = 4;
        /// <summary>
        /// Maximum number of animation sets a character can have. This is set to 27 as per the original Magicka game limitations.
        /// </summary>
        public const int MaxAnimationSets = 27;
        /// <summary>
        /// Maximum number of equipment slots a character can have. This is set to 8 as per the original Magicka game limitations.
        /// </summary>
        public const int MaxEquipmentSlots = 8;

        /// <summary>
        /// The bytes for the ContentReader header for Characters. This is used to identify the XNB type when importing.
        /// </summary>
        private static readonly byte[] ReaderHeader =
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

        /// <summary>
        /// There are differences between the modern versions of Magicka and older copies, both of which are frequently modded.
        /// This boolean is required to be accurate to the version from which the XNB was derived from to prevent errors while reading the stream.
        /// </summary>
        [JsonIgnore]
        public bool CompileForModernMagicka { get; set; }
        /// <summary>
        /// The ID of the character.
        /// </summary>
        /// <remarks>It is recommended to keep this the same as the filename. Ensure that you don't use the same ID as another character or it will cause Magicka to crash.</remarks>
        public string Name { get; set; }
        /// <summary>
        /// The localized name of the character. These start with a "#" symbol and link up to localization tables found in Content/Languages/*/
        /// </summary>
        public string LocalizedName { get; set; }
        /// <summary>
        /// The faction of the character. This determines what targets they select, with Friendly factions attacking Evil and vice versa.
        /// </summary>
        /// <remarks>None means they will freely attack and be attacked by any faction.</remarks>
        [JsonConverter(typeof(JsonStringEnumConverter<Factions>))]
        public Factions Faction { get; set; }
        /// <summary>
        /// Blood type determines the type of blood particle that is spilled when the character is damaged.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter<BloodType>))]
        public BloodType BloodType { get; set; }
        /// <summary>
        /// Determines whether or not the character will be Ethereal on spawn. This effect goes away after the enemy makes an action that isn't a movement.
        /// </summary>
        public bool IsEthereal { get; set; }
        /// <summary>
        /// Determines whether or not the character appears Ethereal. This does not affect their collision or grant them invulnerability.
        /// </summary>
        /// <remarks>This has odd visual effects when a character is on fire or wet.</remarks>
        public bool LooksEthereal { get; set; }
        /// <summary>
        /// Determines whether or not the character is affected by the Fear effect.
        /// </summary>
        public bool Fearless { get; set; }
        /// <summary>
        /// Determines whether or not the character can be affected by the Charm effect;
        /// </summary>
        public bool Uncharmable { get; set; }
        /// <summary>
        /// Determines whether or not the character slips on grease.
        /// </summary>
        public bool Nonslippery { get; set; }
        /// <summary>
        /// Determines whether or not the character starts with a fairy.
        /// </summary>
        public bool HasFairy { get; set; }
        /// <summary>
        /// Determines whether or not a character can see targets with the invisibility effect.
        /// </summary>
        public bool CanSeeInvisible { get; set; }
        /// <summary>
        /// An array of sounds that are constantly emitted by the characters.
        /// </summary>
        public Sound[] Sounds { get; set; }
        /// <summary>
        /// A list of models that are violently expelled by the character if they are exploded.
        /// </summary>
        public Gib[] Gibs { get; set; }
        /// <summary>
        /// Lights that are attached to certain bones of the character's model.
        /// </summary>
        /// <exception cref="CantLoadInMagickaException">Thrown if the number of lights exceeds the maximum allowed (4).</exception>"
        public BonedLight[] Lights { get; set; }
        /// <summary>
        /// The maximum HP of the character and their starting health.
        /// </summary>
        public float MaxHitpoints { get; set; }
        /// <summary>
        /// The number of healthbars a character has, setting this beyond the default amount will create the yellow/gray healthbar that certain armored enemies have.
        /// </summary>
        public int NumberOfHealthbars { get; set; }
        /// <summary>
        /// Functions like the Staff of the Dead, preventing character death (besides explosive kills).
        /// </summary>
        public bool Undying { get; set; }
        /// <summary>
        /// How swiftly the character is restored to UndieHitpoints.
        /// </summary>
        public float UndieTime { get; set; }
        /// <summary>
        /// The health restored to the character on a Staff of the Dead revive.
        /// </summary>
        public float UndieHitpoints { get; set; }
        /// <summary>
        /// The maximum amount of damage a character can take in one hit before flinching.
        /// </summary>
        public int PainTolerance { get; set; }
        /// <summary>
        /// The maximum earth magnitude (amount of earth elements conjured) that a character can resist before being knocked down in an area cast.
        /// </summary>
        public float KnockdownTolerance { get; set; }
        /// <summary>
        /// The amount of points awarded on kill in Challenge mode.
        /// </summary>
        public int ScoreValue { get; set; }

        // Modern Values
        /// <summary>
        /// The amount of XP you get in the now defunct tourney mode.
        /// </summary>
        /// <remarks>Modern value, ignored on earlier versions of the game.</remarks>
        public int XPValue { get; set; }
        /// <summary>
        /// Whether or not you get XP on killing a character normally in the now defunct tourney mode.
        /// </summary>
        /// <remarks>Modern value, ignored on earlier versions of the game.</remarks>
        public bool RewardOnKill { get; set; }
        /// <summary>
        /// Whether or not you get XP on killing a character explosively in the now defunct tourney mode.
        /// </summary>
        /// <remarks>Modern value, ignored on earlier versions of the game.</remarks>
        public bool RewardOnOverkill { get; set; }

        /// <summary>
        /// The amount of health the character regenerates per second.
        /// </summary>
        /// <remarks>
        /// Ignored if the character is poisoned or on fire. Will cause the character to instantly die if negative.
        /// </remarks>
        public int Regeneration { get; set; }
        /// <summary>
        /// The maximum panic a character level a character can have while on fire.
        /// </summary>
        public float MaxPanic { get; set; }
        /// <summary>
        /// The animation & movement speed modifier while being electrocuted.
        /// </summary>
        public float ZapModifier { get; set; }
        /// <summary>
        /// The height of the character collision capsule.
        /// </summary>
        public float Length { get; set; }
        /// <summary>
        /// The radius of the character collision capsule.
        /// </summary>
        public float Radius { get; set; }
        /// <summary>
        /// The mass of the character.
        /// </summary>
        public float Mass { get; set; }
        /// <summary>
        /// The movement speed of the character
        /// </summary>
        public float Speed { get; set; }
        /// <summary>
        /// The turning speed of the character.
        /// </summary>
        public float TurnSpeed { get; set; }
        /// <summary>
        /// The damage taken while under effect of the Bleeding effect.
        /// </summary>
        public float BleedRate { get; set; }
        /// <summary>
        /// The duration of any stunning effect on the character.
        /// </summary>
        public float StunTime { get; set; }
        /// <summary>
        /// The wavebank used whenever a character conjures an element.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter<Banks>))]
        public Banks SummonElementBank { get; set; }
        /// <summary>
        /// The sound cue used whenever a character conjures an element.
        /// </summary>
        public string SummonElementCue { get; set; }
        /// <summary>
        /// A list of various resistances & vulnerabilities a character has.
        /// </summary>
        /// <remarks>Although you can use bitflag combinations like "Fire, Poison" avoid doing so and create seperate resistances, doing otherwise may cause crashses</remarks>
        public Resistance[] Resistances { get; set; }
        /// <summary>
        /// An array of models that make up the graphical models for the character. Each model can have a different scale and tint.
        /// </summary>
        /// <remarks>If you're controlling the character the tint is overriden with the player color and the index is set from 1 - 4 for the players respectively. Otherwise NPCs pick a random model.</remarks>
        public CharacterModel[] Models { get; set; }
        /// <summary>
        /// The path to the animation skeleton model used by the character. This is used to determine the bone structure for the character's animations as well as the available AnimationKeys.
        /// </summary>
        public string AnimationSkeleton { get; set; }
        /// <summary>
        /// Attached visual effects that are constantly emitted from a bone on the character.
        /// </summary>
        public BonedEffect[] Effects { get; set; }
        /// <summary>
        /// An array of AnimationSets that define the animations available to the character. Each AnimationSet contains a array of AnimationClips that determine animation actions.
        /// </summary>
        /// <remarks>Each AnimationSet aligns with the WeaponClass enum values. To change the animations depending on weapon class align the index of the array with the desired class.</remarks>
        /// <exception cref="CantLoadInMagickaException">Thrown if you have more than 27 animation sets</exception>
        public AnimationSet[] Animations { get; set; }
        /// <summary>
        /// An array of Attachments that determine items held by the character.
        /// </summary>
        /// <exception cref="CantLoadInMagickaException">Thrown if you have more than 8 item attachments</exception>
        public Attachment[] Equipment { get; set; }
        /// <summary>
        /// Akin to item conditions, these control polymorphic events that are invoked should the conditions be met, such as on death.
        /// </summary>
        public ConditionCollection[] Conditions { get; set; }
        /// <summary>
        /// The radius at which the character can detect other characters and react to them.
        /// </summary>
        public float AlertRadius { get; set; }
        /// <summary>
        /// Determines how coordinated the character moves with other characters in a group that are attacking another faction.
        /// </summary>
        public float GroupChase { get; set; }
        /// <summary>
        /// How far apart characters in a group will try to keep from each other when moving.
        /// </summary>
        public float GroupSeperation { get; set; }
        /// <summary>
        /// How close characters in a group will attempt to stay to eachtother when moving.
        /// </summary>
        public float GroupCohesion { get; set; }
        /// <summary>
        /// How tightly characters in a group will try to align with each other when moving.
        /// </summary>
        public float GroupAlignment { get; set; }
        /// <summary>
        /// The randomness in movement by characters in a group.
        /// </summary>
        public float GroupWander { get; set; }
        /// <summary>
        /// The distance characters will attempt to stay away from their own faction.
        /// </summary>
        public float FriendlyAvoidance { get; set; }
        /// <summary>
        /// The distance characters will attempt to stay away from enemy factions.
        /// </summary>
        public float EnemyAvoidance { get; set; }
        /// <summary>
        /// The distance characters will attempt to avoid line of sight with enemy factions.
        /// </summary>
        public float SightAvoidance { get; set; }
        /// <summary>
        /// The distance characters will attempt to stay away from dangerous projectiles such as bombs, grease, or summon death.
        /// </summary>
        public float DangerAvoidance { get; set; }
        /// <summary>
        /// Determines how much the aggro stat of an enemy affects this characters targeting.
        /// </summary>
        public float AngerWeight { get; set; }
        /// <summary>
        /// Determines how much the distance from this character affects their targeting of other characters.
        /// </summary>
        public float DistanceWeight { get; set; }
        /// <summary>
        /// Determines how much the health of this character affects their targeting of other characters.
        /// </summary>
        public float HealthWeight { get; set; }
        /// <summary>
        /// Determines if the characters use Flocking ai.
        /// </summary>
        public bool Flocking { get; set; }
        /// <summary>
        /// Determines how much struggling is required to break from grapples.
        /// </summary>
        public float BreakFreeStrength { get; set; }
        /// <summary>
        /// An array of abilities selected by the AI agent to use on targets.
        /// </summary>
        public Ability[] Abilities { get; set; }
        /// <summary>
        /// An array of different animations associated with different types of movement, such as jumping.
        /// </summary>
        public Movement[] Movement { get; set; }
        /// <summary>
        /// An array of buffs that are applied to only this character. Similar to the effects of auras but with no radius.
        /// </summary>
        public Buff[] Buffs { get; set; }
        /// <summary>
        /// An array of auras that have associated abilities and radius. Often apply buffs on nearby characters.
        /// </summary>
        public Aura[] Auras { get; set; }

        protected override void MidExport(BinaryWriter binaryWriter)
        {
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
            foreach (Resistance resistance in Resistances)
            {
                binaryWriter.Write((int)resistance.Element);
                binaryWriter.Write(resistance.Multiplier);
                binaryWriter.Write(resistance.Modifier);
                binaryWriter.Write(resistance.StatusImmunity);
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
        }

        protected override void MidImport(BinaryReader binaryReader)
        {
            binaryReader.BaseStream.Position += ReaderHeader.Length;

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
        }
    }
}