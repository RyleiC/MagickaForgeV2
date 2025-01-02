using MagickaForge.Components.Common;
using MagickaForge.Utils.Data;
using MagickaForge.Utils.Data.Auras;
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
        protected AuraType _type;

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
            bw.Write((byte)_type);
            bw.Write((byte)VisualCategory);
            Color.Write(bw);
            bw.Write(Effect);
            bw.Write(Duration);
            bw.Write(Radius);
            bw.Write(Types);
            bw.Write((int)Faction);
        }

        public static Aura GetAura(BinaryReader binaryReader)
        {
            var aura = new Aura();
            AuraTarget target = (AuraTarget)binaryReader.ReadByte();
            AuraType auraType = (AuraType)binaryReader.ReadByte();
            VisualCategory visualCategory = (VisualCategory)binaryReader.ReadByte();
            var color = new Color(binaryReader);
            var effect = binaryReader.ReadString();
            var duration = binaryReader.ReadSingle();
            var Radius = binaryReader.ReadSingle();
            var Types = binaryReader.ReadString();
            Factions Faction = (Factions)binaryReader.ReadInt32();

            switch (auraType)
            {
                case AuraType.Buff:
                    {
                        aura = new BuffAura()
                        {
                            Buff = Buff.GetBuff(binaryReader)
                        };
                    }
                    break;
                case AuraType.Deflect:
                    {
                        aura = new DeflectAura()
                        {
                            DeflectStrength = binaryReader.ReadSingle(),
                        };
                    }
                    break;
                case AuraType.Boost:
                    {
                        aura = new BoostAura()
                        {
                            BoostStrength = binaryReader.ReadSingle(),
                        };
                    }
                    break;
                case AuraType.LifeSteal:
                    {
                        aura = new LifeStealAura()
                        {
                            LifeStealAmount = binaryReader.ReadSingle(),
                        };
                    }
                    break;
                case AuraType.Love:
                    {
                        aura = new LoveAura()
                        {
                            CharmRadius = binaryReader.ReadSingle(),
                            CharmDuration = binaryReader.ReadSingle(),
                        };
                    }
                    break;
            };

            aura.AuraTarget = target;
            aura.VisualCategory = visualCategory;
            aura.Color = color;
            aura.Effect = effect;
            aura.Duration = duration;
            aura.Radius = Radius;
            aura.Types = Types;
            aura.Faction = Faction;
            return aura;
        }
    }

    public class BuffAura : Aura
    {
        public Buff Buff { get; set; }
        public BuffAura() { _type = AuraType.Buff; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            Buff.Write(bw);
        }
    }
    public class DeflectAura : Aura
    {
        public float DeflectStrength { get; set; }
        public DeflectAura() { _type = AuraType.Deflect; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(DeflectStrength);
        }
    }
    public class BoostAura : Aura
    {
        public float BoostStrength { get; set; }
        public BoostAura() { _type = AuraType.Boost; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(BoostStrength);
        }
    }
    public class LifeStealAura : Aura
    {
        public float LifeStealAmount { get; set; }
        public LifeStealAura() { _type = AuraType.LifeSteal; }
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
        public LoveAura() { _type = AuraType.Love; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(CharmRadius);
            bw.Write(CharmDuration);
        }
    }
}
