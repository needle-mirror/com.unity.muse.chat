using System;

namespace Unity.Muse.Chat.Tokenization.PostProcessors.Templating
{
    /// <summary>
    ///     Represents a special token in a <see cref="Template" />.
    /// </summary>
    class SpecialToken : Piece, IEquatable<SpecialToken>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SpecialToken" /> type.
        /// </summary>
        /// <param name="value">
        ///     The value of the token.
        /// </param>
        /// <param name="sequenceId">
        ///     Identifies the sequence to link this special token to:
        ///     <list type="bullet">
        ///         <item>
        ///             <term>
        ///                 <see cref="SequenceIdentifier.A" /> for the primary sequence.
        ///             </term>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="SequenceIdentifier.B" /> for the secondary sequence.
        ///             </term>
        ///         </item>
        ///     </list>
        /// </param>
        public SpecialToken(string value, SequenceIdentifier sequenceId)
        {
            Value = value;
            SequenceId = sequenceId;
        }

        /// <summary>
        ///     The value of the token.
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     Identifies the sequence to link this special token to:
        ///     <list type="bullet">
        ///         <item>
        ///             <term>
        ///                 <see cref="SequenceIdentifier.A" /> for the primary sequence.
        ///             </term>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="SequenceIdentifier.B" /> for the secondary sequence.
        ///             </term>
        ///         </item>
        ///     </list>
        /// </summary>
        public SequenceIdentifier SequenceId { get; }

        public bool Equals(SpecialToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value && SequenceId == other.SequenceId;
        }

        protected override bool PieceEquals(Piece other)
        {
            return other is SpecialToken token && Equals(token);
        }

        protected override int GetPieceHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Value, SequenceId);
        }

        public override string ToString()
        {
            return $"{Value}:{SequenceId}";
        }
    }
}
