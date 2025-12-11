using RPG.Items;

namespace RPG.Attacks
{
    internal class MagicAttack : IAttack
    {
        public int Visit(IItem item)
        {
            return item.Damage;
        }
        public int Visit(LightWeapon light)
        {
            return 1;
        }
        public int Visit(HeavyWeapon heavy)
        {
            return 1;
        }
        public int Visit(MagicWeapon magic)
        {
            return magic.Damage;
        }
    }
}