using RPG.Items;
using RPG.Players;

namespace RPG.Defenses
{
    internal class NormalDefense : IDefense
    {
        public int Visit(IItem item, PlayerStats stats)
        {
            return stats.Agility;
        }
        public int Visit(LightWeapon light, PlayerStats stats)
        {
            return stats.Agility + stats.Luck;
        }
        public int Visit(HeavyWeapon heavy, PlayerStats stats)
        {
            return stats.Power + stats.Luck;
        }
        public int Visit(MagicWeapon magic, PlayerStats stats)
        {
            return stats.Agility + stats.Luck;
        }
    }
}