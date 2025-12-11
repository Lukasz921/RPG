using RPG.Maps;

namespace RPG.TCP
{
    internal class MessageFromServer
    {
        public required Map Map { get; set; }
        public required PlayerAction Action { get; set; }
    }
}