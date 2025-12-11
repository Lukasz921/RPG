using RPG.Chains;
using RPG.Displays;
using RPG.Maps;

namespace RPG.ClientChains
{
    internal class ClientHandsChain : IChain
    {
        public IChain? Next { get; set; }
        public void ProcessKey(ConsoleKeyInfo key, Map map, int playeridx)
        {
            switch (key.Key)
            {
                case ConsoleKey.L:
                case ConsoleKey.P:
                    HandleRequest(key, map, playeridx);
                    break;
                default:
                    ((IChain)this).ProcessNext(key, map, playeridx);
                    break;
            }
        }
        public void HandleRequest(ConsoleKeyInfo key, Map map, int playeridx)
        {
            Display.DrawGoodInput();
            Display.DrawPlayerStats(map.Players[playeridx].Stats);
            Display.DrawPlayerHands(map.Players[playeridx]);
            Display.DrawPlayerInventory(map.Players[playeridx]);
            Display.DrawAttacks(map.Players[playeridx]);
        }
    }
}