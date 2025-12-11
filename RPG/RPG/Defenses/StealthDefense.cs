using RPG.Items;
using RPG.Players;

namespace RPG.Defenses
{
    internal class StealthDefense : IDefense
    {
        public int Visit(IItem item, PlayerStats stats)
        {
            return 0;
        }
        public int Visit(LightWeapon light, PlayerStats stats)
        {
            return stats.Agility;
        }
        public int Visit(HeavyWeapon heavy, PlayerStats stats)
        {
            return stats.Power;
        }
        public int Visit(MagicWeapon magic, PlayerStats stats)
        {
            return 0;
        }
    }
}