using RPG.Maps;

namespace RPG.Chains
{
    internal class DropChain : IChain
    {
        public IChain? Next { get; set; }
        public void ProcessKey(ConsoleKeyInfo key, Map map, int playeridx)
        {
            if (key.Key == ConsoleKey.Q) HandleRequest(key, map, playeridx);
            else ((IChain)this).ProcessNext(key, map, playeridx);
        }
        public void HandleRequest(ConsoleKeyInfo key, Map map, int playeridx)
        {
            map.DropItem(playeridx);
        }
    }
}