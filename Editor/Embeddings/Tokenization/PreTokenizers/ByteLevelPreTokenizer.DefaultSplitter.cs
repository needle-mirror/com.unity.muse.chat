using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers
{
    partial class ByteLevelPreTokenizer
    {
        internal class DefaultSplitter : IConverter<SubString, IEnumerable<SubString>>
        {
            public IEnumerable<SubString> Convert(SubString input)
            {
                yield return input;
            }
        }
    }
}
