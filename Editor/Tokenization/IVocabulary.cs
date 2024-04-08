using System.Collections.Generic;

namespace Unity.Muse.Chat.Tokenization
{
    interface IVocabulary
    {
        /// <summary>
        ///     Tries to find the token with the longest value matching with the given
        ///     <paramref name="input" />.
        /// </summary>
        /// <param name="input">
        ///     The source to search a matching token into.
        /// </param>
        /// <param name="output">
        ///     The result of the lookup.
        ///     <ul>
        ///         <li><c>definition</c>: The <see cref="ITokenDefinition" /> found.</li>
        ///         <li>
        ///             <c>length</c>: The length of the portion of source where the token was
        ///             found.
        ///         </li>
        ///     </ul>
        /// </param>
        /// <param name="prefix">
        ///     The lookup of the <paramref name="input" /> might require a token prefix to be
        ///     considered (typically if the <paramref name="input" /> starts in the middle of a
        ///     word).
        /// </param>
        /// <returns>
        ///     Whether a <see cref="ITokenDefinition" /> instance has been found.
        /// </returns>
        bool Find(
            SubString input,
            out (ITokenDefinition definition, int length) output,
            string prefix = null);

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
        /// <returns>
        ///     Whether a <see cref="ITokenDefinition" /> instance has been found.
        /// </returns>
        bool TryGetToken(string value, out ITokenDefinition definition, bool special = false);

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
