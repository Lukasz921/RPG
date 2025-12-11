using RPG.Maps;

namespace RPG.Chains
{
    internal class MoveChain : IChain
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
            map.MovePlayer(key, playeridx);
        }
    }
}