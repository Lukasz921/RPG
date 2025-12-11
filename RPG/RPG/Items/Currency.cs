using RPG.Attacks;
using RPG.Defenses;
using RPG.Players;
using System.Text.Json.Serialization;

namespace RPG.Items
{
    internal class Currency(string name, char symbol, int damage, bool isTwoHanded, int value) : ICurrency
    {
        [JsonIgnore]
        public string RawName { get; set; } = name;
        public string Name { get; set; } = name;
        public char Symbol { get; set; } = symbol;
        public int Damage { get; set; } = damage;
        public bool IsTwoHanded { get; set; } = isTwoHanded;
        public int Value { get; set; } = value;
        public void Use(List<PlayerEffect> effects, PlayerStats stats) { }
        public bool IsUsable() { return false; }
        public bool ApplyOnPickUp(PlayerStats stats)
        {
            stats.CurrencyCounter += Value;
            return false;
        }
        public bool DeApplyOnThrow(PlayerStats stats)
        {
            stats.CurrencyCounter -= Value;
            return false;
        }
        public void ApplyOnPlayer(PlayerStats stats) { }
        public void DeApplyOnPlayer(PlayerStats stats) { }
        public int Accept(IAttack attack)
        {
            return attack.Visit(this);
        }
        public int Accept(IDefense defense, PlayerStats stats)
        {
            return defense.Visit(this, stats);
        }
    }
}