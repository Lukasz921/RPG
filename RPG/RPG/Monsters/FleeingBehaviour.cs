using RPG.Maps;
using RPG.Players;
using System.Text.Json.Serialization;

namespace RPG.Monsters
{
    internal class FleeingBehaviour : IBehaviour
    {
        [JsonConstructor]
        public FleeingBehaviour() { }
        public void Execute(Monster monster, Map map)
        {
            Player? nearest = map.Players.Values.OrderBy(p => Math.Abs(p.X - monster.X) + Math.Abs(p.Y - monster.Y)).FirstOrDefault();
            if (nearest == null) return;

            List<(int, int)> directions = [];
            directions.Add((monster.X - 1, monster.Y));
            directions.Add((monster.X + 1, monster.Y));
            directions.Add((monster.X, monster.Y + 1));
            directions.Add((monster.X, monster.Y - 1));

            int dx = nearest.X - monster.X;
            int dy = nearest.Y - monster.Y;

            if (dx == 0)
            {
                if (dy > 0) directions.RemoveAt(2);
                else directions.RemoveAt(3);
            }
            else if (dy == 0)
            {
                if (dx > 0) directions.RemoveAt(1);
                else directions.RemoveAt(0);
            }
            else
            {
                if (dy > 0) directions.RemoveAt(2);
                else directions.RemoveAt(3);
                if (dx > 0) directions.RemoveAt(1);
                else directions.RemoveAt(0);
            }

            foreach(var direction in directions)
            {
                if (IsValidMove(direction.Item1, direction.Item2, monster, map))
                {
                    monster.X = direction.Item1;
                    monster.Y = direction.Item2;
                    break;
                }
            }
        }
        private static bool IsValidMove(int x, int y, Monster monster, Map map)
        {
            if (x < 0 || x >= map.Width || y < 0 || y >= map.Height) return false;
            if (!map.Board[y][x].IsPassable()) return false;
            if (map.Monsters.Any(m => m != monster && m.X == x && m.Y == y)) return false;
            if (map.Players.Values.Any(p => p.X == x && p.Y == y)) return false;
            return true;
        }
    }
}