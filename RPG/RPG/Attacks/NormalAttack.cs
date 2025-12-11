using RPG.Items;

namespace RPG.Attacks
{
    internal class NormalAttack : IAttack
    {
        public int Visit(IItem item)
        {
            return item.Damage;
        }
        public int Visit(LightWeapon light)
        {
            return light.Damage;
        }
        public int Visit(HeavyWeapon heavy)
        {
            return heavy.Damage;
        }
        public int Visit(MagicWeapon magic)
        {
            return 1;
        }
    }
}