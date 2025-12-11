using RPG.Chains;
using RPG.Displays;
using RPG.Maps;

namespace RPG.ClientChains
{
    internal class ClientDropAllChain : IChain
    {
        public IChain? Next { get; set; }
        public void ProcessKey(ConsoleKeyInfo key, Map map, int playeridx)
        {
            if (key.Key == ConsoleKey.G) HandleRequest(key, map, playeridx);
            else ((IChain)this).ProcessNext(key, map, playeridx);
        }
        public void HandleRequest(ConsoleKeyInfo key, Map map, int playeridx)
        {
            Display.DrawGoodInput();
            Display.DrawPlayerMove(map.Board, map.Players[playeridx]);
            Display.DrawPlayerStats(map.Players[playeridx].Stats);
            Display.DrawPlayerCurrency(map.Players[playeridx].Stats);
            Display.DrawPlayerInventory(map.Players[playeridx]);
        }
    }
}