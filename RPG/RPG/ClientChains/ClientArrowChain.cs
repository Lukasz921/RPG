using RPG.Chains;
using RPG.Displays;
using RPG.Maps;

namespace RPG.ClientChains
{
    internal class ClientArrowChain : IChain
    {
        public IChain? Next { get; set; }
        public void ProcessKey(ConsoleKeyInfo key, Map map, int playeridx)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.DownArrow:
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
            Display.DrawPlayerInventory(map.Players[playeridx]);
        }
    }
}