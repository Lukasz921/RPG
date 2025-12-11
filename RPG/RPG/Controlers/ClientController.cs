using RPG.Chains;
using RPG.ClientChains;

namespace RPG.Controlers
{
    internal class ClientController
    {
        public List<IChain> Chains { get; set; } = [];
        public void Connect()
        {
            ClientWrongInputChain clientWrongInputChain = new();
            Chains.Add(clientWrongInputChain);
            for (int i = 0; i < Chains.Count - 1; i++)
            {
                Chains[i].Next = Chains[i + 1];
            }
        }
    }
}