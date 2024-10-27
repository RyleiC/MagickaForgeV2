using MagickaForge.Utils.Definitions;
using MagickaForge.Utils.Definitions.Auras;
using MagickaForge.Utils.Structures;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Auras
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "_AuraType")]
    [JsonDerivedType(typeof(BuffAura), typeDiscriminator: "Buff")]
    [JsonDerivedType(typeof(DeflectAura), typeDiscriminator: "Deflect")]
    [JsonDerivedType(typeof(BoostAura), typeDiscriminator: "Boost")]
    [JsonDerivedType(typeof(LifeStealAura), typeDiscriminator: "LifeSteal")]
    [JsonDerivedType(typeof(LoveAura), typeDiscriminator: "Love")]
    public class Aura
    {
        protected AuraType type;

        [JsonConverter(typeof(JsonStringEnumConverter<AuraTarget>))]
        public AuraTarget AuraTarget { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<VisualCategory>))]
        public VisualCategory VisualCategory { get; set; }
        public Color Color { get; set; }
        public string? Effect { get; set; }
        public float Duration { get; set; }
        public float Radius { get; set; }
        public string? Types { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Factions>))]
        public Factions Faction { get; set; }

        public virtual void Write(BinaryWriter bw)
        {
            bw.Write((byte)AuraTarget);
            bw.Write((byte)type);
            bw.Write((byte)VisualCategory);
            Color.Write(bw);
            bw.Write(Effect);
            bw.Write(Duration);
            bw.Write(Radius);
            bw.Write(Types);
            bw.Write((int)Faction);
        }
    }

    public class BuffAura : Aura
    {
        public Buff Buff { get; set; }
        public BuffAura() { type = AuraType.Buff; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            Buff.Write(bw);
        }
    }
    public class DeflectAura : Aura
    {
        public float DeflectStrength { get; set; }
        public DeflectAura() { type = AuraType.Deflect; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(DeflectStrength);
        }
    }
    public class BoostAura : Aura
    {
        public float BoostStrength { get; set; }
        public BoostAura() { type = AuraType.Boost; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(BoostStrength);
        }
    }
    public class LifeStealAura : Aura
    {
        public float LifeStealAmount { get; set; }
        public LifeStealAura() { type = AuraType.LifeSteal; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(LifeStealAmount);
        }
    }
    public class LoveAura : Aura
    {
        public float CharmRadius { get; set; }
        public float CharmDuration { get; set; }
        public LoveAura() { type = AuraType.Love; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(CharmRadius);
            bw.Write(CharmDuration);
        }
    }
}
