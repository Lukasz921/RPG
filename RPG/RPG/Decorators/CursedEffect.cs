using RPG.Attacks;
using RPG.Defenses;
using RPG.Items;
using RPG.Players;
using System.Text.Json.Serialization;

namespace RPG.Decorators
{
    [method: JsonConstructor]
    internal class CursedEffect(IWeapon weapon, string rawName, char symbol, int damage, bool isTwoHanded) : IWeapon
    {
        public CursedEffect(IWeapon weapon) : this(weapon, weapon.Name, weapon.Symbol, weapon.Damage - 10, weapon.IsTwoHanded) { }
        public IWeapon Weapon { get; set; } = weapon;
        public int Booster { get; set; } = -10;
        public string RawName { get; set; } = rawName;
        [JsonIgnore]
        public string Name
        {
            get => RawName + " (Cursed)";
            set => RawName = value;
        }
        public char Symbol { get; set; } = symbol;
        public int Damage { get; set; } = damage;
        public bool IsTwoHanded { get; set; } = isTwoHanded;
        public int RealDamageTaken { get; set; }
        public void Use(List<PlayerEffect> effects, PlayerStats stats) { }
        public bool IsUsable() { return false; }
        public bool ApplyOnPickUp(PlayerStats stats) { return true; }
        public bool DeApplyOnThrow(PlayerStats stats) { return true; }
        public void ApplyOnPlayer(PlayerStats stats)
        {
            stats.Health += Booster;
            if (stats.Health <= 0)
            {
                RealDamageTaken = stats.Health - Booster - 1;
                stats.Health = 1;
            }
            else RealDamageTaken = -Booster;
        }
        public void DeApplyOnPlayer(PlayerStats stats)
        {
            stats.Health += RealDamageTaken;
        }
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