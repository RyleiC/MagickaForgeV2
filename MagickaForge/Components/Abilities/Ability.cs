using MagickaForge.Utils.Definitions;
using MagickaForge.Utils.Definitions.Abilities;
using MagickaForge.Utils.Definitions.Spellcasting;
using MagickaForge.Utils.Structures;
using System.Text.Json.Serialization;
namespace MagickaForge.Components.Abilities
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "_AbilityType")]
    [JsonDerivedType(typeof(Block), typeDiscriminator: "Block")]
    [JsonDerivedType(typeof(CastSpell), typeDiscriminator: "CastSpell")]
    [JsonDerivedType(typeof(ConfuseGrip), typeDiscriminator: "ConfuseGrip")]
    [JsonDerivedType(typeof(DamageGrip), typeDiscriminator: "DamageGrip")]
    [JsonDerivedType(typeof(Dash), typeDiscriminator: "Dash")]
    [JsonDerivedType(typeof(ElementSteal), typeDiscriminator: "ElementSteal")]
    [JsonDerivedType(typeof(GripCharacterFromBehind), typeDiscriminator: "GripCharacterFromBehind")]
    [JsonDerivedType(typeof(Jump), typeDiscriminator: "Jump")]
    [JsonDerivedType(typeof(Melee), typeDiscriminator: "Melee")]
    [JsonDerivedType(typeof(PickUpCharacter), typeDiscriminator: "PickUpCharacter")]
    [JsonDerivedType(typeof(Ranged), typeDiscriminator: "Ranged")]
    [JsonDerivedType(typeof(RemoveStatus), typeDiscriminator: "RemoveStatus")]
    [JsonDerivedType(typeof(SpecialAbilityAbility), typeDiscriminator: "SpecialAbilityAbility")]
    [JsonDerivedType(typeof(ThrowGrip), typeDiscriminator: "ThrowGrip")]
    [JsonDerivedType(typeof(ZombieGrip), typeDiscriminator: "ZombieGrip")]
    public class Ability
    {
        protected AbilityType _type;
        public float Cooldown { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<AbilityTarget>))]
        public AbilityTarget AbilityTarget { get; set; }
        public string? FuzzyExpression { get; set; }
        public string[]? Animations { get; set; }

        public virtual void Write(BinaryWriter bw)
        {
            bw.Write(_type.ToString());
            bw.Write(Cooldown);
            bw.Write((byte)AbilityTarget);
            bool hasFuzzy = FuzzyExpression!.Length > 0;
            bw.Write(hasFuzzy);
            if (hasFuzzy)
            {
                bw.Write(FuzzyExpression);
            }
            bw.Write(Animations!.Length);
            foreach (string animation in Animations)
            {
                bw.Write(animation);
            }
        }
        public static Ability GetAbility(BinaryReader br, AbilityType type)
        {
            Ability ability = new Ability();
            float cooldown = br.ReadSingle();
            AbilityTarget abilityTarget = (AbilityTarget)br.ReadByte();
            string fuzzyExpression = string.Empty;
            if (br.ReadBoolean())
            {
                fuzzyExpression = br.ReadString();
            }
            string[] animations = new string[br.ReadInt32()];
            for (int i = 0; i < animations.Length; i++)
            {
                animations[i] = br.ReadString();
            }

            if (type == AbilityType.Block)
            {
                ability = new Block() { Arc = br.ReadSingle(), Shield = br.ReadInt32() };
            }
            else if (type == AbilityType.CastSpell)
            {
                ability = new CastSpell() { MinimumRange = br.ReadSingle(), MaximumRange = br.ReadSingle(), Angle = br.ReadSingle(), ChantTime = br.ReadSingle(), Power = br.ReadSingle(), CastType = (CastType)br.ReadInt32() };
                CastSpell? castSpell = ability as CastSpell;
                castSpell!.Elements = new Elements[br.ReadInt32()];
                for (int i = 0; i < castSpell.Elements.Length; i++)
                {
                    castSpell.Elements[i] = (Elements)br.ReadInt32();
                }
            }
            else if (type == AbilityType.ConfuseGrip)
            {
                ability = new ConfuseGrip();
            }
            else if (type == AbilityType.DamageGrip)
            {
                ability = new DamageGrip();
            }
            else if (type == AbilityType.Dash)
            {
                ability = new Dash() { MinimumRange = br.ReadSingle(), MaximumRange = br.ReadSingle(), Arc = br.ReadSingle(), Velocity = new Vector3(br) };
            }
            else if (type == AbilityType.ElementSteal)
            {
                ability = new ElementSteal() { Range = br.ReadSingle(), Angle = br.ReadSingle() };
            }
            else if (type == AbilityType.GripCharacterFromBehind)
            {
                ability = new GripCharacterFromBehind() { MaximumRange = br.ReadSingle(), MinimumRange = br.ReadSingle(), Angle = br.ReadSingle(), MaxWeight = br.ReadSingle() };
            }
            else if (type == AbilityType.Jump)
            {
                ability = new Jump() { MaximumRange = br.ReadSingle(), MinimumRange = br.ReadSingle(), Angle = br.ReadSingle(), Elevation = br.ReadSingle() };
            }
            else if (type == AbilityType.Melee)
            {
                ability = new Melee() { MinimumRange = br.ReadSingle(), MaximumRange = br.ReadSingle(), ArcAngle = br.ReadSingle() };
                Melee melee = (ability as Melee)!;
                melee.WeaponSlots = new int[br.ReadInt32()];
                for (int i = 0; i < melee.WeaponSlots.Length; i++)
                {
                    melee.WeaponSlots[i] = br.ReadInt32();
                }
                melee.Rotate = br.ReadBoolean();
            }
            else if (type == AbilityType.PickUpCharacter)
            {
                ability = new PickUpCharacter() { MaximumRange = br.ReadSingle(), MinimumRange = br.ReadSingle(), Angle = br.ReadSingle(), MaxWeight = br.ReadSingle(), DropAnimation = br.ReadString() };
            }
            else if (type == AbilityType.Ranged)
            {
                ability = new Ranged() { MinimumRange = br.ReadSingle(), MaximumRange = br.ReadSingle(), Elevation = br.ReadSingle(), Arc = br.ReadSingle(), Accuracy = br.ReadSingle() };
                Ranged ranged = (ability as Ranged)!;
                ranged.WeaponSlots = new int[br.ReadInt32()];
                for (int i = 0; i < ranged.WeaponSlots.Length; i++)
                {
                    ranged.WeaponSlots[i] = br.ReadInt32();
                }
            }
            else if (type == AbilityType.RemoveStatus)
            {
                ability = new RemoveStatus();
            }
            else if (type == AbilityType.SpecialAbilityAbility)
            {
                ability = new SpecialAbilityAbility() { MaximumRange = br.ReadSingle(), MinimumRange = br.ReadSingle(), Angle = br.ReadSingle(), WeaponSlot = br.ReadInt32() };
            }
            else if (type == AbilityType.ThrowGrip)
            {
                ability = new ThrowGrip() { MaximumRange = br.ReadSingle(), MinimumRange = br.ReadSingle(), Elevation = br.ReadSingle() };
                ThrowGrip throwGrip = (ability as ThrowGrip)!;
                throwGrip.Damages = new Damage[br.ReadInt32()];
                for (int i = 0; i < throwGrip.Damages.Length; i++)
                {
                    throwGrip.Damages[i].AttackProperty = (AttackProperties)br.ReadInt32();
                    throwGrip.Damages[i].Element = (Elements)br.ReadInt32();
                    throwGrip.Damages[i].Amount = br.ReadInt32();
                    throwGrip.Damages[i].Magnitude = br.ReadSingle();
                }
            }
            else if (type == AbilityType.ZombieGrip)
            {
                ability = new ZombieGrip() { MaximumRange = br.ReadSingle(), MinimumRange = br.ReadSingle(), Angle = br.ReadSingle(), MaxWeight = br.ReadSingle(), DropAnimation = br.ReadString() };
            }
            ability.Cooldown = cooldown;
            ability.AbilityTarget = abilityTarget;
            ability.FuzzyExpression = fuzzyExpression;
            ability.Animations = animations;
            return ability;
        }
    }
    public class Block : Ability
    {
        public float Arc { get; set; }
        public int Shield { get; set; }
        public Block()
        {
            _type = AbilityType.Block;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Arc);
            bw.Write(Shield);
        }
    }
    public class CastSpell : Ability
    {
        public float MinimumRange { get; set; }
        public float MaximumRange { get; set; }
        public float Angle { get; set; }
        public float ChantTime { get; set; }
        public float Power { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<CastType>))]
        public CastType CastType { get; set; }
        public Elements[]? Elements { get; set; }

        public CastSpell()
        {
            _type = AbilityType.CastSpell;
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MinimumRange);
            bw.Write(MaximumRange);
            bw.Write(Angle);
            bw.Write(ChantTime);
            bw.Write(Power);
            bw.Write((int)CastType);
            bw.Write(Elements!.Length);
            foreach (int element in Elements)
            {
                bw.Write(element);
            }
        }
    }
    public class ConfuseGrip : Ability
    {
        public ConfuseGrip()
        {
            _type = AbilityType.ConfuseGrip;
        }
    }

    public class DamageGrip : Ability
    {
        public DamageGrip()
        {
            _type = AbilityType.DamageGrip;
        }
    }
    public class Dash : Ability
    {
        public float MinimumRange { get; set; }
        public float MaximumRange { get; set; }
        public float Arc { get; set; }
        public Vector3 Velocity { get; set; }

        public Dash()
        {
            _type = AbilityType.Dash;
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MinimumRange);
            bw.Write(MaximumRange);
            bw.Write(Arc);
            Velocity.Write(bw);
        }
    }
    public class ElementSteal : Ability
    {
        public float Range { get; set; }
        public float Angle { get; set; }

        public ElementSteal()
        {
            _type = AbilityType.ElementSteal;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Range);
            bw.Write(Angle);
        }
    }
    public class GripCharacterFromBehind : Ability
    {
        public float MaximumRange { get; set; }
        public float MinimumRange { get; set; }
        public float Angle { get; set; }
        public float MaxWeight { get; set; }

        public GripCharacterFromBehind()
        {
            _type = AbilityType.GripCharacterFromBehind;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MaximumRange);
            bw.Write(MinimumRange);
            bw.Write(Angle);
            bw.Write(MaxWeight);
        }
    }
    public class Jump : Ability
    {
        public float MaximumRange { get; set; }
        public float MinimumRange { get; set; }
        public float Angle { get; set; }
        public float Elevation { get; set; }

        public Jump()
        {
            _type = AbilityType.Jump;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MaximumRange);
            bw.Write(MinimumRange);
            bw.Write(Angle);
            bw.Write(Elevation);
        }
    }
    public class Melee : Ability
    {
        public float MinimumRange { get; set; }
        public float MaximumRange { get; set; }
        public float ArcAngle { get; set; }
        public int[]? WeaponSlots { get; set; }
        public bool Rotate { get; set; }

        public Melee()
        {
            _type = AbilityType.Melee;
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MinimumRange);
            bw.Write(MaximumRange);
            bw.Write(ArcAngle);
            bw.Write(WeaponSlots!.Length);
            foreach (int i in WeaponSlots)
            {
                bw.Write(i);
            }
            bw.Write(Rotate);
        }
    }
    public class PickUpCharacter : Ability
    {
        public float MaximumRange { get; set; }
        public float MinimumRange { get; set; }
        public float Angle { get; set; }
        public float MaxWeight { get; set; }
        public string? DropAnimation { get; set; }
        public PickUpCharacter()
        {
            _type = AbilityType.PickUpCharacter;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MaximumRange);
            bw.Write(MinimumRange);
            bw.Write(Angle);
            bw.Write(MaxWeight);
            bw.Write(DropAnimation!);
        }
    }

    public class Ranged : Ability
    {
        public float MinimumRange { get; set; }
        public float MaximumRange { get; set; }
        public float Elevation { get; set; }
        public float Arc { get; set; }
        public float Accuracy { get; set; }
        public int[] WeaponSlots { get; set; }

        public Ranged()
        {
            _type = AbilityType.Ranged;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MinimumRange);
            bw.Write(MaximumRange);
            bw.Write(Elevation);
            bw.Write(Arc);
            bw.Write(Accuracy);
            bw.Write(WeaponSlots.Length);
            foreach (int i in WeaponSlots)
            {
                bw.Write(i);
            }
        }
    }
    public class RemoveStatus : Ability
    {
        public RemoveStatus()
        {
            _type = AbilityType.RemoveStatus;
        }
    }
    public class SpecialAbilityAbility : Ability
    {
        public float MaximumRange { get; set; }
        public float MinimumRange { get; set; }
        public float Angle { get; set; }
        public int WeaponSlot { get; set; }

        public SpecialAbilityAbility()
        {
            _type = AbilityType.SpecialAbilityAbility;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MaximumRange);
            bw.Write(MinimumRange);
            bw.Write(Angle);
            bw.Write(WeaponSlot);
        }
    }
    public class ThrowGrip : Ability
    {
        public float MaximumRange { get; set; }
        public float MinimumRange { get; set; }
        public float Elevation { get; set; }
        public Damage[] Damages { get; set; }
        public ThrowGrip()
        {
            _type = AbilityType.ThrowGrip;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MaximumRange);
            bw.Write(MinimumRange);
            bw.Write(Elevation);
            bw.Write(Damages.Length);
            foreach (Damage damage in Damages)
            {
                bw.Write((int)damage.AttackProperty);
                bw.Write((int)damage.Element);
                bw.Write(damage.Amount);
                bw.Write(damage.Magnitude);
            }
        }

    }
    public class ZombieGrip : Ability
    {
        public float MaximumRange { get; set; }
        public float MinimumRange { get; set; }
        public float Angle { get; set; }
        public float MaxWeight { get; set; }
        public string DropAnimation { get; set; }
        public ZombieGrip()
        {
            _type = AbilityType.ZombieGrip;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MaximumRange);
            bw.Write(MinimumRange);
            bw.Write(Angle);
            bw.Write(MaxWeight);
            bw.Write(DropAnimation);
        }
    }
}