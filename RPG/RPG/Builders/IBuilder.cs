namespace RPG.Builders
{
    internal interface IBuilder
    {
        void NullDungeon();
        void WallsDungeon();
        void AddRandomPaths();
        void AddBorders();
        void AddCentralRoom();
        void AddRandomRooms();
        void AddLightWeapons();
        void AddHeavyWeapons();
        void AddMagicWeapons();
        void AddDecoratedWeapons();
        void AddCurrency();
        void AddUnusable();
        void AddPotions();
        void AddMonsters();
    }
}