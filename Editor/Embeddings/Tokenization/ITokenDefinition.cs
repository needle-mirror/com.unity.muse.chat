using System;
using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization
{
    /// <summary>
    ///     Definition of a token, its <see cref="Value" /> and its <see cref="Ids" />.
    /// </summary>
    interface ITokenDefinition : IEquatable<ITokenDefinition>, IComparable<ITokenDefinition>
    {
        /// <summary>
        ///     The string representation of the token.
        /// </summary>
        string Value { get; }

        /// <summary>
        ///     The list of ids.
        /// </summary>
        IReadOnlyCollection<int> Ids { get; }

        /// <summary>
        ///     Tells whether this token is special.
        ///     Being special means it is probably used for structuring the sequence of tokens and
        ///     it shouldn't be part of the detokenized <see cref="string" /> value.
        /// </summary>
        bool IsSpecial { get; }
    }
}
