using RPG.Attacks;
using RPG.Defenses;
using RPG.Players;
using System.Text.Json.Serialization;

namespace RPG.Items
{
    [method: JsonConstructor]
    internal class LightWeapon(string rawname, char symbol, int damage, bool isTwoHanded) : IWeapon
    {
        [JsonIgnore]
        public string Name
        {
            get => "(Light) " + RawName + (IsTwoHanded ? " (Two-Handed)" : "");
            set => RawName = value;
        }
        public string RawName { get; set; } = rawname;
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
            return attack.Visit(this);
        }
        public int Accept(IDefense defense, PlayerStats stats)
        {
            return defense.Visit(this, stats);
        }
    }
}
