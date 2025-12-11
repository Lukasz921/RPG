using RPG.Attacks;
using RPG.Defenses;
using RPG.Items;
using RPG.Players;
using System.Text.Json.Serialization;

namespace RPG.Decorators
{
    [method: JsonConstructor]
    internal class StrongEffect(IWeapon weapon, string rawName, char symbol, int damage, bool isTwoHanded) : IWeapon
    {
        public StrongEffect(IWeapon weapon) : this(weapon, weapon.Name, weapon.Symbol, weapon.Damage + 5, weapon.IsTwoHanded) { }
        public IWeapon Weapon { get; set; } = weapon;
        public int Booster { get; set; } = 5;
        public string RawName { get; set; } = rawName;
        [JsonIgnore]
        public string Name
        {
            get => RawName + " (Strong)";
            set => RawName = value;
        }
        public char Symbol { get; set; } = symbol;
        public int Damage { get; set; } = damage;
        public bool IsTwoHanded { get; set; } = isTwoHanded;
        public void Use(List<PlayerEffect> effects, PlayerStats stats) { }
        public bool IsUsable() { return false; }
        public bool ApplyOnPickUp(PlayerStats stats) { return true; }
        public bool DeApplyOnThrow(PlayerStats stats) { return true; }
        public void ApplyOnPlayer(PlayerStats stats) { }
        public void DeApplyOnPlayer(PlayerStats stats) { }
        public int Accept(IAttack attack)
        {
            int damage = Weapon.Accept(attack);
            if (damage + Booster < 0) return 0;
            else return damage + Booster;
        }
        public int Accept(IDefense defense, PlayerStats stats)
        {
            return Weapon.Accept(defense, stats);
        }
    }
}