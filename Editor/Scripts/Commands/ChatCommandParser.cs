using System;
using System.Text.RegularExpressions;

namespace Unity.Muse.Chat
{
    internal static class ChatCommandParser
    {
        private static readonly Regex s_CommandPattern = new Regex(@"^\/(\w+)\s+(.*)");

        internal static bool IsCommand(string text)
        {
            return text.StartsWith('/');
        }

        public static (ChatCommandType command, string arguments) Parse(string input)
        {
            var match = s_CommandPattern.Match(input);

            if (match.Success)
            {
                var cmdText = match.Groups[1].Value;
                var argumentText = match.Groups[2].Value;

                if (Enum.TryParse<ChatCommandType>(cmdText, true, out var cmdType))
                    return (cmdType, argumentText);
            }

            return (default, null);
        }
    }
}
