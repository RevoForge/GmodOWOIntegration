using OWOGame;

namespace GmodOWOIntegration
{
    public class GmodDamageTypes
    {
        public Dictionary<string, Sensation> DamageTypes = new()
        {
            { "DMG_GENERIC", GmodSensations.DMG_GENERIC },
            { "DMG_CRUSH", GmodSensations.DMG_CRUSH },
            { "DMG_BULLET", GmodSensations.DMG_BULLET },
            { "DMG_SLASH", GmodSensations.DMG_SLASH },
            { "DMG_BURN", GmodSensations.DMG_BURN },
            { "DMG_VEHICLE", GmodSensations.DMG_VEHICLE },
            { "DMG_FALL", GmodSensations.DMG_FALL },
            { "DMG_BLAST", GmodSensations.DMG_BLAST },
            { "DMG_CLUB", GmodSensations.DMG_CLUB },
            { "DMG_SHOCK", GmodSensations.DMG_SHOCK },
            { "DMG_SONIC", GmodSensations.DMG_SONIC },
            { "DMG_ENERGYBEAM", GmodSensations.DMG_ENERGYBEAM },
            { "DMG_PREVENT_PHYSICS_FORCE", GmodSensations.DMG_PREVENT_PHYSICS_FORCE },
            { "DMG_NEVERGIB", GmodSensations.DMG_NEVERGIB },
            { "DMG_ALWAYSGIB", GmodSensations.DMG_ALWAYSGIB },
            { "DMG_DROWN", GmodSensations.DMG_DROWN },
            { "DMG_PARALYZE", GmodSensations.DMG_PARALYZE },
            { "DMG_NERVEGAS", GmodSensations.DMG_NERVEGAS },
            { "DMG_POISON", GmodSensations.DMG_POISON },
            { "DMG_RADIATION", GmodSensations.DMG_RADIATION },
            { "DMG_DROWNRECOVER", GmodSensations.DMG_DROWNRECOVER },
            { "DMG_ACID", GmodSensations.DMG_ACID },
            { "DMG_SLOWBURN", GmodSensations.DMG_SLOWBURN },
            { "DMG_REMOVENORAGDOLL", GmodSensations.DMG_REMOVENORAGDOLL },
            { "DMG_PHYSGUN", GmodSensations.DMG_PHYSGUN },
            { "DMG_PLASMA", GmodSensations.DMG_PLASMA },
            { "DMG_AIRBOAT", GmodSensations.DMG_AIRBOAT },
            { "DMG_DISSOLVE", GmodSensations.DMG_DISSOLVE },
            { "DMG_BLAST_SURFACE", GmodSensations.DMG_BLAST_SURFACE },
            { "DMG_DIRECT", GmodSensations.DMG_DIRECT },
            { "DMG_BUCKSHOT", GmodSensations.DMG_BUCKSHOT },
            { "DMG_SNIPER", GmodSensations.DMG_SNIPER },
            { "DMG_MISSILEDEFENSE", GmodSensations.DMG_MISSILEDEFENSE }
        };
    }
    [Serializable]
    public enum Hitbox
    {
        HITGROUP_GENERIC = 0,
        HITGROUP_HEAD = 1,
        HITGROUP_CHEST = 2,
        HITGROUP_STOMACH = 3,
        HITGROUP_LEFTARM = 4,
        HITGROUP_RIGHTARM = 5,
        HITGROUP_LEFTLEG = 6,
        HITGROUP_RIGHTLEG = 7
    }
}
