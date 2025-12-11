using RPG.Builders;
using RPG.Displays;
using RPG.Maps;
using RPG.TCP;

namespace RPG
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            int height = Display.Height;
            int width = Display.Width;

            Console.WriteLine("Start as (S)erver or as (C)lient?");
            ConsoleKeyInfo startkey;
            do { startkey = Console.ReadKey(true); } while (startkey.Key != ConsoleKey.C && startkey.Key != ConsoleKey.S);
            Console.Clear();

            MapBuilder builder = new(height, width);
            Director.BuildClassic(builder);

            Map servermap = new(height, width)
            {
                Board = builder.Board,
                Monsters = builder.Monsters
            };

            Server server = new()
            { 
                Map = servermap 
            };

            Client client = new();

            if (startkey.Key == ConsoleKey.S) server.RunServer();
            else client.RunClient();
        }
    }
}