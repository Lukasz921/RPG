using RPG.Items;
using System.Text.Json.Serialization;

namespace RPG.Tiles
{
    internal class ItemTile : ITile
    {
        [JsonConstructor]
        public ItemTile() { }
        public ItemTile(IItem? item)
        {
            if (item != null)
            {
                Items.Push(item);
                Symbol = item.Symbol;
            }
            else Symbol = ' ';
        }
        public Stack<IItem> Items { get; set; } = new Stack<IItem>();
        public char Symbol { get; set; }
        public IItem? GetItem()
        {
            if (Items.Count == 0) return null;
            else return Items.Peek();
        }
        public void AddItem(IItem item)
        {
            Items.Push(item);
            Symbol = item.Symbol;
        }
        public void RemoveItem()
        {
            Items.Pop();
            if (Items.Count == 0) Symbol = ' ';
            else Symbol = Items.Peek().Symbol;
        }
        public bool IsPassable() { return true; }
    }
}