namespace RPG.Players
{
    internal class PlayerStats
    {
        public int Power { get; set; } = 25;
        public int Agility { get; set; } = 25;
        public int Health { get; set; } = 100;
        public int Luck { get; set; } = 10;
        public int Aggression { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int CurrencyCounter { get; set; } = 0;
    }
}