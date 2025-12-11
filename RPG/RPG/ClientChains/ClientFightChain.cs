using RPG.Chains;
using RPG.Displays;
using RPG.Maps;

namespace RPG.ClientChains
{
    internal class ClientFightChain : IChain
    {
        public IChain? Next { get; set; }
        public void ProcessKey(ConsoleKeyInfo key, Map map, int playeridx)
        {
            switch (key.Key)
            {
                case ConsoleKey.M:
                case ConsoleKey.N:
                case ConsoleKey.B:
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
            Display.DrawMonster(map, map.Players[playeridx]);
            Display.DrawPlayerStats(map.Players[playeridx].Stats);
        }
    }
}