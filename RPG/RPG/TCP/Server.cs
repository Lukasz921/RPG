using RPG.Builders;
using RPG.Controlers;
using RPG.Displays;
using RPG.Maps;
using RPG.Players;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace RPG.TCP
{
    internal class Server
    {
        public required Map Map { get; set; }
        public static List<(int, NetworkStream)> Clients { get; set; } = [];
        public static List<bool> IdB {  get; set; } = [];
        public static object StreamsLock { get; set; } = new();
        public static JsonSerializerOptions JsonOptions { get; set; } = new()
        {
            IncludeFields = true
        };

        public void RunServer()
        {
            for (int i = 0; i < 9; i++) IdB.Add(false);

            ChainBuilder chain = new();
            Director.BuildClassic(chain);
            Controller controller = new()
            {
                Chains = chain.Chains
            };
            controller.Connect();

            int port = 5555;
            TcpListener server = new(IPAddress.Any, port);
            server.Start();
            ServerDisplay.Run(port);

            Thread m = new(() => MonsterMove(Map)) { IsBackground = true };
            m.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                int idx = 0;
                int playerId = -1;
                lock(IdB)
                {
                    while (IdB[idx]) idx++;
                    playerId = idx;
                    if (playerId <= 8) IdB[playerId] = true;
                }
                if (playerId > 8)
                {
                    client.Close();
                    continue;
                }

                ServerDisplay.Connected(playerId);
                NetworkStream stream = client.GetStream();

                lock (StreamsLock) Clients.Add((playerId, stream));

                Player newPlayer = new((char)('0' + playerId));
                Map.AddPlayer(newPlayer, playerId);

                Thread t = new(() => HandleClient(stream, Map, controller, playerId)) { IsBackground = true };
                t.Start();

                PlayerAction action = new()
                { Key = ConsoleKey.Enter, PlayerID = playerId };
                MessageFromServer msg;
                lock (Map) msg = new() { Map = Map, Action = action };
                string json = JsonSerializer.Serialize(msg, JsonOptions);
                lock (StreamsLock)
                {
                    try
                    {
                        var w = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                        w.WriteLine(json);
                    }
                    catch { }
                }

                PlayerAction playerAction = new()
                { Key = ConsoleKey.W, PlayerID = playerId };
                BroadcastUpdate(Map, playerAction);
            }
        }
        public static void HandleClient(NetworkStream stream, Map map, Controller controller, int playerID)
        {
            try
            {
                using StreamReader reader = new(stream, Encoding.UTF8);
                while (true)
                {
                    string line = reader.ReadLine()!;
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    line = line.TrimStart('\uFEFF');

                    MessageFromClient action = JsonSerializer.Deserialize<MessageFromClient>(line, JsonOptions)!;
                    ServerDisplay.Received(action);

                    var info = new ConsoleKeyInfo(
                        keyChar: '\0',
                        key: action.Action.Key,
                        shift: false,
                        alt: false,
                        control: false
                    );
                    lock(map) controller.Chains[0].ProcessKey(info, map, action.Action.PlayerID);
                    BroadcastUpdate(map, action.Action);
                }
            }
            catch (IOException) { }
            finally 
            {
                lock (StreamsLock) Clients.Remove((playerID, stream));
                lock (IdB) IdB[playerID] = false;
                map.RemovePlayer(playerID);
                PlayerAction playeraction = new() { Key = ConsoleKey.Enter, PlayerID = -1 };
                BroadcastUpdate(map, playeraction);
                stream.Close();
                if (stream is { CanRead: true } && stream.Socket is { } socket) socket.Close();
                ServerDisplay.End(playerID);
            }
        }
        public static void MonsterMove(Map map)
        {
            while (true)
            {
                Thread.Sleep(1000);
                lock (map) map.MoveMonsters();
                BroadcastUpdate(map);
            }
        }
        static void BroadcastUpdate(Map map)
        {
            lock (StreamsLock)
            {
                foreach (var group in Clients)
                {
                    try
                    {
                        PlayerAction action = new()
                        { Key = ConsoleKey.W, PlayerID = group.Item1 };

                        MessageFromServer msg;
                        lock (map) msg = new() { Map = map, Action = action };
                        string json = JsonSerializer.Serialize(msg, JsonOptions);

                        var w = new StreamWriter(group.Item2, Encoding.UTF8) { AutoFlush = true };
                        w.WriteLine(json);
                    }
                    catch { }
                }
            }
        }
        static void BroadcastUpdate(Map map, PlayerAction action)
        {
            MessageFromServer msg;
            lock (map) msg = new() { Map = map, Action = action };
            string json = JsonSerializer.Serialize(msg, JsonOptions);

            lock (StreamsLock)
            {
                foreach (var group in Clients)
                {
                    try
                    {
                        var w = new StreamWriter(group.Item2, Encoding.UTF8) { AutoFlush = true };
                        w.WriteLine(json);
                    }
                    catch { }
                }
            }
        }
    }
}