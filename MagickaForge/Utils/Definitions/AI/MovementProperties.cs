namespace MagickaForge.Utils.Definitions.AI
{
    [Flags]
    public enum MovementProperties
    {
        Default = 0,
        Water = 1,
        Jump = 2,
        Fly = 4,
        Dynamic = 128,
        All = 255
    }
}
