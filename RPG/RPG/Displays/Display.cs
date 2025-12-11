using RPG.Attacks;
using RPG.Defenses;
using RPG.Builders;
using RPG.Items;
using RPG.Maps;
using RPG.Monsters;
using RPG.Players;
using RPG.Tiles;

namespace RPG.Displays
{
    internal sealed class Display
    {
        private Display() { }
        private static Display? Instance;
        public static Display GetInstance()
        {
            Instance ??= new Display();
            return Instance;
        }
        public static int Height { get; set; } = 17;
        public static int Width { get; set; } = 41;
        public static int StandardWidth { get; set; } = 50;
        public static int StatsH { get; set; } = 0;
        public static int StatsW { get; set; } = Width;
        public static int CurrencyH { get; set; } = 3;
        public static int CurrencyW { get; set; } = Width;
        public static int HandsH { get; set; } = 6;
        public static int HandsW { get; set; } = Width;
        public static int InventoryH { get; set; } = 10;
        public static int InventoryW { get; set; } = Width;
        public static int ManualH { get; set; } = InventoryH + 13;
        public static int ManualW { get; set; } = Width;
        public static int MonsterH { get; set; } = Height;
        public static int MonsterW {  get; set; } = 0;
        public static int MoveH { get; set; } = Height + 6;
        public static int MoveW {  get; set; } = 0;
        public static int AttackH { get; set; } = MoveH + 5;
        public static int AttackW { get; set; } = 0;
        public static int PotionsH { get; set; } = AttackH + 5;
        public static int PotionsW { get; set; } = 0;
        public static void InitialDrawClient(Map map, ManualBuilder man, int ClientPlayerID)
        {
            DrawAllWindows(map.Players[ClientPlayerID]);
            DrawAll(map, map.Players[ClientPlayerID], man.Manual);
        }
        public static void DrawBoard(Map map)
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < map.Height; i++)
            {
                for (int j = 0; j < map.Width; j++)
                {
                    Console.Write($"{map.Board[i][j].Symbol}");
                }
                Console.WriteLine();
            }
            foreach(Monster monster in map.Monsters)
            {
                Console.SetCursorPosition(monster.X, monster.Y);
                Console.Write($"{monster.Symbol}");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var group in map.Players)
            {
                Console.SetCursorPosition(group.Value.X, group.Value.Y);
                Console.Write($"{group.Value.Symbol}");
            }
            Console.ResetColor();
        }
        public static void RefreshMap(Map map)
        {
            Dictionary<(int, int), Monster> monsters = [];
            foreach(Monster monster in map.Monsters)
            {
                monsters.Add((monster.Y, monster.X), monster);
            }
            Dictionary<(int, int), Player> players = [];
            foreach(var group in map.Players)
            {
                players.Add((group.Value.Y, group.Value.X), group.Value);
            }

            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < map.Height; i++)
            {
                for (int j = 0; j < map.Width; j++)
                {
                    if (players.ContainsKey((i, j)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{players[(i, j)].Symbol}");
                        Console.ResetColor();
                    }
                    else if (monsters.ContainsKey((i, j)))
                    {
                        Console.Write($"{monsters[(i, j)].Symbol}");
                    }
                    else Console.Write($"{map.Board[i][j].Symbol}");
                }
                Console.WriteLine();
            }
        }
        public static void RefreshMonsters(Map oldmap, Map newmap)
        {
            foreach (Monster monster in oldmap.Monsters)
            {
                Console.SetCursorPosition(monster.X, monster.Y);
                Console.Write($"{monster.Symbol}");
            }
            foreach (Monster oldmonster in oldmap.Monsters)
            {
                bool b = true;
                foreach (Monster newmonster in newmap.Monsters)
                {
                    if (oldmonster.X == newmonster.X &&  newmonster.Y == newmonster.Y) { b = false; break; }
                }
                if (b)
                {
                    Console.SetCursorPosition(oldmonster.X, oldmonster.Y);
                    Console.Write($"{newmap.Board[oldmonster.Y][oldmonster.X].Symbol}");
                }
            }
        }
        public static void DrawPlayerMove(ITile[][] Board, Player player)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (Board[player.Y][player.X].GetItem() == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.SetCursorPosition(MoveW + 1, MoveH + 1 + i);
                    Console.Write(new string(' ', Width - 2));
                }
                return;
            }
            Console.SetCursorPosition(MoveW + 1, MoveH + 1);
            Console.Write("Item to pick (E)");
            Console.SetCursorPosition(MoveW + 1, MoveH + 2);
            string str = $"{Board[player.Y][player.X].GetItem()!.Name}";
            Console.Write(str + new string(' ', Width - 2 - str.Length));
            Console.SetCursorPosition(MoveW + 1, MoveH + 3);
            if (Board[player.Y][player.X].Items.Count > 1) Console.Write("Multiple items to pick");
            else Console.Write(new string(' ', Width - 2));
            Console.ResetColor();
        }
        public static void DrawPlayerStats(PlayerStats stats)
        {
            Console.SetCursorPosition(StatsW + 1, StatsH + 1);
            Console.ForegroundColor = ConsoleColor.Blue;
            string str = new($"Gracz: P:{stats.Power} A:{stats.Agility} H:{stats.Health} L:{stats.Luck} A:{stats.Aggression} W:{stats.Wisdom}");
            Console.Write(str + new string(' ', StandardWidth - 2 - str.Length));
            Console.ResetColor();
        }
        public static void DrawPlayerCurrency(PlayerStats stats)
        {
            Console.SetCursorPosition(CurrencyW + 1, CurrencyH + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            string str = new($"Ilość waluty: {stats.CurrencyCounter}");
            Console.Write(str + new string(' ', StandardWidth - 2 - str.Length));
            Console.ResetColor();
        }
        public static void DrawPlayerHands(Player player)
        {
            Console.SetCursorPosition(HandsW + 1, HandsH + 1);
            Console.ForegroundColor = ConsoleColor.Magenta;
            if (player.LeftHand == null)
            {
                string str = new("Lewa ręka jest pusta.");
                Console.Write(str + new string(' ', StandardWidth - 2 - str.Length));
            }
            else
            {
                string str = new($"Lewa ręka: {player.LeftHand.Name} (L)");
                if (str.Length > StandardWidth - 2)
                {
                    str = string.Concat(str.AsSpan(0, StandardWidth - 7), " ...");
                    Console.Write(str);
                }
                else Console.Write(str + new string(' ', StandardWidth - 2 - str.Length));
            }
            Console.SetCursorPosition(HandsW + 1, HandsH + 2);
            if (player.RightHand == null)
            {
                string str = new("Prawa ręka jest pusta.");
                Console.Write(str + new string(' ', StandardWidth - 2 - str.Length));
            }
            else
            {
                string str = new($"Prawa ręka: {player.RightHand.Name} (P)");
                if (str.Length > StandardWidth - 2)
                {
                    str = string.Concat(str.AsSpan(0, StandardWidth - 7), " ...");
                    Console.Write(str);
                }
                else Console.Write(str + new string(' ', StandardWidth - 2 - str.Length));
            }
            Console.ResetColor();
        }
        public static void DrawPlayerInventory(Player player)
        {
            Console.SetCursorPosition(InventoryW + 1, InventoryH + 1);
            Console.ForegroundColor = ConsoleColor.Cyan;
            int j = 0;
            if (player.Inventory.Count == 0)
            {
                string str = new("Ekwipunek jest pusty.");
                Console.Write(str + new string(' ', StandardWidth - 2 - str.Length));
            }
            else
            {
                string str = new("Ekwipunek: (G) (ArrowUp/ArrowDown)");
                Console.Write(str + new string(' ', StandardWidth - 2 - str.Length));
                Console.ResetColor();
                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    IItem item = player.Inventory[i];
                    Console.SetCursorPosition(InventoryW + 1, InventoryH + 2 + i);
                    if (i == player.SelectedItemIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(new string(' ', StandardWidth - 2));
                        Console.SetCursorPosition(InventoryW + 1, InventoryH + 2 + i);
                        string strink = new($"> {item.Name} (L/P) (Q)");
                        if (item.IsUsable())
                        {
                            strink += " (F)";
                        }
                        if (strink.Length > StandardWidth - 2)
                        {
                            strink = string.Concat(strink.AsSpan(0, StandardWidth - 7), " ...");
                        }
                        Console.WriteLine(strink);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(new string(' ', StandardWidth - 2));
                        Console.SetCursorPosition(InventoryW + 1, InventoryH + 2 + i);
                        string strink = new($"  {item.Name}");
                        if (strink.Length > StandardWidth - 2)
                        {
                            strink = string.Concat(strink.AsSpan(0, StandardWidth - 7), " ...");
                        }
                        Console.WriteLine(strink);
                    }
                }
                j = player.Inventory.Count;
            }
            for (int k = j + 1; k < player.MaxInventoryCount + 1; k++)
            {
                Console.SetCursorPosition(InventoryW + 1, InventoryH + 1 + k);
                Console.Write(new string(' ', StandardWidth - 2));
            }
            Console.ResetColor();
        }
        public static void DrawManual(List<string> manual)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            int idx = 0;
            foreach (string line in manual)
            {
                Console.SetCursorPosition(ManualW + 1, ManualH + 1 + idx++);
                Console.Write(line);
            }
            Console.ResetColor();
        }
        public static void DrawMonster(Map map, Player player)
        {
            bool b = false;
            int idx = 0;
            while (idx < map.Monsters.Count)
            {
                if (map.Monsters[idx].X == player.X && map.Monsters[idx].Y == player.Y - 1)
                {
                    b = true; 
                    break;
                }
                if (map.Monsters[idx].X == player.X && map.Monsters[idx].Y == player.Y + 1)
                {
                    b = true;
                    break;
                }
                if (map.Monsters[idx].X == player.X - 1 && map.Monsters[idx].Y == player.Y)
                {
                    b = true;
                    break;
                }
                if (map.Monsters[idx].X == player.X + 1 && map.Monsters[idx].Y == player.Y)
                {
                    b = true;
                    break;
                }
                idx++;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (b)
            {
                string str = $"Monster: {map.Monsters[idx].Name}";
                Console.SetCursorPosition(MonsterW + 1, MonsterH + 1);
                Console.Write(str + new string(' ', Width - 2 - str.Length));
                str = $"Attack: {map.Monsters[idx].Stats.Damage}";
                Console.SetCursorPosition(MonsterW + 1, MonsterH + 2);
                Console.Write(str + new string(' ', Width - 2 - str.Length));
                str = $"Health: {map.Monsters[idx].Stats.Health}";
                Console.SetCursorPosition(MonsterW + 1, MonsterH + 3);
                Console.Write(str + new string(' ', Width - 2 - str.Length));
                str = $"Defense: {map.Monsters[idx].Stats.Defense}";
                Console.SetCursorPosition(MonsterW + 1, MonsterH + 4);
                Console.Write(str + new string(' ', Width - 2 - str.Length));
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.SetCursorPosition(MonsterW + 1, MonsterH + 1 + i);
                    Console.WriteLine(new string(' ', Width - 2));
                }
            }
            Console.ResetColor();
        }
        public static void DrawAttacks(Player player)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(AttackW + 1, AttackH + 1);
            string str = $"Normal attack (N): ({player.Damage(new NormalAttack())}) ({player.Defense(new NormalDefense())})";
            Console.Write(str + new string(' ', Width - 2 - str.Length));
            Console.SetCursorPosition(AttackW + 1, AttackH + 2);
            str = $"Magic attack (M): ({player.Damage(new MagicAttack())}) ({player.Defense(new MagicDefense())})";
            Console.Write(str + new string(' ', Width - 2 - str.Length));
            Console.SetCursorPosition(AttackW + 1, AttackH + 3);
            str = $"Stealth attack (B): ({player.Damage(new StealthAttack())}) ({player.Defense(new StealthDefense())})";
            Console.Write(str + new string(' ', Width - 2 - str.Length));
            Console.ResetColor();
        }
        public static void DrawPotions(List<PlayerEffect> effects)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            int idx = 0;
            while (idx < effects.Count)
            {
                Console.SetCursorPosition(PotionsW + 1, PotionsH + 1 + idx);
                string str = $"{effects[idx].Name}:";
                if (effects[idx].LeftTurns != -1) str += $" {effects[idx].LeftTurns}";
                else str += " INF";
                Console.Write(str + new string(' ', Width - 2 - str.Length));
                idx++;
            }
            while (idx < 4)
            {
                Console.SetCursorPosition(PotionsW + 1, PotionsH + 1 + idx);
                Console.WriteLine(new string(' ', Width - 2));
                idx++;
            }
            Console.ResetColor();
        }
        public static void DrawGoodInput()
        {
            for (int i = 0; i < 13; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write('█');
            }
        }
        public static void DrawWrongInput()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("WrongInput!!!");
            Console.ResetColor();
        }
        public static void DrawWindow(int startH, int startW, int space, int width, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(startW, startH);
            for (int i = 0; i < width; i++)
            {
                Console.Write("-");
            }
            Console.SetCursorPosition(startW, startH + space + 1);
            for (int i = 0; i < width; i++)
            {
                Console.Write("-");
            }
            for (int i = 0; i < space; i++)
            {
                Console.SetCursorPosition(startW, startH + 1 + i);
                Console.Write("|");
                Console.SetCursorPosition(startW + width - 1, startH + 1 + i);
                Console.Write("|");
            }
            Console.SetCursorPosition(startW, startH);
            Console.Write("*");
            Console.SetCursorPosition(startW + width - 1, startH);
            Console.Write("*");
            Console.SetCursorPosition(startW, startH + space + 1);
            Console.Write("*");
            Console.SetCursorPosition(startW + width - 1, startH + space + 1);
            Console.Write("*");
            Console.ResetColor();
        }
        public static void DrawAll(Map map, Player player, List<string> manual)
        {
            DrawBoard(map);
            DrawPlayerStats(player.Stats);
            DrawPlayerCurrency(player.Stats);
            DrawPlayerHands(player);
            DrawPlayerInventory(player);
            //DrawManual(manual);
            DrawMonster(map, player);
            DrawPlayerMove(map.Board, player);
            DrawAttacks(player);
            DrawPotions(player.Effects);
        }
        public static void DrawAllWindows(Player player)
        {
            DrawWindow(StatsH, StatsW, 1, StandardWidth, ConsoleColor.Blue);
            DrawWindow(CurrencyH, CurrencyW, 1, StandardWidth, ConsoleColor.Red);
            DrawWindow(HandsH, HandsW, 2, StandardWidth, ConsoleColor.Magenta);
            DrawWindow(InventoryH, InventoryW, player.MaxInventoryCount + 1, StandardWidth, ConsoleColor.Cyan);
            //DrawWindow(ManualH, ManualW, 18, StandardWidth, ConsoleColor.Green);
            DrawWindow(MonsterH, MonsterW, 4, Width, ConsoleColor.DarkRed);
            DrawWindow(MoveH, MoveW, 3, Width, ConsoleColor.Yellow);
            DrawWindow(AttackH, AttackW, 3, Width, ConsoleColor.Blue);
            DrawWindow(PotionsH, PotionsW, 4, Width, ConsoleColor.DarkBlue);
        }
    }
}