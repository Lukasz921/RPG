using RPG.Items;
using RPG.Players;

namespace RPG.Defenses
{
    internal interface IDefense
    {
        int Visit(IItem item, PlayerStats stats);
        int Visit(LightWeapon weapon, PlayerStats stats);
        int Visit(HeavyWeapon heavy, PlayerStats stats);
        int Visit(MagicWeapon magic, PlayerStats stats);
    }
}