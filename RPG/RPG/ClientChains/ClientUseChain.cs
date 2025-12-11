using RPG.Chains;
using RPG.Displays;
using RPG.Maps;

namespace RPG.ClientChains
{
    internal class ClientUseChain : IChain
    {
        public IChain? Next { get; set; }
        public void ProcessKey(ConsoleKeyInfo key, Map map, int playeridx)
        {
            if (key.Key == ConsoleKey.F) HandleRequest(key, map, playeridx);
            else ((IChain)this).ProcessNext(key, map, playeridx);
        }
        public void HandleRequest(ConsoleKeyInfo key, Map map, int playeridx)
        {
            Display.DrawGoodInput();
            Display.DrawPlayerStats(map.Players[playeridx].Stats);
            Display.DrawPlayerInventory(map.Players[playeridx]);
            Display.DrawPotions(map.Players[playeridx].Effects);
            Display.DrawAttacks(map.Players[playeridx]);
        }
    }
}