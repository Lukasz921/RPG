namespace RPG.Players
{
    internal class PlayerEffect(string name, int leftturns, int powerbooster, int agilitybooster, int healthbooster, int luckbooster, int aggressionbooster, int wisdombooster)
    {
        public string Name { get; set; } = name;
        public int LeftTurns { get; set; } = leftturns;
        public int PowerBooster { get; set; } = powerbooster;
        public int AgilityBooster { get; set; } = agilitybooster;
        public int HealthBooster { get; set; } = healthbooster;
        public int LuckBooster { get; set; } = luckbooster;
        public int AggressionBooster { get; set; } = aggressionbooster;
        public int WisdomBooster { get; set; } = wisdombooster;
        public void Turn(PlayerStats stats)
        {
            if (LeftTurns == -1) return;
            LeftTurns--;
            if (LeftTurns == 0)
            {
                DeAttachEffect(stats);
            }
        }
        public void AttachEffect(List<PlayerEffect> effects, PlayerStats stats)
        {
            effects.Add(this);
            stats.Power += PowerBooster;
            stats.Agility += AgilityBooster;
            stats.Health += HealthBooster;
            stats.Luck += LuckBooster;
            stats.Aggression += AggressionBooster;
            stats.Wisdom += WisdomBooster;
        }
        public void DeAttachEffect(PlayerStats stats)
        {
            stats.Power -= PowerBooster;
            stats.Agility -= AgilityBooster;
            stats.Health -= HealthBooster;
            stats.Luck -= LuckBooster;
            stats.Aggression -= AggressionBooster;
            stats.Wisdom -= WisdomBooster;
        }
    }
}