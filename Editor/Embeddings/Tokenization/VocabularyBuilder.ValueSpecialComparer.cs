using System;
using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization
{
    partial class VocabularyBuilder
    {
        class ValueSpecialComparer : IEqualityComparer<TokenDefinition>
        {
            public bool Equals(TokenDefinition x, TokenDefinition y)
            {
                return x!.Value == y!.Value
                       && x.IsSpecial == y.IsSpecial;
            }

            public int GetHashCode(TokenDefinition obj)
            {
                return HashCode.Combine(obj.Value.GetHashCode(), obj.IsSpecial.GetHashCode());
            }
        }
    }
}
