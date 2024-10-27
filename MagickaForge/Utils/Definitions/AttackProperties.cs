namespace MagickaForge.Utils.Definitions
{
    [Flags]
    public enum AttackProperties : int
    {
        Damage = 1,
        Knockdown = 2,
        Pushed = 4,
        Knockback = 6,
        Piercing = 8,
        ArmourPiercing = 16,
        Status = 32,
        Entanglement = 64,
        Stun = 128,
        Bleed = 256
    }
}
