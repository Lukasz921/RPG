using RPG.Maps;

namespace RPG.Chains
{
    internal class PickUpChain : IChain
    {
        public IChain? Next { get; set; }
        public void ProcessKey(ConsoleKeyInfo key, Map map, int playeridx)
        {
            if (key.Key == ConsoleKey.E) HandleRequest(key, map, playeridx);
            else ((IChain)this).ProcessNext(key, map, playeridx);
        }
        public void HandleRequest(ConsoleKeyInfo key, Map map, int playeridx)
        {
            map.PickUpItem(playeridx);
        }
    }
}