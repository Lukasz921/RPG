using RPG.Builders;
using RPG.Controlers;
using RPG.Displays;
using RPG.Maps;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace RPG.TCP
{
    internal class Client
    {
        public Map? Map { get; set; }
        public int ClientPlayerID { get; set; } = -1;
        public static JsonSerializerOptions JsonOptions { get; set; } = new()
        {
            IncludeFields = true
        };
        public void RunClient()
        {
            Console.Write("Ip:");
            string ip = Console.ReadLine()!;

            TcpClient client = new(ip, 5555);
            NetworkStream stream = client.GetStream();

            ClientChainBuilder clientchain = new();
            Director.BuildClassic(clientchain);
            ClientController clientcontroller = new() { Chains = clientchain.Chains };
            clientcontroller.Connect();

            ManualBuilder man = new();
            Director.BuildClassic(man);

            var receiveThread = new Thread(() => ReceiveLoop(stream, man, clientcontroller)) { IsBackground = true };
            receiveThread.Start();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                var action = new PlayerAction
                {
                    PlayerID = ClientPlayerID,
                    Key = keyInfo.Key
                };
                var msg = new MessageFromClient
                { 
                    Action = action 
                };
                string json = JsonSerializer.Serialize(msg, JsonOptions);
                var w = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                w.WriteLine(json);

                if (keyInfo.Key == ConsoleKey.Escape) Environment.Exit(0);
            }
        }
        public void ReceiveLoop(NetworkStream stream, ManualBuilder man, ClientController clientcontroller)
        {
            int turn = 0;
            using StreamReader reader = new(stream, Encoding.UTF8);

            while (true)
            {
                string line = reader.ReadLine()!;
                if (string.IsNullOrWhiteSpace(line)) continue;
                line = line.TrimStart('\uFEFF');
                MessageFromServer update = JsonSerializer.Deserialize<MessageFromServer>(line, JsonOptions)!;

                if (update.Action.Key == ConsoleKey.Enter && ClientPlayerID == -1) ClientPlayerID = update.Action.PlayerID;

                if (ClientPlayerID != -1 && update.Map.Players[ClientPlayerID].Stats.Health == 0)
                {
                    DeathMessage(stream);
                    Environment.Exit(0);
                }

                var info = new ConsoleKeyInfo(
                    keyChar: '\0',
                    key: update.Action.Key,
                    shift: false,
                    alt: false,
                    control: false
                );

                Map oldmap = Map!;
                Map = update.Map;

                lock (Map)
                {
                    if (turn == 0)
                    {
                        Display.InitialDrawClient(Map, man, ClientPlayerID);
                        turn++;
                    }
                    else if (ClientPlayerID == update.Action.PlayerID)
                    {
                        ThisPlayerAction(update, oldmap);
                        clientcontroller.Chains[0].ProcessKey(info, Map, ClientPlayerID);
                    }
                    else ElsePlayerAction();
                }
            }
        }
        public void ThisPlayerAction(MessageFromServer update, Map oldmap)
        {
            switch (update.Action.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.S:
                case ConsoleKey.A:
                case ConsoleKey.D:
                    Display.RefreshMap(Map!);
                    break;
                case ConsoleKey.M:
                case ConsoleKey.N:
                case ConsoleKey.B:
                    Display.RefreshMonsters(oldmap, Map!);
                    break;

            }
        }
        public void ElsePlayerAction()
        {
            Display.RefreshMap(Map!);
            Display.DrawPlayerStats(Map!.Players[ClientPlayerID].Stats);
            Display.DrawPotions(Map.Players[ClientPlayerID].Effects);
            Display.DrawMonster(Map, Map.Players[ClientPlayerID]);
        }
        public void DeathMessage(NetworkStream stream)
        {
            var action = new PlayerAction
            {
                PlayerID = ClientPlayerID,
                Key = ConsoleKey.Escape
            };
            var msg = new MessageFromClient
            {
                Action = action
            };
            string json = JsonSerializer.Serialize(msg, JsonOptions);
            var w = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            w.WriteLine(json);
        }
    }
}