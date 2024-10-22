using MagickaForge.Utils;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Events
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "_EventType")]
    [JsonDerivedType(typeof(DamageEvent), typeDiscriminator: "Damage")]
    [JsonDerivedType(typeof(SplashEvent), typeDiscriminator: "Splash")]
    [JsonDerivedType(typeof(SoundEvent), typeDiscriminator: "Sound")]
    [JsonDerivedType(typeof(EffectEvent), typeDiscriminator: "Effect")]
    [JsonDerivedType(typeof(RemoveEvent), typeDiscriminator: "Remove")]
    [JsonDerivedType(typeof(CameraShakeEvent), typeDiscriminator: "CameraShake")]
    [JsonDerivedType(typeof(DecalEvent), typeDiscriminator: "Decal")]
    [JsonDerivedType(typeof(SpawnEvent), typeDiscriminator: "Spawn")]
    [JsonDerivedType(typeof(SpawnGibsEvent), typeDiscriminator: "SpawnGibs")]
    [JsonDerivedType(typeof(SpawnItemEvent), typeDiscriminator: "SpawnItem")]
    [JsonDerivedType(typeof(SpawnMagickEvent), typeDiscriminator: "SpawnMagick")]
    [JsonDerivedType(typeof(SpawnMissileEvent), typeDiscriminator: "SpawnMissile")]
    [JsonDerivedType(typeof(LightEvent), typeDiscriminator: "Light")]
    [JsonDerivedType(typeof(CastMagickEvent), typeDiscriminator: "CastMagick")]
    [JsonDerivedType(typeof(DamageOwnerEvent), typeDiscriminator: "DamageOwner")]
    [JsonDerivedType(typeof(OverkillEvent), typeDiscriminator: "Overkill")]
    public abstract class Event
    {
        protected EventType type;
        public virtual void Write(BinaryWriter bw) { bw.Write((byte)type); }
    }

    public class DamageEvent : Event
    {
        [JsonConverter(typeof(JsonStringEnumConverter<AttackProperties>))]
        public AttackProperties AttackProperty { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Elements>))]
        public Elements Element { get; set; }
        public float Amount { get; set; }
        public float Magnitude { get; set; }
        public bool VelocityBased { get; set; }
        public DamageEvent()
        {
            type = EventType.Damage;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write((int)AttackProperty);
            bw.Write((int)Element);
            bw.Write(Amount);
            bw.Write(Magnitude);
            bw.Write(VelocityBased);
        }
    }

    public class SplashEvent : Event
    {
        [JsonConverter(typeof(JsonStringEnumConverter<AttackProperties>))]
        public AttackProperties AttackProperty { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Elements>))]
        public Elements Element { get; set; }
        public int Amount { get; set; }
        public float Magnitude { get; set; }
        public float Radius { get; set; }
        public SplashEvent()
        {
            type = EventType.Splash;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write((int)AttackProperty);
            bw.Write((int)Element);
            bw.Write(Amount);
            bw.Write(Magnitude);
            bw.Write(Radius);
        }
    }

    public class SoundEvent : Event
    {
        [JsonConverter(typeof(JsonStringEnumConverter<Banks>))]
        public Banks Bank { get; set; }
        public required string Cue { get; set; }
        public float Magnitude { get; set; }
        public bool StopOnRemove { get; set; }
        public SoundEvent()
        {
            type = EventType.Sound;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write((int)Bank);
            bw.Write(Cue);
            bw.Write(Magnitude);
            bw.Write(StopOnRemove);
        }
    }

    public class EffectEvent : Event
    {
        public bool Follow { get; set; }
        public bool WorldAligned { get; set; }
        public required string Effect { get; set; }

        public EffectEvent()
        {
            type = EventType.Effect;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Follow);
            bw.Write(WorldAligned);
            bw.Write(Effect);
        }

    }
    public class RemoveEvent : Event
    {
        public int Bounces { get; set; }
        public RemoveEvent()
        { type = EventType.Remove; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Bounces);
        }
    }

    public class CameraShakeEvent : Event
    {
        public float Time { get; set; }
        public float Magnitude { get; set; }

        public bool AtPosition { get; set; }
        public CameraShakeEvent() { type = EventType.CameraShake; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Time);
            bw.Write(Magnitude);
            bw.Write(AtPosition);
        }
    }
    public class DecalEvent : Event
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Scale { get; set; }
        public DecalEvent() { type = EventType.Decal; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(X);
            bw.Write(Y);
            bw.Write(Scale);
        }
    }
    public class SpawnEvent : Event
    {
        public string? Type { get; set; }
        public string? IdleAnimation { get; set; }
        public string? SpawnAnimation { get; set; }
        public float Health { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Order>))]
        public Order Order { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<ReactTo>))]
        public ReactTo ReactTo { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Order>))]
        public Order Reaction { get; set; }
        public float Rotation { get; set; }
        public required float[] Offset { get; set; }
        public SpawnEvent() { type = EventType.Spawn; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Type);
            bw.Write(IdleAnimation);
            bw.Write(SpawnAnimation);
            bw.Write(Health);
            bw.Write((byte)Order);
            bw.Write((byte)ReactTo);
            bw.Write((byte)Reaction);
            bw.Write(Rotation);
            foreach (int i in Offset)
            {
                bw.Write(i);
            }
        }
    }

    public class SpawnGibsEvent : Event
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public SpawnGibsEvent() { type = EventType.SpawnGibs; }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(StartIndex);
            bw.Write(EndIndex);
        }
    }

    public class SpawnItemEvent : Event
    {
        public required string Item { get; set; }
        public SpawnItemEvent() { type = EventType.SpawnItem; }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Item);
        }
    }

    public class SpawnMagickEvent : Event
    {
        public string? MagickType { get; set; }
        public SpawnMagickEvent() { type = EventType.SpawnMagick; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MagickType);
        }
    }

    public class SpawnMissileEvent : Event
    {
        public required string Type { get; set; }
        public required float[] Velocity { get; set; }
        public bool Facing { get; set; }
        public SpawnMissileEvent() { type = EventType.SpawnMissile; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Type);
            foreach (float f in Velocity)
            {
                bw.Write(f);
            }
            bw.Write(Facing);
        }
    }
    public class LightEvent : Event
    {
        public float Radius { get; set; }
        public required float[] DiffuseColor { get; set; }
        public required float[] AmbientColor { get; set; }
        public float SpecularAmount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<LightVariationType>))]
        public LightVariationType LightVariationType { get; set; }
        public float VariationAmount { get; set; }
        public float VariationSpeed { get; set; }
        public LightEvent() { type = EventType.Light; }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write((float)Radius);
            foreach (float v in DiffuseColor)
            {
                bw.Write(v);
            }
            foreach (float v in AmbientColor)
            {
                bw.Write(v);
            }
            bw.Write(SpecularAmount);
            bw.Write((byte)LightVariationType);
            bw.Write(VariationAmount);
            bw.Write(VariationSpeed);
        }
    }
    public class CastMagickEvent : Event
    {
        public required string MagickType { get; set; }
        public int[] Elements { get; set; }
        public CastMagickEvent() { type = EventType.CastMagick; }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(MagickType);
            bw.Write(Elements.Length);
            foreach (int e in Elements)
            {
                bw.Write(e);
            }
        }
    }
    public class OverkillEvent : Event
    {
        public OverkillEvent() { type = EventType.Overkill; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
        }
    }
    public class DamageOwnerEvent : Event
    {
        [JsonConverter(typeof(JsonStringEnumConverter<AttackProperties>))]
        public AttackProperties AttackProperty { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Elements>))]
        public Elements Element { get; set; }
        public float Amount { get; set; }
        public float Magnitude { get; set; }
        public bool VelocityBased { get; set; }
        public DamageOwnerEvent() { type = EventType.DamageOwner; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write((int)AttackProperty);
            bw.Write((int)Element);
            bw.Write(Amount);
            bw.Write(Magnitude);
            bw.Write(VelocityBased);
        }
    }
}
