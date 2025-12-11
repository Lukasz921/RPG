using RPG.Items;
using System.Text.Json.Serialization;

namespace RPG.Tiles
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(ItemTile), "ItemTile")]
    [JsonDerivedType(typeof(WallTile), "WallTile")]
    internal interface ITile
    {
        Stack<IItem> Items { get; set; }
        char Symbol { get; set; }
        IItem? GetItem();
        void AddItem(IItem item);
        void RemoveItem();
        bool IsPassable();
    }
}