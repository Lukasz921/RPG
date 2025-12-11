using System.Text.Json.Serialization;

namespace RPG.Monsters
{
    [method: JsonConstructor]
    internal class Monster(int x, int y, string name, char symbol, MonsterStats stats, IBehaviour behaviour)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public string Name { get; set; } = name;
        public char Symbol { get; set; } = symbol;
        public MonsterStats Stats { get; set; } = stats;
        public IBehaviour Behaviour { get; set; } = behaviour;
        public void ResolveFight(int damage)
        {
            int realdamage = damage - Stats.Defense;
            if (realdamage > 0)
                Stats.Health -= realdamage;
        }
        public void ChangeBehaviour()
        {
            if (Stats.MaxHealth / 3 > Stats.Health) Behaviour = new FleeingBehaviour();
        }
    }
}
