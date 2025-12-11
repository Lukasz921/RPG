using RPG.Decorators;
using RPG.Items;
using RPG.Monsters;
using RPG.Players;
using RPG.Tiles;

namespace RPG.Builders
{
    internal class MapBuilder(int height, int width) : IBuilder
    {
        public int Height { get; } = height;
        public int Width { get; } = width;
        public int CentralRoomHeight { get; } = height / 5;
        public int CentralRoomWidth { get; } = width / 5;
        public int RoomMinSize { get; } = 3;
        public int RoomMaxSize { get; } = 6;
        public int RoomSize { get; set; }
        public int Sigma { get; set; }
        public ITile[][] Board { get; set; } = CreateBoard(width, height);
        public List<Monster> Monsters { get; set; } = [];
        public static ITile[][] CreateBoard(int width, int height)
        {
            var board = new ITile[height][];
            for (int i = 0; i < height; i++)
            {
                board[i] = new ITile[width];
            }
            return board;
        }
        public void NullDungeon()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Board[i][j] = new ItemTile(item: null);
                }
            }
        }
        public void WallsDungeon()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Board[i][j] = new WallTile();
                }
            }
        }
        public void AddRandomPaths()
        {
            var random = new Random();

            void DFS(int x, int y)
            {
                Board[y][x] = new ItemTile(item: null);
                var directions = new (int, int)[]
                {
                    (-1, 0),
                    (1, 0),
                    (0, -1),
                    (0, 1)
                };
                directions = [.. directions.OrderBy(d => random.Next())];
                foreach (var (dx, dy) in directions)
                {
                    int newX = x + dx * 2;
                    int newY = y + dy * 2;
                    if (newX >= 1 && newY >= 1 && newX < Width - 1 && newY < Height - 1 && !Board[newY][newX].IsPassable())
                    {
                        Board[newY][newX] = new ItemTile(item: null);
                        Board[y + dy][x + dx] = new ItemTile(item: null);
                        DFS(newX, newY);
                    }
                }
            }

            int startX = 1;
            int startY = 1;

            DFS(startX, startY);
        }
        public void AddBorders()
        {
            for (int i = 0; i < Width; i++)
            {
                Board[0][i] = new WallTile();
                Board[Height - 1][i] = new WallTile();
            }

            for (int i = 0; i < Height; i++)
            {
                Board[i][0] = new WallTile();
                Board[i][Width - 1] = new WallTile();
            }
        }
        public void AddCentralRoom()
        {
            int roomX = (Width - CentralRoomWidth) / 2;
            int roomY = (Height - CentralRoomHeight) / 2;

            for (int i = roomY; i < roomY + CentralRoomHeight; i++)
            {
                for (int j = roomX; j < roomX + CentralRoomWidth; j++)
                {
                    Board[i][j] = new ItemTile(item: null);
                }
            }
        }
        public void AddRandomRooms()
        {
            RoomSize = new Random().Next(RoomMinSize, RoomMaxSize);
            Sigma = new Random().Next(1, 4);

            for (int i = 0; i < RoomSize; i++)
            {
                for (int j = 0; j < RoomSize; j++)
                {
                    Board[Sigma + i][Sigma + j] = new ItemTile(item: null);
                }
            }

            for (int i = 0; i < RoomSize; i++)
            {
                for (int j = 0; j < RoomSize; j++)
                {
                    Board[Sigma + i][Width - 1 - Sigma - j] = new ItemTile(item: null);
                }
            }

            for (int i = 0; i < RoomSize; i++)
            {
                for (int j = 0; j < RoomSize; j++)
                {
                    Board[Height - 1 - Sigma - i][Sigma + j] = new ItemTile(item: null);
                }
            }

            for (int i = 0; i < RoomSize; i++)
            {
                for (int j = 0; j < RoomSize; j++)
                {
                    Board[Height - 1 - Sigma - i][Width - 1 - Sigma - j] = new ItemTile(item: null);
                }
            }
        }
        public void AddLightWeapons()
        {
            char c = '\u2694';
            List<IWeapon> LightWeapons = [];
            LightWeapons.Add(new LightWeapon("Dagger", c, 20, false));
            LightWeapons.Add(new LightWeapon("Axe", c, 25, false));
            LightWeapons.Add(new LightWeapon("Sword", c, 30, false));
            LightWeapons.Add(new LightWeapon("Spear", c, 35, false));

            var random = new Random();
            while (LightWeapons.Count > 0)
            {
                int x = random.Next(1, Width);
                int y = random.Next(1, Height);
                if (Board[y][x].IsPassable())
                {
                    Board[y][x] = new ItemTile(item: LightWeapons[^1]);
                    LightWeapons.RemoveAt(LightWeapons.Count - 1);
                }
            }
        }
        public void AddHeavyWeapons()
        {
            char c = '\u2694';
            List<IWeapon> HeavyWeapons = [];
            HeavyWeapons.Add(new HeavyWeapon("Big Knife", c, 30, false));
            HeavyWeapons.Add(new HeavyWeapon("Battle Axe", c, 35, false));
            HeavyWeapons.Add(new HeavyWeapon("Sword", c, 40, true));
            HeavyWeapons.Add(new HeavyWeapon("Spear of Destiny", c, 50, true));

            var random = new Random();
            while (HeavyWeapons.Count > 0)
            {
                int x = random.Next(1, Width);
                int y = random.Next(1, Height);
                if (Board[y][x].IsPassable())
                {
                    Board[y][x] = new ItemTile(item: HeavyWeapons[^1]);
                    HeavyWeapons.RemoveAt(HeavyWeapons.Count - 1);
                }
            }
        }
        public void AddMagicWeapons()
        {
            char c = '\u2694';
            List<IWeapon> MagicWeapons = [];
            MagicWeapons.Add(new MagicWeapon("Air Element", c, 50, true));
            MagicWeapons.Add(new MagicWeapon("Earth Element", c, 60, true));
            MagicWeapons.Add(new MagicWeapon("Water Element", c, 70, true));
            MagicWeapons.Add(new MagicWeapon("Fire Element", c, 80, true));

            var random = new Random();
            while (MagicWeapons.Count > 0)
            {
                int x = random.Next(1, Width);
                int y = random.Next(1, Height);
                if (Board[y][x].IsPassable())
                {
                    Board[y][x] = new ItemTile(item: MagicWeapons[^1]);
                    MagicWeapons.RemoveAt(MagicWeapons.Count - 1);
                }
            }
        }
        public void AddDecoratedWeapons()
        {
            char c = '\u2694';
            List<IWeapon> DecoratedWeapons = [];

            LightWeapon knife = new("Knife", c, 20, false);
            HeavyWeapon spear = new("Spear", c, 40, true);
            MagicWeapon stone = new("Stone", c, 50, true);

            IWeapon w1 = new CursedEffect(knife);
            IWeapon w2 = new CursedEffect(spear);
            IWeapon w3 = new CursedEffect(stone);
            DecoratedWeapons.Add(w1);
            DecoratedWeapons.Add(w2);
            DecoratedWeapons.Add(w3);

            w1 = new StrongEffect(knife);
            w2 = new StrongEffect(spear);
            w3 = new StrongEffect(stone);
            DecoratedWeapons.Add(w1);
            DecoratedWeapons.Add(w2);
            DecoratedWeapons.Add(w3);

            w1 = new WeakEffect(knife);
            w2 = new WeakEffect(spear);
            w3 = new WeakEffect(stone);
            DecoratedWeapons.Add(w1);
            DecoratedWeapons.Add(w2);
            DecoratedWeapons.Add(w3);

            var random = new Random();
            while (DecoratedWeapons.Count > 0)
            {
                int x = random.Next(1, Width);
                int y = random.Next(1, Height);
                if (Board[y][x].IsPassable())
                {
                    Board[y][x] = new ItemTile(item: DecoratedWeapons[^1]);
                    DecoratedWeapons.RemoveAt(DecoratedWeapons.Count - 1);
                }
            }
        }
        public void AddCurrency()
        {
            char c = '\u2623';
            List<ICurrency> Currencies = [];
            for (int i = 0; i < 3; i++)
            {
                Currencies.Add(new Currency("Bronze", c, 0, false, 10));
                Currencies.Add(new Currency("Silver", c, 0, false, 20));
                Currencies.Add(new Currency("Gold", c, 0, false, 50));
            }

            var random = new Random();
            while (Currencies.Count > 0)
            {
                int x = random.Next(1, Width);
                int y = random.Next(1, Height);
                if (Board[y][x].IsPassable())
                {
                    Board[y][x] = new ItemTile(item: Currencies[^1]);
                    Currencies.RemoveAt(Currencies.Count - 1);
                }
            }
        }
        public void AddUnusable()
        {
            char c = '\u00A4';
            List<IUnusable> Unusables = [];
            Unusables.Add(new Unusable("Bucket", c, 0, false));
            Unusables.Add(new Unusable("Stone", c, 0, false));
            Unusables.Add(new Unusable("Broken Key", c, 0, false));

            var random = new Random();
            while (Unusables.Count > 0)
            {
                int x = random.Next(1, Width);
                int y = random.Next(1, Height);
                if (Board[y][x].IsPassable())
                {
                    Board[y][x] = new ItemTile(item: Unusables[^1]);
                    Unusables.RemoveAt(Unusables.Count - 1);
                }
            }
        }
        public void AddPotions()
        {
            char c = '\u2697';
            List<IPotion> Potions = [];
            Potions.Add(new Potion("Lucky Potion", c, 0, false, new PlayerEffect("Lucky Potion", 25, 0, 0, 0, 25, 0, 25)));
            Potions.Add(new Potion("Strength Potion", c, 0, false, new PlayerEffect("Strength Potion", 25, 15, 0, 10, 0, 10, 0)));
            Potions.Add(new Potion("Healing Potion", c, 0, false, new PlayerEffect("Healing Potion", 25, 0, 25, 50, 0, 0, 0)));
            Potions.Add(new Potion("Mana Potion", c, 0, false, new PlayerEffect("Mana Potion", - 1, 5, 5, 5, 5, 5, 5)));

            var random = new Random();
            while (Potions.Count > 0)
            {
                int x = random.Next(1, Width);
                int y = random.Next(1, Height);
                if (Board[y][x].IsPassable())
                {
                    Board[y][x] = new ItemTile(item: Potions[^1]);
                    Potions.RemoveAt(Potions.Count - 1);
                }
            }
        }
        public void AddMonsters()
        {
            char c = '\u2620';

            AggressiveBehaviour agg1 = new();
            AggressiveBehaviour agg2 = new();
            AggressiveBehaviour agg3 = new();
            AggressiveBehaviour agg4 = new();
            AggressiveBehaviour agg5 = new();
            AggressiveBehaviour agg6 = new();
            AggressiveBehaviour agg7 = new();
            AggressiveBehaviour agg8 = new();

            MonsterStats s1 = new(10, 20, 10);
            MonsterStats s2 = new(15, 30, 15);
            MonsterStats s3 = new(20, 40, 20);
            MonsterStats s4 = new(25, 50, 25);

            Monster m1 = new(-1, -1, "Goblin", c, s1, agg1);
            Monster m2 = new(-1, -1, "Goblin", c, s1, agg2);
            Monster m3 = new(-1, -1, "Troll", c, s2, agg3);
            Monster m4 = new(-1, -1, "Troll", c, s2, agg4);
            Monster m5 = new(-1, -1, "Warewolf", c, s3, agg5);
            Monster m6 = new(-1, -1, "Warewolf", c, s3, agg6);
            Monster m7 = new(-1, -1, "Ghost", c, s4, agg7);
            Monster m8 = new(-1, -1, "Ghost", c, s4, agg8);

            Monsters.Add(m1);
            Monsters.Add(m2);
            Monsters.Add(m3);
            Monsters.Add(m4);
            Monsters.Add(m5);
            Monsters.Add(m6);
            Monsters.Add(m7);
            Monsters.Add(m8);

            var random = new Random();
            int j = 0;
            while (j < 8)
            {
                int x = random.Next(1, Width);
                int y = random.Next(1, Height);
                bool b = true;
                if (!Board[y][x].IsPassable()) b = false;
                else
                {
                    foreach (Monster monster in Monsters)
                    {
                        if (monster.X == x && monster.Y == y) b = false;
                    }
                }
                if (b)
                {
                    Monsters[j].X = x;
                    Monsters[j].Y = y;
                    j++;
                }
            }
        }
    }
}