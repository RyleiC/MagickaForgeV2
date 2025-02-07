using MagickaForge.Utils.Helpers;
namespace MagickaForge.Utils.Data
{
    [EnumeratedString]
    [Flags]
    public enum Elements
    {
        None = 0,
        Earth = 1,
        Water = 2,
        Cold = 4,
        Fire = 8,
        Lightning = 16,
        Arcane = 32,
        Life = 64,
        Shield = 128,
        Ice = 256,
        Steam = 512,
        Poison = 1024,
        Offensive = 65343,
        Defensive = 176,
        All = 65535,
        Basic = 255,
        Instant = 881,
        InstantPhysical = 369,
        InstantNonPhysical = 624,
        StatusEffects = 1614,
        ShieldElements = 224,
        PhysicalElements = 257,
        Beams = 96
    }
}
