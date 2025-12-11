using RPG.Attacks;
using RPG.Defenses;
using RPG.Players;
using System.Text.Json.Serialization;

namespace RPG.Items
{
    class Potion(string name, char symbol, int damage, bool isTwoHanded, PlayerEffect effect) : IPotion
    {
        [JsonIgnore]
        public string RawName { get; set; } = name;
        public string Name { get; set; } = name;
        public char Symbol { get; set; } = symbol;
        public int Damage { get; set; } = damage;
        public bool IsTwoHanded { get; set; } = isTwoHanded;
        public PlayerEffect Effect { get; set; } = effect;
        public void Use(List<PlayerEffect> effects, PlayerStats stats)
        {
            Effect.AttachEffect(effects, stats);
        }
        public bool IsUsable() { return true; }
        public bool ApplyOnPickUp(PlayerStats stats) { return true; }
        public bool DeApplyOnThrow(PlayerStats stats) { return true; }
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