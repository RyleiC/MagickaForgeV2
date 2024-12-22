namespace MagickaForge.Utils.Data.Events
{
    public enum EventType : byte
    {
        Damage,
        Splash,
        Sound,
        Effect,
        Remove,
        CameraShake,
        Decal,
        Blast, //INVALID IN-GAME
        Spawn,
        Overkill,
        SpawnGibs,
        SpawnItem,
        SpawnMagick,
        SpawnMissile,
        Light,
        CastMagick,
        DamageOwner,
        Callback //INVALID IN-GAME
    }
}
