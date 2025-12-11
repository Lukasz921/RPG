using RPG.TCP;

namespace RPG.Displays
{
    internal class ServerDisplay
    {
        private ServerDisplay() { }
        private static ServerDisplay? Instance;
        public static ServerDisplay GetInstance()
        {
            Instance ??= new ServerDisplay();
            return Instance;
        }
        public static void Run(int port)
        {
            Console.WriteLine($"Server started at port {port}, waiting for players…");
        }
        public static void Connected(int playerId)
        {
            Console.WriteLine($"Player {playerId} connected.");
        }
        public static void Received(MessageFromClient action)
        {
            Console.WriteLine($"Received action from player: {action.Action.PlayerID}: {action.Action.Key}");
        }
        public static void End(int playerId)
        {
            Console.WriteLine($"Player: {playerId} ends.");
        }
    }
}