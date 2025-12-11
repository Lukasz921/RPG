using RPG.Chains;

namespace RPG.Builders
{
    internal class ChainBuilder : IBuilder
    {
        public List<IChain> Chains { get; set; } = [];
        public void NullDungeon()
        {
            EndChain endChain = new();
            Chains.Add(endChain);
        }
        public void WallsDungeon()
        {

        }
        public void AddRandomPaths()
        {
            MoveChain moveChain = new();
            Chains.Add(moveChain);
        }
        public void AddBorders()
        {

        }
        public void AddCentralRoom()
        {

        }
        public void AddRandomRooms()
        {

        }
        public void AddLightWeapons()
        {
            PickUpChain pickUpChain = new();
            Chains.Add(pickUpChain);
        }
        public void AddHeavyWeapons()
        {
            DropChain dropChain = new();
            Chains.Add(dropChain);
        }
        public void AddMagicWeapons()
        {
            DropAllChain dropAllChain = new();
            Chains.Add(dropAllChain);
        }
        public void AddDecoratedWeapons()
        {

        }
        public void AddCurrency()
        {
            HandsChain handsChain = new();
            Chains.Add(handsChain);
        }
        public void AddUnusable()
        {
            ArrowChain arrowChain = new();
            Chains.Add(arrowChain);
        }
        public void AddPotions()
        {
            UseChain useChain = new();
            Chains.Add(useChain);
        }
        public void AddMonsters()
        {
            FightChain fightChain = new();
            Chains.Add(fightChain);
        }
    }
}