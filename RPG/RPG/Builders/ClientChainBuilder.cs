using RPG.Chains;
using RPG.ClientChains;

namespace RPG.Builders
{
    internal class ClientChainBuilder : IBuilder
    {
        public List<IChain> Chains { get; set; } = [];
        public void NullDungeon()
        {

        }
        public void WallsDungeon()
        {

        }
        public void AddRandomPaths()
        {
            ClientMoveChain moveChain = new();
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
            ClientPickUpChain pickUpChain = new();
            Chains.Add(pickUpChain);
        }
        public void AddHeavyWeapons()
        {
            ClientDropChain dropChain = new();
            Chains.Add(dropChain);
        }
        public void AddMagicWeapons()
        {
            ClientDropAllChain dropAllChain = new();
            Chains.Add(dropAllChain);
        }
        public void AddDecoratedWeapons()
        {

        }
        public void AddCurrency()
        {
            ClientHandsChain handsChain = new();
            Chains.Add(handsChain);
        }
        public void AddUnusable()
        {
            ClientArrowChain arrowChain = new();
            Chains.Add(arrowChain);
        }
        public void AddPotions()
        {
            ClientUseChain useChain = new();
            Chains.Add(useChain);
        }
        public void AddMonsters()
        {
            ClientFightChain fightChain = new();
            Chains.Add(fightChain);
        }
    }
}