using RPG.Chains;
using RPG.Displays;
using RPG.Maps;

namespace RPG.ClientChains
{
    internal class ClientWrongInputChain : IChain
    {
        public IChain? Next { get; set; }
        public void ProcessKey(ConsoleKeyInfo key, Map map, int playeridx)
        {
            HandleRequest(key, map, playeridx);
        }
        public void HandleRequest(ConsoleKeyInfo key, Map map, int playeridx)
        {
            Display.DrawWrongInput();
        }
    }
}