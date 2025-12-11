using RPG.Chains;

namespace RPG.Controlers
{
    internal class Controller
    {
        public List<IChain> Chains { get; set; } = [];
        public void Connect()
        {
            for (int i = 0; i < Chains.Count - 1; i++)
            {
                Chains[i].Next = Chains[i + 1];
            }
        }
    }
}