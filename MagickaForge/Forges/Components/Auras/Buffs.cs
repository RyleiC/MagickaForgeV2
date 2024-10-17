using MagickaForge.Utils;
using System.Text.Json.Serialization;

namespace MagickaForge.Forges.Components.Auras
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "_BuffType")]
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
        protected BuffType buffType;
        [JsonConverter(typeof(JsonStringEnumConverter<VisualCategory>))]
        public VisualCategory BuffVisualCategory { get; set; }
        public float[]? BuffColor { get; set; }
        public float BuffTime { get; set; }
        public string? BuffEffect { get; set; }

        public static Buff GetBuff(BinaryReader br)
        {
            Buff buff = new Buff();
            BuffType buffType = (BuffType)br.ReadByte();
            VisualCategory visualCat = (VisualCategory)br.ReadByte();
            float[] color = { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
            float buffTime = br.ReadSingle();
            string buffEffect = br.ReadString();

            switch (buffType)
            {
                case (BuffType.BoostDamage):
                    {
                        buff = new BoostDamageBuff() { AttackProperty = (AttackProperties)br.ReadInt32(), Element = (Elements)br.ReadInt32(), Amount = br.ReadSingle(), Magnitude = br.ReadSingle() };
                    }
                    break;
                case (BuffType.DealDamage):
                    {
                        buff = new DealDamageBuff() { AttackProperty = (AttackProperties)br.ReadInt32(), Element = (Elements)br.ReadInt32(), Amount = br.ReadSingle(), Magnitude = br.ReadSingle() };
                    }
                    break;
                case (BuffType.Resistance):
                    {
                        buff = new ResistanceBuff() { Element = (Elements)br.ReadInt32(), Multiplier = br.ReadSingle(), Modifier = br.ReadSingle(), StatusImmunity = br.ReadBoolean() };
                    }
                    break;
                case (BuffType.Undying):
                    {
                        buff = new UndyingBuff();
                    }
                    break;
                case (BuffType.Boost):
                    {
                        buff = new BoostBuff() { BoostAmount = br.ReadSingle() };
                    }
                    break;
                case (BuffType.ReduceAgro):
                    {
                        buff = new ReduceAggroBuff() { AggroReduceAmount = br.ReadSingle() };
                    }
                    break;
                case (BuffType.ModifyHitPoints):
                    {
                        buff = new ModifyHitpointsBuff() { HealthMultiplier = br.ReadSingle(), HealthModifier = br.ReadSingle() };
                    }
                    break;
                case (BuffType.ModifySpellTTL):
                    {
                        buff = new ModifySpellTTLBuff() { SpellTimeMultiplier = br.ReadSingle(), SpellTimeModifier = br.ReadSingle() };
                    }
                    break;
                case (BuffType.ModifySpellRange):
                    {
                        buff = new ModifySpellRangeBuff() { SpellRangeMultiplier = br.ReadSingle(), SpellRangeModifier = br.ReadSingle() };
                    }
                    break;
            }

            buff.BuffVisualCategory = visualCat;
            buff.BuffColor = color;
            buff.BuffTime = buffTime;
            buff.BuffEffect = buffEffect;
            return buff;
        }
        public virtual void Write(BinaryWriter bw)
        {
            bw.Write((byte)buffType);
            bw.Write((byte)BuffVisualCategory);
            foreach (float f in BuffColor)
            {
                bw.Write(f);
            }
            bw.Write(BuffTime);
            bw.Write(BuffEffect);
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
            buffType = BuffType.BoostDamage;
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
            buffType = BuffType.DealDamage;
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
            buffType = BuffType.Resistance;
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
        public BoostBuff() { buffType = BuffType.Boost; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(BoostAmount);
        }
    }
    public class ReduceAggroBuff : Buff
    {
        public float AggroReduceAmount { get; set; }
        public ReduceAggroBuff() { buffType = BuffType.ReduceAgro; }
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
        public ModifyHitpointsBuff() { buffType = BuffType.ModifyHitPoints; }
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
        public ModifySpellTTLBuff() { buffType = BuffType.ModifySpellTTL; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(SpellTimeMultiplier);
            bw.Write(SpellTimeModifier);
        }
    }
    public class UndyingBuff : Buff
    {
        public UndyingBuff() { buffType = BuffType.Undying; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
        }
    }
    public class ModifySpellRangeBuff : Buff
    {
        public float SpellRangeMultiplier { get; set; }
        public float SpellRangeModifier { get; set; }
        public ModifySpellRangeBuff() { buffType = BuffType.ModifySpellRange; }
        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(SpellRangeMultiplier);
            bw.Write(SpellRangeModifier);
        }
    }
}
