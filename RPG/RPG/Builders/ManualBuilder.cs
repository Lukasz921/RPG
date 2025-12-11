namespace RPG.Builders
{
    internal class ManualBuilder : IBuilder
    {
        public List<string> Manual { get; set; } = [];

        public void NullDungeon()
        {
            Manual.Add("RPG GAME!");
            Manual.Add("AUTHOR: LUKASZ PRZYBYLSKI");
            Manual.Add("");
            Manual.Add("");
        }
        public void WallsDungeon()
        {
            Manual.Add("No noclip - can't go through walls!");
        }
        public void AddRandomPaths()
        {
            Manual.Add("Don't get lost in dungeon!");
        }
        public void AddBorders()
        {
            Manual.Add("You can't escape dungeon!");
        }
        public void AddCentralRoom()
        {
            Manual.Add("Central room is the biggest one!");
        }
        public void AddRandomRooms()
        {
            Manual.Add("There are plenty of other rooms!");
        }
        public void AddLightWeapons()
        {
            Manual.Add("Light weapons are fast!");
        }
        public void AddHeavyWeapons()
        {
            Manual.Add("Heavy weapons are powerful!");
        }
        public void AddMagicWeapons()
        {
            Manual.Add("Magic weapons are the greatest!");
        }
        public void AddDecoratedWeapons()
        {
            Manual.Add("Some weapons are better than other ones!");
        }
        public void AddCurrency()
        {
            Manual.Add("Gather more and more money!");
        }
        public void AddUnusable()
        {
            Manual.Add("Unusable is dogshit!");
        }
        public void AddPotions()
        {
            Manual.Add("Drink potions to become greater!");
        }
        public void AddMonsters()
        {
            Manual.Add("Fight monsters with different attacks!");
        }
    }
}