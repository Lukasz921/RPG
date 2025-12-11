using RPG.Maps;

namespace RPG.Chains
{
    internal class HandsChain : IChain
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
            map.PlayerSetHand(key, playeridx);
        }
    }
}