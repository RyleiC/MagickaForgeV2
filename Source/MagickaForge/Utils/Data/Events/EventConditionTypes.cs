namespace MagickaForge.Utils.Data.Events
{
    [Flags]
    public enum EventConditionTypes : byte
    {
        Default = 1,
        Hit = 2,
        Collision = 4,
        Damaged = 8,
        Timer = 16,
        Death = 32,
        OverKill = 64
    }
}
