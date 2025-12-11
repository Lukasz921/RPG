using RPG.Items;

namespace RPG.Tiles
{
    internal class WallTile : ITile
    {
        public Stack<IItem> Items { get; set; } = [];
        public char Symbol { get; set; } = '█';
        public IItem? GetItem() { return null; }
        public void AddItem(IItem item) { }
        public void RemoveItem() { }
        public bool IsPassable() { return false; }
    }
}