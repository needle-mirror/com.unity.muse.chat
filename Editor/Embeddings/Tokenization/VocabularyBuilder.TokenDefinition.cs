using System;
using System.Collections.Generic;
using System.Linq;

namespace Unity.Muse.Chat.Tokenization
{
    partial class VocabularyBuilder
    {
        /// <summary>
        ///     Internal implementation of the <see cref="ITokenDefinition" /> for the
        ///     <see cref="Vocabulary" /> type.
        /// </summary>
        class TokenDefinition : ITokenDefinition
        {
            /// <summary>
            ///     The ids of the token.
            /// </summary>
            public readonly IReadOnlyCollection<int> Ids;

            /// <inheritdoc cref="ITokenDefinition.IsSpecial" />
            /// .
            public readonly bool IsSpecial;

            /// <summary>
            ///     The <see cref="string" /> representation of the token.
            /// </summary>
            public readonly string Value;

            /// <summary>
            ///     Initializes a new instance of the <see cref="TokenDefinition" /> type.
            /// </summary>
            /// <param name="value">
            ///     The <see cref="string" /> representation of the token.
            /// </param>
            /// <param name="ids">
            ///     The ids of the token.
            /// </param>
            /// <param name="isSpecial">
            ///     Tells whether this token is special.
            ///     See <see cref="IsSpecial" />.
            /// </param>
            public TokenDefinition(string value, IReadOnlyCollection<int> ids, bool isSpecial)
            {
                Value = value;
                Ids = ids;
                IsSpecial = isSpecial;
            }

            string ITokenDefinition.Value => Value;

            IReadOnlyCollection<int> ITokenDefinition.Ids => Ids;

            bool ITokenDefinition.IsSpecial => IsSpecial;

            bool IEquatable<ITokenDefinition>.Equals(ITokenDefinition other)
            {
                return Equals(other);
            }

            int IComparable<ITokenDefinition>.CompareTo(ITokenDefinition other)
            {
                return CompareTo(other);
            }

            /// <inheritdoc cref="IEquatable{T}" />
            public bool Equals(ITokenDefinition other)
            {
                return other != null
                       && (
                           Ids.SequenceEqual(other.Ids)
                           || (
                               Value.Equals(other.Value, StringComparison.InvariantCulture)
                               && IsSpecial == other.IsSpecial
                           )
                       );
            }

            public int CompareTo(ITokenDefinition other)
            {
                var valueComp = string.Compare(Value, other.Value, StringComparison.Ordinal);
                return valueComp == 0
                    ? IsSpecial.CompareTo(other.IsSpecial)
                    : valueComp;
            }

            public override bool Equals(object obj)
            {
                return obj is ITokenDefinition other && Equals(other);
            }

            public override int GetHashCode()
            {
                var hashCode = Ids.Aggregate(0, (current, id) => current ^ id);
                return hashCode;
            }

            public override string ToString()
            {
                return $"{(IsSpecial ? @"★" : "")}{Value}:{Ids}";
            }
        }
    }
}
