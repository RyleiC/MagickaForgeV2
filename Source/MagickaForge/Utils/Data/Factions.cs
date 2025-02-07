namespace MagickaForge.Utils.Data
{
    [Flags]
    public enum Factions
    {
        None = 0,
        Evil = 1,
        Wild = 2,
        Friendly = 4,
        Demon = 8,
        Undead = 16,
        Human = 32,
        Wizard = 64,
        Neutral = 255,
        Player0 = 256,
        Player1 = 512,
        Player2 = 1024,
        Player3 = 2048,
        Team_Red = 4096,
        Team_Blue = 8192,
        Players = 16128,
    }
}
