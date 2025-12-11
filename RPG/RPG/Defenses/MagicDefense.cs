using RPG.Items;
using RPG.Players;

namespace RPG.Defenses
{
    internal class MagicDefense : IDefense
    {
        public int Visit(IItem item, PlayerStats stats)
        {
            return stats.Luck;
        }
        public int Visit(LightWeapon light, PlayerStats stats)
        {
            return stats.Luck;
        }
        public int Visit(HeavyWeapon heavy, PlayerStats stats)
        {
            return stats.Luck;
        }
        public int Visit(MagicWeapon magic, PlayerStats stats)
        {
            return 2 * stats.Wisdom;
        }
    }
}