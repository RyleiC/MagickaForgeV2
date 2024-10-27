namespace MagickaForge.Utils.Definitions.Animations
{
    [Flags]
    public enum Targets : byte
    {
        None = 0,
        Target = 0,
        Friendly = 1,
        Enemy = 2,
        NonCharacters = 4,
        All = 255
    }
}
