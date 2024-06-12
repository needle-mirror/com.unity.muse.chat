using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization
{
    interface IVocabulary
    {
        /// <summary>
        ///     Tries to find a <see cref="ITokenDefinition" /> from its
        ///     <see cref="ITokenDefinition.Value" />.
        /// </summary>
        /// <param name="value">
        ///     The string representation of the <see cref="ITokenDefinition" /> instance to look
        ///     for.
        /// </param>
        /// <param name="definition">
        ///     The token definition, if found.
        /// </param>
        /// <param name="special">
        ///     Whether the requested token is special.
        /// </param>
        /// <param name="prefix">
        ///     Token prefix.
        /// </param>
        /// <returns>
        ///     Whether a <see cref="ITokenDefinition" /> instance has been found.
        /// </returns>
        bool TryGetToken(
            SubString value,
            out ITokenDefinition definition,
            bool special = false,
            SubString? prefix = null);

        /// <summary>
        ///     Tries to find a <see cref="ITokenDefinition" /> from its
        ///     <see cref="ITokenDefinition.Ids" />.
        /// </summary>
        /// <param name="ids">
        ///     The sequence of ids of the <see cref="ITokenDefinition" /> instance to look for.
        /// </param>
        /// <param name="definition">
        ///     The token definition, if found.
        /// </param>
        /// <returns>
        ///     Whether a <see cref="ITokenDefinition" /> instance has been found.
        /// </returns>
        bool TryGetToken(IEnumerable<int> ids, out ITokenDefinition definition);
    }
}
