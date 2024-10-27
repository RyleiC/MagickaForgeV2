using MagickaForge.Utils.Definitions;
using MagickaForge.Utils.Definitions.Auras;
using MagickaForge.Utils.Structures;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Auras
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "_Type")]
    [JsonDerivedType(typeof(BoostDamageBuff), typeDiscriminator: "BoostDamage")]
    [JsonDerivedType(typeof(DealDamageBuff), typeDiscriminator: "DealDamage")]
    [JsonDerivedType(typeof(ResistanceBuff), typeDiscriminator: "Resistance")]
    [JsonDerivedType(typeof(BoostBuff), typeDiscriminator: "Boost")]
    [JsonDerivedType(typeof(ReduceAggroBuff), typeDiscriminator: "ReduceAgro")]
    [JsonDerivedType(typeof(UndyingBuff), typeDiscriminator: "Undying")]
    [JsonDerivedType(typeof(ModifyHitpointsBuff), typeDiscriminator: "ModifyHitPoints")]
    [JsonDerivedType(typeof(ModifySpellTTLBuff), typeDiscriminator: "ModifySpellTTL")]
    [JsonDerivedType(typeof(ModifySpellRangeBuff), typeDiscriminator: "ModifySpellRange")]
    public class Buff
    {
        protected BuffType Type;
        [JsonConverter(typeof(JsonStringEnumConverter<VisualCategory>))]
        public VisualCategory VisualCategory { get; set; }
        public Color Color { get; set; }
        public float Time { get; set; }
        public string? Effect { get; set; }

        public static Buff GetBuff(BinaryReader br)
        {
            var buff = new Buff();
            var Type = (BuffType)br.ReadByte();
            var visualCat = (VisualCategory)br.ReadByte();
            var Color = new Color(br);
            var Time = br.ReadSingle();
            var Effect = br.ReadString();

            switch (Type)
            {
                case BuffType.BoostDamage:
                    {
                        buff = new BoostDamageBuff() { AttackProperty = (AttackProperties)br.ReadInt32(), Element = (Elements)br.ReadInt32(), Amount = br.ReadSingle(), Magnitude = br.ReadSingle() };
                    }
                    break;
                case BuffType.DealDamage:
                    {
                        buff = new DealDamageBuff() { AttackProperty = (AttackProperties)br.ReadInt32(), Element = (Elements)br.ReadInt32(), Amount = br.ReadSingle(), Magnitude = br.ReadSingle() };
                    }
                    break;
                case BuffType.Resistance:
                    {
                        buff = new ResistanceBuff() { Element = (Elements)br.ReadInt32(), Multiplier = br.ReadSingle(), Modifier = br.ReadSingle(), StatusImmunity = br.ReadBoolean() };
                    }
                    break;
                case BuffType.Undying:
                    {
                        buff = new UndyingBuff();
                    }
                    break;
                case BuffType.Boost:
                    {
                        buff = new BoostBuff() { BoostAmount = br.ReadSingle() };
                    }
                    break;
                case BuffType.ReduceAgro:
                    {
                        buff = new ReduceAggroBuff() { AggroReduceAmount = br.ReadSingle() };
                    }
                    break;
                case BuffType.ModifyHitPoints:
                    {
                        buff = new ModifyHitpointsBuff() { HealthMultiplier = br.ReadSingle(), HealthModifier = br.ReadSingle() };
                    }
                    break;
                case BuffType.ModifySpellTTL:
                    {
                        buff = new ModifySpellTTLBuff() { SpellTimeMultiplier = br.ReadSingle(), SpellTimeModifier = br.ReadSingle() };
                    }
                    break;
                case BuffType.ModifySpellRange:
                    {
                        buff = new ModifySpellRangeBuff() { SpellRangeMultiplier = br.ReadSingle(), SpellRangeModifier = br.ReadSingle() };
                    }
                    break;
            }

            buff.VisualCategory = visualCat;
            buff.Color = Color;
            buff.Time = Time;
            buff.Effect = Effect;
            return buff;
        }
        public virtual void Write(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write((byte)VisualCategory);
            Color.Write(bw);
            bw.Write(Time);
            bw.Write(Effect);
        }
    }
    public class BoostDamageBuff : Buff
    {
        [JsonConverter(typeof(JsonStringEnumConverter<AttackProperties>))]
        public AttackProperties AttackProperty { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Elements>))]
        public Elements Element { get; set; }
        public float Amount { get; set; }
        public float Magnitude { get; set; }

        public BoostDamageBuff()
        {
            Type = BuffType.BoostDamage;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write((int)AttackProperty);
            bw.Write((int)Element);
            bw.Write(Amount);
            bw.Write(Magnitude);
        }
    }
    public class DealDamageBuff : Buff
    {
        [JsonConverter(typeof(JsonStringEnumConverter<AttackProperties>))]
        public AttackProperties AttackProperty { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Elements>))]
        public Elements Element { get; set; }
        public float Amount { get; set; }
        public float Magnitude { get; set; }
        public DealDamageBuff() : base()
        {
            Type = BuffType.DealDamage;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write((int)AttackProperty);
            bw.Write((int)Element);
            bw.Write(Amount);
            bw.Write(Magnitude);
        }
    }
    public class ResistanceBuff : Buff
    {
        [JsonConverter(typeof(JsonStringEnumConverter<Elements>))]
        public Elements Element { get; set; }
        public float Multiplier { get; set; }
        public float Modifier { get; set; }
        public bool StatusImmunity { get; set; }
        public ResistanceBuff()
        {
            Type = BuffType.Resistance;
        }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write((int)Element);
            bw.Write(Multiplier);
            bw.Write(Modifier);
            bw.Write(StatusImmunity);
        }
    }
    public class BoostBuff : Buff
    {
        public float BoostAmount { get; set; }
        public BoostBuff() { Type = BuffType.Boost; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(BoostAmount);
        }
    }
    public class ReduceAggroBuff : Buff
    {
        public float AggroReduceAmount { get; set; }
        public ReduceAggroBuff() { Type = BuffType.ReduceAgro; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(AggroReduceAmount);
        }
    }
    public class ModifyHitpointsBuff : Buff
    {
        public float HealthMultiplier { get; set; }
        public float HealthModifier { get; set; }
        public ModifyHitpointsBuff() { Type = BuffType.ModifyHitPoints; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(HealthMultiplier);
            bw.Write(HealthModifier);
        }
    }
    public class ModifySpellTTLBuff : Buff
    {
        public float SpellTimeMultiplier { get; set; }
        public float SpellTimeModifier { get; set; }
        public ModifySpellTTLBuff() { Type = BuffType.ModifySpellTTL; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(SpellTimeMultiplier);
            bw.Write(SpellTimeModifier);
        }
    }
    public class UndyingBuff : Buff
    {
        public UndyingBuff() { Type = BuffType.Undying; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
        }
    }
    public class ModifySpellRangeBuff : Buff
    {
        public float SpellRangeMultiplier { get; set; }
        public float SpellRangeModifier { get; set; }
        public ModifySpellRangeBuff() { Type = BuffType.ModifySpellRange; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(SpellRangeMultiplier);
            bw.Write(SpellRangeModifier);
        }
    }
}
