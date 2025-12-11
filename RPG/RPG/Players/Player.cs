using RPG.Attacks;
using RPG.Defenses;
using RPG.Items;

namespace RPG.Players
{
    internal class Player(char symbol)
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public char Symbol { get; set; } = symbol;
        public PlayerStats Stats { get; set; } = new PlayerStats();
        public List<PlayerEffect> Effects { get; set; } = [];
        public List<IItem> Inventory { get; set; } = [];
        public int SelectedItemIndex { get; set; } = -1;
        public int MaxInventoryCount { get; set; } = 10;
        public IItem? LeftHand { get; set; }
        public IItem? RightHand { get; set; }
        public void PickupItem(IItem item)
        {
            if (Inventory.Count == 0) SelectedItemIndex = 0;
            if (item.ApplyOnPickUp(Stats))
            {
                Inventory.Add(item);
            }
        }
        public void RemoveItem()
        {
            if (SelectedItemIndex >= 0 && SelectedItemIndex < Inventory.Count)
            {
                Inventory[SelectedItemIndex].DeApplyOnThrow(Stats);
                Inventory.RemoveAt(SelectedItemIndex);
            }
            if (SelectedItemIndex == Inventory.Count) SelectedItemIndex--;
        }
        public void NavigateInventory(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.UpArrow)
            {
                if (SelectedItemIndex == 0) SelectedItemIndex = Inventory.Count - 1;
                else SelectedItemIndex--;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                if (SelectedItemIndex == Inventory.Count - 1) SelectedItemIndex = 0;
                else SelectedItemIndex++;
            }
        }
        public void SetHand(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.L && LeftHand != null && MaxInventoryCount == Inventory.Count) return;
            if (key.Key == ConsoleKey.P && RightHand != null && MaxInventoryCount == Inventory.Count) return;
            if (key.Key == ConsoleKey.L)
            {
                if (LeftHand != null)
                {
                    LeftHand.DeApplyOnPlayer(Stats);
                    PickupItem(LeftHand);
                    if (LeftHand == RightHand) RightHand = null;
                    LeftHand = null;
                }
                else if (Inventory.Count > 0)
                {
                    if (Inventory[SelectedItemIndex].IsTwoHanded)
                    {
                        if (RightHand == null)
                        {
                            Inventory[SelectedItemIndex].ApplyOnPlayer(Stats);
                            LeftHand = Inventory[SelectedItemIndex];
                            RightHand = Inventory[SelectedItemIndex];
                            RemoveItem();
                        }
                    }
                    else
                    {
                        Inventory[SelectedItemIndex].ApplyOnPlayer(Stats);
                        LeftHand = Inventory[SelectedItemIndex];
                        RemoveItem();
                    }
                }
            }
            else if (key.Key == ConsoleKey.P)
            {
                if (RightHand != null)
                {
                    RightHand.DeApplyOnPlayer(Stats);
                    PickupItem(RightHand);
                    if (LeftHand == RightHand) LeftHand = null;
                    RightHand = null;
                }
                else if (Inventory.Count > 0)
                {
                    if (Inventory[SelectedItemIndex].IsTwoHanded)
                    {
                        if (LeftHand == null)
                        {
                            Inventory[SelectedItemIndex].ApplyOnPlayer(Stats);
                            LeftHand = Inventory[SelectedItemIndex];
                            RightHand = Inventory[SelectedItemIndex];
                            RemoveItem();
                        }
                    }
                    else
                    {
                        Inventory[SelectedItemIndex].ApplyOnPlayer(Stats);
                        RightHand = Inventory[SelectedItemIndex];
                        RemoveItem();
                    }
                }
            }
        }
        public void Turn()
        {
            for (int i = Effects.Count - 1; i >= 0; i--)
            {
                PlayerEffect effect = Effects[i];
                effect.Turn(Stats);
                if (effect.LeftTurns == 0) Effects.RemoveAt(i);
            }
        }
        public int Damage(IAttack attack)
        {
            int damage1 = 0;
            if (LeftHand != null) damage1 = LeftHand.Accept(attack);
            int damage2 = 0;
            if (RightHand != null) damage2 = RightHand.Accept(attack);

            if (LeftHand != null && RightHand != null)
            {
                if (LeftHand.IsTwoHanded) return damage1;
                else return damage1 + damage2;
            }
            else return damage1 + damage2;
        }
        public int Defense(IDefense defense)
        {
            int defense1 = 0;
            if (LeftHand != null) defense1 = LeftHand.Accept(defense, Stats);
            int defense2 = 0;
            if (RightHand != null ) defense2 = RightHand.Accept(defense, Stats);

            if (LeftHand != null && RightHand != null)
            {
                if (LeftHand.IsTwoHanded) return defense1;
                else return defense1 + defense2;
            }
            return defense1 + defense2;
        }
        public void ResolveFight(int damage, IDefense defense)
        {
            int realdamage = damage - Defense(defense);
            if (realdamage > 0) Stats.Health -= realdamage;
        }
    }
}