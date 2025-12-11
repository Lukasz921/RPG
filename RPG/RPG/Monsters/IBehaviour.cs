using RPG.Maps;
using System.Text.Json.Serialization;

namespace RPG.Monsters
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(PassiveBehaviour), "PassiveBehaviour")]
    [JsonDerivedType(typeof(AggressiveBehaviour), "AggressiveBehaviour")]
    [JsonDerivedType(typeof(FleeingBehaviour), "FleeingBehaviour")]
    internal interface IBehaviour
    {
        void Execute(Monster monster, Map map);
    }
}