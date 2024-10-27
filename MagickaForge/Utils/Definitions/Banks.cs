namespace MagickaForge.Utils.Definitions
{
    [Flags]
    public enum Banks : int
    {
        None = 0,
        WaveBank = 1,
        Music = 2,
        Ambience = 4,
        UI = 8,
        Spells = 16,
        Characters = 32,
        Footsteps = 64,
        Weapons = 128,
        Misc = 256,
        Additional = 512,
        AdditionalMusic = 1024
    }
}
