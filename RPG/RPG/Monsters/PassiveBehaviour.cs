using RPG.Maps;
using System.Text.Json.Serialization;

namespace RPG.Monsters
{
    internal class PassiveBehaviour : IBehaviour
    {
        [JsonConstructor]
        public PassiveBehaviour() { }
        public void Execute(Monster monster, Map map) { }
    }
}