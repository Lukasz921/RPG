using RPG.Attacks;
using RPG.Defenses;
using RPG.Monsters;
using RPG.Players;
using RPG.Tiles;

namespace RPG.Maps
{
    internal class Map(int height, int width)
    {
        public int Height { get; } = height;
        public int Width { get; } = width;
        public required ITile[][] Board { get; set; }
        public Dictionary<int, Player> Players { get; set; } = [];
        public List<Monster> Monsters { get; set; } = [];
        public NormalAttack NormalAttack { get; set; } = new();
        public StealthAttack StealthAttack { get; set; } = new();
        public MagicAttack MagicAttack { get; set; } = new();
        public NormalDefense NormalDefense { get; set; } = new();
        public StealthDefense StealthDefense { get; set; } = new();
        public MagicDefense MagicDefense { get; set; } = new();
        public void AddPlayer(Player player, int playerId)
        {
            while (true)
            {
                int x = new Random().Next(1, Width);
                int y = new Random().Next(1, Height);
                if (Board[y][x].IsPassable())
                {
                    player.X = x;
                    player.Y = y;
                    break;
                }
            }
            Players.Add(playerId, player);
        }
        public void RemovePlayer(int playerId)
        {
            Players.Remove(playerId);
        }
        public void MovePlayer(ConsoleKeyInfo key, int playeridx)
        {
            int newX = Players[playeridx].X;
            int newY = Players[playeridx].Y;

            switch (key.Key)
            {
                case ConsoleKey.W:
                    newY--;
                    break;
                case ConsoleKey.S:
                    newY++;
                    break;
                case ConsoleKey.A:
                    newX--;
                    break;
                case ConsoleKey.D:
                    newX++;
                    break;
                default:
                    break;
            }

            if (newX < 0 || newX > Width - 1) return;
            if (newY < 0 || newY > Height - 1) return;
            if (!Board[newY][newX].IsPassable()) return;
            foreach (Monster monster in Monsters)
            {
                if (monster.X == newX && monster.Y == newY) return;
            }
            foreach (var group in Players)
            {
                if (group.Value == Players[playeridx]) continue;
                if (group.Value.X == newX && group.Value.Y == newY) return;
            }

            Players[playeridx].X = newX;
            Players[playeridx].Y = newY;
            Turn();
        }
        public void PickUpItem(int playeridx)
        {
            if (Players[playeridx].MaxInventoryCount == Players[playeridx].Inventory.Count) return;
            if (Board[Players[playeridx].Y][Players[playeridx].X].GetItem() == null) return;
            Players[playeridx].PickupItem(Board[Players[playeridx].Y][Players[playeridx].X].GetItem()!);
            Board[Players[playeridx].Y][Players[playeridx].X].RemoveItem();
        }
        public void DropItem(int playeridx)
        {
            if (Players[playeridx].Inventory.Count > 0)
            {
                Board[Players[playeridx].Y][Players[playeridx].X].AddItem(Players[playeridx].Inventory[Players[playeridx].SelectedItemIndex]);
                Players[playeridx].RemoveItem();
            }
        }
        public void DropAll(int playeridx)
        {
            while (Players[playeridx].Inventory.Count > 0) DropItem(playeridx);
        }
        public void PlayerInventoryNavigate(ConsoleKeyInfo key, int playeridx)
        {
            Players[playeridx].NavigateInventory(key);
        }
        public void PlayerSetHand(ConsoleKeyInfo key, int playeridx)
        {
            Players[playeridx].SetHand(key);
        }
        public void PlayerDrink(int playeridx)
        {
            if (Players[playeridx].Inventory.Count == 0) return;
            if (!Players[playeridx].Inventory[Players[playeridx].SelectedItemIndex].IsUsable()) return;
            Players[playeridx].Inventory[Players[playeridx].SelectedItemIndex].Use(Players[playeridx].Effects, Players[playeridx].Stats);
            Players[playeridx].RemoveItem();
        }
        public void Turn()
        {
            foreach (var group in Players)
            {
                group.Value.Turn();
            }
        }
        public void Fight(ConsoleKeyInfo key, int playeridx)
        {
            bool b = false;
            int idx = 0;
            while (idx < Monsters.Count)
            {
                if (Monsters[idx].X == Players[playeridx].X && Monsters[idx].Y == Players[playeridx].Y - 1)
                {
                    b = true;
                    break;
                }
                if (Monsters[idx].X == Players[playeridx].X && Monsters[idx].Y == Players[playeridx].Y + 1)
                {
                    b = true;
                    break;
                }
                if (Monsters[idx].X == Players[playeridx].X - 1 && Monsters[idx].Y == Players[playeridx].Y)
                {
                    b = true;
                    break;
                }
                if (Monsters[idx].X == Players[playeridx].X + 1 && Monsters[idx].Y == Players[playeridx].Y)
                {
                    b = true;
                    break;
                }
                idx++;
            }

            if (!b) return;

            int playerdamage = 0;
            int monsterdamage = Monsters[idx].Stats.Damage;
            switch (key.Key)
            {
                case ConsoleKey.M:
                    playerdamage = Players[playeridx].Damage(MagicAttack);
                    Players[playeridx].ResolveFight(monsterdamage, MagicDefense);
                    break;
                case ConsoleKey.N:
                    playerdamage = Players[playeridx].Damage(NormalAttack);
                    Players[playeridx].ResolveFight(monsterdamage, NormalDefense);
                    break;
                case ConsoleKey.B:
                    playerdamage = Players[playeridx].Damage(StealthAttack);
                    Players[playeridx].ResolveFight(monsterdamage, StealthDefense);
                    break;
                default:
                    break;
            }
            Monsters[idx].ResolveFight(playerdamage);

            if (Monsters[idx].Stats.Health <= 0)
            {
                Monsters.RemoveAt(idx);
            }
            if (Players[playeridx].Stats.Health <= 0) Players[playeridx].Stats.Health = 0;
        }
        public Monster? MonsterAttack(int playeridx)
        {
            bool b = false;
            int idx = 0;
            while (idx < Monsters.Count)
            {
                if (Monsters[idx].X == Players[playeridx].X && Monsters[idx].Y == Players[playeridx].Y - 1)
                {
                    b = true;
                    break;
                }
                if (Monsters[idx].X == Players[playeridx].X && Monsters[idx].Y == Players[playeridx].Y + 1)
                {
                    b = true;
                    break;
                }
                if (Monsters[idx].X == Players[playeridx].X - 1 && Monsters[idx].Y == Players[playeridx].Y)
                {
                    b = true;
                    break;
                }
                if (Monsters[idx].X == Players[playeridx].X + 1 && Monsters[idx].Y == Players[playeridx].Y)
                {
                    b = true;
                    break;
                }
                idx++;
            }

            if (!b) return null;

            int playerdamage = 0;
            int monsterdamage = Monsters[idx].Stats.Damage;

            int def1 = Players[playeridx].Defense(NormalDefense);
            int def2 = Players[playeridx].Defense(StealthDefense);
            int def3 = Players[playeridx].Defense(MagicDefense);

            int min = Math.Min(Math.Min(def1, def2), def3);
            if (min == def1)
            {
                playerdamage = Players[playeridx].Damage(NormalAttack);
                Players[playeridx].ResolveFight(monsterdamage, NormalDefense);
            }
            else if (min == def2)
            {
                playerdamage = Players[playeridx].Damage(StealthAttack);
                Players[playeridx].ResolveFight(monsterdamage, StealthDefense);
            }
            else if (min == def3)
            {
                playerdamage = Players[playeridx].Damage(MagicAttack);
                Players[playeridx].ResolveFight(monsterdamage, MagicDefense);
            }
            
            Monsters[idx].ResolveFight(playerdamage);
            if (Players[playeridx].Stats.Health <= 0) Players[playeridx].Stats.Health = 0;
            if (Monsters[idx].Stats.Health <= 0)
            {
                return Monsters[idx];
            }
            return null;
        }
        public void MoveMonsters()
        {
            List<Monster> dead = [];
            foreach (var monster in Monsters)
            {
                monster.Behaviour.Execute(monster, this);
                monster.ChangeBehaviour();
                Player? nearest = Players.Values.OrderBy(p => Math.Abs(p.X - monster.X) + Math.Abs(p.Y - monster.Y)).FirstOrDefault();
                if (nearest != null)
                {
                    int id = nearest.Symbol - '0';
                    Monster? d = MonsterAttack(id);
                    if (d != null) dead.Add(d);
                }
            }
            foreach(Monster d in dead)
            {
                Monsters.Remove(d);
            }
        }
    }
}