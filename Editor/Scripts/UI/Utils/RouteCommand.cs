using System.Data;
using CommandType = Unity.Muse.Chat.ChatCommandType;

namespace Unity.Muse.Chat
{
    public readonly struct RouteCommand
    {
        public readonly string Route;
        public readonly string Label;
        public readonly string Description;
        public readonly string PopupLabel;
        public readonly CommandType Type;

        public RouteCommand(string route, string label, string popupLabel, string description, CommandType type)
        {
            Route = route;
            Label = label;
            PopupLabel = popupLabel;
            Description = description;
            Type = type;
        }
    }
}
