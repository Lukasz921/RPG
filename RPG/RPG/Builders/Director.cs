namespace RPG.Builders
{
    internal class Director
    {
        public static void BuildClassic(IBuilder builder)
        {
            builder.NullDungeon();
            builder.WallsDungeon();
            builder.AddRandomPaths();
            builder.AddRandomRooms();
            builder.AddBorders();
            builder.AddCentralRoom();
            builder.AddLightWeapons();
            builder.AddHeavyWeapons();
            builder.AddMagicWeapons();
            builder.AddDecoratedWeapons();
            builder.AddCurrency();
            builder.AddUnusable();
            builder.AddPotions();
            builder.AddMonsters();
        }
    }
}