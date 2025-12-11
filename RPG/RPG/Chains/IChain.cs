using RPG.Maps;

namespace RPG.Chains
{
    internal interface IChain
    {
        public IChain? Next { get; set; }
        void SetNext(IChain next)
        {
            Next = next;
        }
        void ProcessKey(ConsoleKeyInfo key, Map map, int playeridx);
        void ProcessNext(ConsoleKeyInfo key, Map map, int playeridx)
        {
            Next?.ProcessKey(key, map, playeridx);
        }
        void HandleRequest(ConsoleKeyInfo key, Map map, int playeridx);
    }
}