using RPG.Items;

namespace RPG.Attacks
{
    internal class StealthAttack : IAttack
    {
        public int Visit(IItem item)
        {
            return item.Damage;
        }
        public int Visit(LightWeapon light)
        {
            int damage = light.Damage;
            return 2 * damage;
        }
        public int Visit(HeavyWeapon heavy)
        {
            int damage = heavy.Damage;
            return damage / 2;
        }
        public int Visit(MagicWeapon magic)
        {
            return 1;
        }
    }
}