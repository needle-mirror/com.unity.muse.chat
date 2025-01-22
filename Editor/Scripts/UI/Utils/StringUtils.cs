using System.Collections.Generic;
using System.Linq;

namespace Unity.Muse.Chat
{
    static class StringUtils
    {
        public static bool StartsWithAnyLinq(string input, IEnumerable<string> prefixes)
        {
            // Using LINQ
            return prefixes.Any(prefix => prefix.StartsWith(input));
        }
    }
}
