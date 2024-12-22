using MagickaForge.Utils.Data;
using MagickaForge.Utils.Data.Abilities;
using MagickaForge.Utils.Data.Animations;
using MagickaForge.Utils.Structures;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Animations
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "_ActionType")]
    [JsonDerivedType(typeof(Block), typeDiscriminator: "Block")]
    [JsonDerivedType(typeof(BreakFree), typeDiscriminator: "BreakFree")]
    [JsonDerivedType(typeof(CameraShake), typeDiscriminator: "CameraShake")]
    [JsonDerivedType(typeof(CastSpell), typeDiscriminator: "CastSpell")]
    [JsonDerivedType(typeof(Crouch), typeDiscriminator: "Crouch")]
    [JsonDerivedType(typeof(DamageGrip), typeDiscriminator: "DamageGrip")]
    [JsonDerivedType(typeof(DealDamage), typeDiscriminator: "DealDamage")]
    [JsonDerivedType(typeof(DetachItem), typeDiscriminator: "DetachItem")]
    [JsonDerivedType(typeof(Ethereal), typeDiscriminator: "Ethereal")]
    [JsonDerivedType(typeof(Footstep), typeDiscriminator: "Footstep")]
    [JsonDerivedType(typeof(Grip), typeDiscriminator: "Grip")]
    [JsonDerivedType(typeof(Gunfire), typeDiscriminator: "Gunfire")]
    [JsonDerivedType(typeof(Immortal), typeDiscriminator: "Immortal")]
    [JsonDerivedType(typeof(Invisible), typeDiscriminator: "Invisible")]
    [JsonDerivedType(typeof(Jump), typeDiscriminator: "Jump")]
    [JsonDerivedType(typeof(Move), typeDiscriminator: "Move")]
    [JsonDerivedType(typeof(OverkillGrip), typeDiscriminator: "OverkillGrip")]
    [JsonDerivedType(typeof(PlayEffect), typeDiscriminator: "PlayEffect")]
    [JsonDerivedType(typeof(PlaySound), typeDiscriminator: "PlaySound")]
    [JsonDerivedType(typeof(ReleaseGrip), typeDiscriminator: "ReleaseGrip")]
    [JsonDerivedType(typeof(RemoveStatus), typeDiscriminator: "RemoveStatus")]
    [JsonDerivedType(typeof(SetItemAttach), typeDiscriminator: "SetItemAttach")]
    [JsonDerivedType(typeof(SpawnMissile), typeDiscriminator: "SpawnMissile")]
    [JsonDerivedType(typeof(SpecialAbilityAction), typeDiscriminator: "SpecialAbility")]
    [JsonDerivedType(typeof(Suicide), typeDiscriminator: "Suicide")]
    [JsonDerivedType(typeof(ThrowGrip), typeDiscriminator: "ThrowGrip")]
    [JsonDerivedType(typeof(Tongue), typeDiscriminator: "Tongue")]
    [JsonDerivedType(typeof(WeaponVisibility), typeDiscriminator: "WeaponVisibility")]
    public class AnimationAction
    {
        protected ActionType _type;

        public float ActionStart { get; set; }
        public float ActionEnd { get; set; }

        public virtual void Write(BinaryWriter bw)
        {
            bw.Write(_type.ToString());
            bw.Write(ActionStart);
            bw.Write(ActionEnd);
        }
        public static AnimationAction GetAction(ActionType aType, BinaryReader br)
        {
            AnimationAction action;
            float start = br.ReadSingle();
            float end = br.ReadSingle();
            if (aType == ActionType.Block)
            {
                action = new Block() { WeaponSlot = br.ReadInt32() };
            }
            else if (aType == ActionType.BreakFree)
            {
                action = new BreakFree() { Magnitude = br.ReadSingle(), WeaponSlot = br.ReadInt32() };
            }
            else if (aType == ActionType.CameraShake)
            {
                br.ReadString();
                action = new CameraShake() { Duration = br.ReadSingle(), Magnitude = br.ReadSingle() };
            }
            else if (aType == ActionType.CastSpell)
            {
                action = new CastSpell() { FromStaff = br.ReadBoolean() };
                if (!(action as CastSpell)!.FromStaff)
                {
                    (action as CastSpell)!.Bone = br.ReadString();
                }
            }
            else if (aType == ActionType.Crouch)
            {
                action = new Crouch() { Radius = br.ReadSingle(), Length = br.ReadSingle() };
            }
            else if (aType == ActionType.DamageGrip)
            {
                bool dmgOwner = br.ReadBoolean();

                Damage[] damage = new Damage[br.ReadInt32()];
                for (int i = 0; i < damage.Length; i++)
                {
                    damage[i].AttackProperty = (AttackProperties)br.ReadInt32();
                    damage[i].Element = (Elements)br.ReadInt32();
                    damage[i].Amount = br.ReadSingle();
                    damage[i].Magnitude = br.ReadSingle();
                }
                action = new DamageGrip() { DamageOwner = dmgOwner, Damages = damage };
            }
            else if (aType == ActionType.DealDamage)
            {
                action = new DealDamage() { WeaponSlot = br.ReadInt32(), Target = (Targets)br.ReadByte() };
            }
            else if (aType == ActionType.DetachItem)
            {
                action = new DetachItem { WeaponSlot = br.ReadInt32(), Velocity = new Vector3(br) };
            }
            else if (aType == ActionType.Ethereal)
            {
                action = new Ethereal() { IsEthereal = br.ReadBoolean(), EtherealAlpha = br.ReadSingle(), EtherealSpeed = br.ReadSingle() };
            }
            else if (aType == ActionType.Footstep)
            {
                action = new Footstep();
            }
            else if (aType == ActionType.Grip)
            {
                action = new Grip() { GripType = (GripType)br.ReadByte(), Radius = br.ReadSingle(), BreakFreeTolerance = br.ReadSingle(), BoneA = br.ReadString(), BoneB = br.ReadString(), FinishOnGrip = br.ReadBoolean() };
            }
            else if (aType == ActionType.Gunfire)
            {
                action = new Gunfire() { WeaponSlot = br.ReadInt32(), Accuracy = br.ReadSingle() };
            }
            else if (aType == ActionType.Immortal)
            {
                action = new Immortal() { Collide = br.ReadBoolean() };
            }
            else if (aType == ActionType.Invisible)
            {
                action = new Invisible() { Shimmer = br.ReadBoolean() };
            }
            else if (aType == ActionType.Jump)
            {
                float elevation = br.ReadSingle();
                float minRange = 0;
                float maxRange = 0;

                if (br.ReadBoolean())
                {
                    minRange = br.ReadSingle();
                }
                if (br.ReadBoolean())
                {
                    maxRange = br.ReadSingle();
                }
                action = new Jump() { Elevation = elevation, MinRange = minRange, MaxRange = maxRange };
            }
            else if (aType == ActionType.Move)
            {
                action = new Move() { Velocity = [br.ReadSingle(), br.ReadSingle(), br.ReadSingle()] };
            }
            else if (aType == ActionType.OverkillGrip)
            {
                action = new OverkillGrip();
            }
            else if (aType == ActionType.PlayEffect)
            {
                action = new PlayEffect() { Bone = br.ReadString(), Attached = br.ReadBoolean(), Effect = br.ReadString(), Value = br.ReadSingle() };
            }
            else if (aType == ActionType.PlaySound)
            {
                action = new PlaySound() { Cue = br.ReadString(), Bank = (Banks)br.ReadInt32() };
            }
            else if (aType == ActionType.ReleaseGrip)
            {
                action = new ReleaseGrip();
            }
            else if (aType == ActionType.RemoveStatus)
            {
                action = new RemoveStatus() { Status = br.ReadString() };
            }
            else if (aType == ActionType.SetItemAttach)
            {
                action = new SetItemAttach() { ItemSlot = br.ReadInt32(), Bone = br.ReadString() };
            }
            else if (aType == ActionType.SpawnMissile)
            {
                action = new SpawnMissile() { WeaponSlot = br.ReadInt32(), Velocity = [br.ReadSingle(), br.ReadSingle(), br.ReadSingle()], Aligned = br.ReadBoolean() };
            }
            else if (aType == ActionType.SpecialAbility)
            {
                SpecialAbility spec = new();
                int weaponSlot = br.ReadInt32();
                if (weaponSlot < 0)
                {
                    spec.Type = br.ReadString();
                    spec.Animation = br.ReadString();
                    spec.Hash = br.ReadString();
                    spec.Elements = new Elements[br.ReadInt32()];
                    for (int i = 0; i < spec.Elements.Length; i++)
                    {
                        spec.Elements[i] = (Elements)br.ReadInt32();
                    }
                }
                action = new SpecialAbilityAction() { WeaponSlot = weaponSlot, SpecialAbilityInstance = spec };
            }
            else if (aType == ActionType.Suicide)
            {
                action = new Suicide() { Overkill = br.ReadBoolean() };
            }
            else if (aType == ActionType.ThrowGrip)
            {
                action = new ThrowGrip();
            }
            else if (aType == ActionType.Tongue)
            {
                action = new Tongue() { MaxLength = br.ReadSingle() };
            }
            else if (aType == ActionType.WeaponVisibility)
            {
                action = new WeaponVisibility() { WeaponSlot = br.ReadInt32(), Visible = br.ReadBoolean() };
            }
            else
            {
                throw new Exception(aType.ToString());
            }
            action.ActionStart = start;
            action.ActionEnd = end;
            return action;
        }
    }

    public class Block : AnimationAction
    {
        public int WeaponSlot { get; set; }

        public Block()
        {
            _type = ActionType.Block;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(WeaponSlot);
        }
    }
    public class BreakFree : AnimationAction
    {
        public float Magnitude { get; set; }
        public int WeaponSlot { get; set; }

        public BreakFree()
        {
            _type = ActionType.BreakFree;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Magnitude);
            bw.Write(WeaponSlot);
        }
    }
    public class CameraShake : AnimationAction
    {
        public float Duration { get; set; }
        public float Magnitude { get; set; }

        public CameraShake()
        {
            _type = ActionType.CameraShake;
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write("RyteWasHere");
            bw.Write(Duration);
            bw.Write(Magnitude);
        }
    }
    public class CastSpell : AnimationAction
    {
        public bool FromStaff { get; set; }
        public string? Bone { get; set; }

        public CastSpell()
        {
            _type = ActionType.CastSpell;
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(FromStaff);
            if (!FromStaff)
            {
                bw.Write(Bone!);
            }
        }
    }
    public class Crouch : AnimationAction
    {
        public float Radius { get; set; }
        public float Length { get; set; }

        public Crouch()
        {
            _type = ActionType.Crouch;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Radius);
            bw.Write(Length);
        }
    }
    public class DamageGrip : AnimationAction
    {
        public bool DamageOwner { get; set; }
        public Damage[]? Damages { get; set; }
        public DamageGrip()
        {
            _type = ActionType.DamageGrip;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(DamageOwner);
            bw.Write(Damages!.Length);
            foreach (Damage damage in Damages)
            {
                bw.Write((int)damage.AttackProperty);
                bw.Write((int)damage.Element);
                bw.Write(damage.Amount);
                bw.Write(damage.Magnitude);
            }
        }
    }
    public class DealDamage : AnimationAction
    {
        public int WeaponSlot { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Targets>))]
        public Targets Target { get; set; }
        public DealDamage()
        {
            _type = ActionType.DealDamage;
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(WeaponSlot);
            bw.Write((byte)Target);
        }
    }
    public class DetachItem : AnimationAction
    {
        public int WeaponSlot { get; set; }
        public Vector3 Velocity { get; set; }
        public DetachItem()
        {
            _type = ActionType.DetachItem;
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(WeaponSlot);
            Velocity.Write(bw);
        }
    }
    public class Ethereal : AnimationAction
    {
        public bool IsEthereal { get; set; }
        public float EtherealAlpha { get; set; }
        public float EtherealSpeed { get; set; }

        public Ethereal()
        {
            _type = ActionType.Ethereal;
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(IsEthereal);
            bw.Write(EtherealAlpha);
            bw.Write(EtherealSpeed);
        }
    }
    public class Footstep : AnimationAction
    {
        public Footstep()
        {
            _type = ActionType.Footstep;
        }
    }
    public class Grip : AnimationAction
    {
        [JsonConverter(typeof(JsonStringEnumConverter<GripType>))]
        public GripType GripType { get; set; }
        public float Radius { get; set; }
        public float BreakFreeTolerance { get; set; }
        public string? BoneA { get; set; }
        public string? BoneB { get; set; }
        public bool FinishOnGrip { get; set; }
        public Grip()
        {
            _type = ActionType.Grip;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write((byte)GripType);
            bw.Write(Radius);
            bw.Write(BreakFreeTolerance);
            bw.Write(BoneA!);
            bw.Write(BoneB!);
            bw.Write(FinishOnGrip);
        }
    }
    public class Gunfire : AnimationAction
    {
        public int WeaponSlot { get; set; }
        public float Accuracy { get; set; }
        public Gunfire()
        {
            _type = ActionType.Gunfire;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(WeaponSlot);
            bw.Write(Accuracy);
        }
    }
    public class Immortal : AnimationAction
    {
        public bool Collide { get; set; }
        public Immortal()
        {
            _type = ActionType.Immortal;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Collide);
        }
    }
    public class Invisible : AnimationAction
    {
        public bool Shimmer { get; set; }
        public Invisible()
        {
            _type = ActionType.Invisible;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Shimmer);
        }
    }
    public class Jump : AnimationAction
    {
        public float Elevation { get; set; }
        public float MinRange { get; set; }
        public float MaxRange { get; set; }
        public Jump()
        {
            _type = ActionType.Jump;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Elevation);
            bool hasMin = MinRange != 0;
            bool hasMax = MaxRange != 0;
            bw.Write(hasMin);
            if (hasMin)
            {
                bw.Write(MinRange);
            }
            bw.Write(hasMax);
            if (hasMax)
            {
                bw.Write(MaxRange);
            }
        }
    }
    public class Move : AnimationAction
    {
        public float[]? Velocity { get; set; }
        public Move()
        {
            _type = ActionType.Move;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            for (int i = 0; i < 3; i++)
            {
                bw.Write(Velocity![i]);
            }
        }
    }
    public class OverkillGrip : AnimationAction
    {
        public OverkillGrip()
        {
            _type = ActionType.OverkillGrip;
        }

    }
    public class PlayEffect : AnimationAction
    {
        public string? Bone { get; set; }
        public bool Attached { get; set; }
        public string? Effect { get; set; }
        public float Value { get; set; }
        public PlayEffect()
        {
            _type = ActionType.PlayEffect;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Bone!);
            bw.Write(Attached);
            bw.Write(Effect!);
            bw.Write(Value);
        }
    }

    public class PlaySound : AnimationAction
    {
        public string? Cue { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Banks>))]
        public Banks Bank { get; set; }
        public PlaySound()
        {
            _type = ActionType.PlaySound;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Cue!);
            bw.Write((int)Bank);
        }
    }
    public class ReleaseGrip : AnimationAction
    {
        public ReleaseGrip()
        {
            _type = ActionType.ReleaseGrip;
        }
    }

    public class RemoveStatus : AnimationAction
    {
        public string? Status { get; set; }
        public RemoveStatus()
        {
            _type = ActionType.RemoveStatus;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Status!);
        }
    }
    public class SetItemAttach : AnimationAction
    {
        public int ItemSlot { get; set; }
        public string Bone { get; set; }
        public SetItemAttach()
        {
            _type = ActionType.SetItemAttach;
            throw new Exception("TODO");
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(ItemSlot);
            bw.Write(Bone);
        }
    }
    public class SpawnMissile : AnimationAction
    {
        public int WeaponSlot { get; set; }
        public float[]? Velocity { get; set; }
        public bool Aligned { get; set; }
        public SpawnMissile()
        {
            _type = ActionType.SpawnMissile;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(WeaponSlot);
            for (int i = 0; i < 3; i++)
            {
                bw.Write(Velocity![i]);
            }
            bw.Write(Aligned);
        }
    }
    public class SpecialAbilityAction : AnimationAction
    {
        public int WeaponSlot { get; set; }
        public SpecialAbility? SpecialAbilityInstance { get; set; }

        public SpecialAbilityAction()
        {
            _type = ActionType.SpecialAbility;
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(WeaponSlot);
            if (WeaponSlot < 0)
            {
                bw.Write(SpecialAbilityInstance!.Type!);
                bw.Write(SpecialAbilityInstance.Animation!);
                bw.Write(SpecialAbilityInstance.Hash!);
                bw.Write(SpecialAbilityInstance.Elements!.Length);
                for (int i = 0; i < SpecialAbilityInstance.Elements.Length; i++)
                {
                    bw.Write((int)SpecialAbilityInstance.Elements[i]);
                }
            }
        }
    }
    public class Suicide : AnimationAction
    {
        public bool Overkill { get; set; }
        public Suicide()
        {
            _type = ActionType.Suicide;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Overkill);
        }
    }
    public class ThrowGrip : AnimationAction
    {
        public ThrowGrip()
        {
            _type = ActionType.ThrowGrip;
        }
    }

    public class Tongue : AnimationAction
    {
        public float MaxLength { get; set; }
        public Tongue()
        {
            _type = ActionType.Tongue;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MaxLength);
        }
    }

    public class WeaponVisibility : AnimationAction
    {
        public int WeaponSlot { get; set; }
        public bool Visible { get; set; }
        public WeaponVisibility()
        {
            _type = ActionType.WeaponVisibility;
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(WeaponSlot);
            bw.Write(Visible);
        }
    }

}