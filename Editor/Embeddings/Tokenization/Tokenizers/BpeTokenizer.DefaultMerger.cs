using System.Collections.Generic;

namespace Unity.Muse.Chat.Tokenization.Tokenizers
{
    partial class BpeTokenizer
    {
        /// <summary>
        ///     The merger type used when no merge rules are given to the <see cref="BpeTokenizer" />
        ///     constructor.
        ///     It is a typical passthrough which doesn't modify the input sequence of tokens.
        /// </summary>
        internal class DefaultMerger : IConverter<IEnumerable<ITokenDefinition>,
            IEnumerable<ITokenDefinition>>
        {
            /// <inheritdoc />
            public IEnumerable<ITokenDefinition> Convert(IEnumerable<ITokenDefinition> definitions)
            {
                return definitions;
            }
        }
    }
}
