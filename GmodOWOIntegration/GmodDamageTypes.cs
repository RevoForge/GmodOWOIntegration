using OWOGame;

namespace GmodOWOIntegration
{
    public class GmodDamageTypes
    {
        public Dictionary<string, Sensation> DamageTypes = new()
        {
            { "DMG_BULLET", GmodSensations.DMG_BULLET },
            { "DMG_FALL", GmodSensations.DMG_FALL },
        };
    }
}
