using System.Collections.Generic;
using System.Linq;

namespace Unity.Muse.Chat.Tokenization
{
    partial class VocabularyBuilder
    {
        class IdsComparer : IEqualityComparer<TokenDefinition>
        {
            public bool Equals(
                TokenDefinition x,
                TokenDefinition y)
            {
                return x!.Ids.SequenceEqual(y!.Ids);
            }

            public int GetHashCode(TokenDefinition obj)
            {
                return obj.Ids.Aggregate((current, id) => current ^ id);
            }
        }
    }
}
