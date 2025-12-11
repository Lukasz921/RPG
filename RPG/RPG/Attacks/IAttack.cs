using RPG.Items;

namespace RPG.Attacks
{
    internal interface IAttack
    {
        int Visit(IItem item);
        int Visit(LightWeapon weapon);
        int Visit(HeavyWeapon heavy);
        int Visit(MagicWeapon magic);
    }
}