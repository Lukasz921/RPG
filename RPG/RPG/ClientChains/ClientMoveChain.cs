using RPG.Chains;
using RPG.Displays;
using RPG.Maps;

namespace RPG.ClientChains
{
    internal class ClientMoveChain : IChain
    {
        public IChain? Next { get; set; }
        public void ProcessKey(ConsoleKeyInfo key, Map map, int playeridx)
        {
            switch (key.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.S:
                case ConsoleKey.A:
                case ConsoleKey.D:
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
            Display.DrawPlayerMove(map.Board, map.Players[playeridx]);
            Display.DrawMonster(map, map.Players[playeridx]);
            Display.DrawPlayerStats(map.Players[playeridx].Stats);
            Display.DrawPlayerInventory(map.Players[playeridx]);
            Display.DrawPotions(map.Players[playeridx].Effects);
        }
    }
}