namespace RPG.Monsters
{
    internal class MonsterStats(int damage, int health, int defense)
    {
        public int Damage { get; set; } = damage;
        public int Health { get; set; } = health;
        public int MaxHealth { get; set; } = health;
        public int Defense { get; set; } = defense;
    }
}