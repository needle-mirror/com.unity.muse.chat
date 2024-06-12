using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Embeddings.Tokenization
{
    /// <summary>
    ///     Represents a portion of a <see cref="string" /> value.
    /// </summary>
    /// <remarks>
    ///     This type is required as <see cref="ReadOnlySpan{T}" /> has some blocking constraints.
    /// </remarks>
    struct SubString :
        IEquatable<string>, IComparable<string>,
        IEquatable<SubString>, IComparable<SubString>,
        IEnumerable<char>
    {
        const string k_NullSourceExceptionMessage =
            "The underlying source of this substring is null";

        /// <summary>
        ///     Creates a <see cref="SubString" /> instance from a full <see cref="string" /> value.
        /// </summary>
        /// <param name="input">
        ///     The original <see cref="string" /> value.
        ///     The resulting <see cref="SubString" /> will cover the whole value of
        ///     <paramref name="input" />.
        /// </param>
        /// <returns>
        ///     A <see cref="SubString" /> instance covering the whole <paramref name="input" />.
        /// </returns>
        public static implicit operator SubString(string input)
        {
            return new SubString(input);
        }

        /// <summary>
        ///     Gets a <see cref="string" /> value from the portion of the source
        ///     <see cref="string" /> of this <see cref="SubString" />.
        /// </summary>
        /// <param name="input">
        ///     The <see cref="SubString" /> value to convert to a <see cref="string" /> value.
        /// </param>
        /// <returns>
        ///     The <see cref="string" /> representing the value of this <see cref="SubString" />.
        /// </returns>
        public static implicit operator string(SubString input)
        {
            return input.ToString();
        }

        /// <summary>
        ///     Creates a <see cref="SubString" /> instance from a <see cref="string" /> source and
        ///     the bounds of the portion to keep.
        /// </summary>
        /// <param name="source">
        ///     The source <see cref="string" /> to build this <see cref="SubString" /> from.
        /// </param>
        /// <param name="from">
        ///     The lower bound of the portion of <paramref name="source" /> to keep.
        /// </param>
        /// <param name="to">
        ///     The upper bound of the portion of <paramref name="source" /> to keep.
        /// </param>
        /// <returns>
        ///     A <see cref="SubString" /> value.
        /// </returns>
        public static SubString FromTo(string source, int from, int to)
        {
            return new SubString(source, from, to - from);
        }

        /// <summary>
        ///     The computed hash code of the portion.
        ///     <see cref="SubString" /> uses itw owns implementation of <see cref="string" /> hash
        ///     code computation because the standard one is not exposed as a helper method by the
        ///     standard library at the moment (recent versions of .NET exposes it).
        ///     It is a nullable value as it is not computed while not required.
        /// </summary>
        int? m_HashCode;

        /// <summary>
        ///     The number of UTF-8 characters of this portion.
        /// </summary>
        int? m_UtfLength;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SubString" /> type.
        /// </summary>
        /// <param name="source">
        ///     The source <see cref="string" /> from which this <see cref="SubString" /> is built.
        /// </param>
        /// <param name="offset">
        ///     The lower bound of the portion of <see cref="Source" /> to keep.
        /// </param>
        /// <param name="length">
        ///     The length of the portion of <see cref="Source" /> to keep.
        /// </param>
        public SubString([CanBeNull] string source, int offset, int length)
        {
            Source = source;

            if (offset < 0 || offset > (source?.Length ?? 0))
                throw new ArgumentOutOfRangeException(nameof(offset), offset, null);
            if (length < 0 || offset + length > (source?.Length ?? 0))
                throw new ArgumentOutOfRangeException(nameof(length), length, null);

            Offset = offset;
            Length = length;
            m_HashCode = source is null ? 0 : default;
            m_UtfLength = default;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SubString" /> type.
        /// </summary>
        /// <param name="source">
        ///     The source <see cref="string" /> from which this <see cref="SubString" /> is built.
        ///     This constructor keeps the whole <paramref name="source" />.
        /// </param>
        public SubString(string source) : this(source, 0, source?.Length ?? 0)
        {
        }

        /// <summary>
        ///     Tells whether the portion covers the source string.
        /// </summary>
        public bool IsApplied => IsNull
            ? throw new NullReferenceException(k_NullSourceExceptionMessage)
            : Offset == 0 && Length == Source.Length;

        /// <summary>
        ///     The source <see cref="string" /> from which this <see cref="SubString" /> is built.
        /// </summary>
        public string Source { get; }

        /// <summary>
        ///     The lower bound of the portion of <see cref="Source" /> to keep.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        ///     The number of <see cref="char" /> of this portion.
        /// </summary>
        public int Length { get; }

        /// <summary>
        ///     The number of Utf-8 valid characters of this portion.
        /// </summary>
        public int UtfLength => m_UtfLength ??= GetUtfLength();

        /// <summary>
        ///     Tells whether the substring does not reference any valid source.
        /// </summary>
        public bool IsNull => Source is null;

        /// <summary>
        ///     Tells whether this instance is empty.
        /// </summary>
        public bool IsEmpty => IsNull
            ? throw new NullReferenceException(k_NullSourceExceptionMessage)
            : Length == 0;

        /// <summary>
        ///     Tells whether this instance is null, empty or if it just contains white spaces.
        /// </summary>
        public bool IsNullOrWhiteSpace
        {
            get
            {
                var source = Source;
                if (source is null) return true;

                for (int i = Offset, limit = Offset + Length; i < limit; i++)
                    if (!char.IsWhiteSpace(source[i]))
                        return false;

                return true;
            }
        }

        int GetUtfLength()
        {
            var count = 0;
            for (int i = Offset, limit = Offset + Length; i < limit; i++)
            {
                while (i + 1 < limit && char.IsSurrogate(Source[i]))
                    i++;

                count++;
            }

            return count;
        }

        /// <summary>
        ///     Returns a new <see cref="SubString" /> value which source <see cref="string" /> is the
        ///     portion of this one.
        /// </summary>
        /// <returns>
        ///     A new <see cref="SubString" /> value which source is the portion of this one.
        /// </returns>
        /// <remarks>
        ///     If the hash code has already been computed for this <see cref="SubString" />, it is
        ///     copied to the new one.
        /// </remarks>
        public SubString Apply()
        {
            return IsApplied
                ? this
                : new SubString(Source.Substring(Offset, Length)) {m_HashCode = m_HashCode};
        }

        /// <summary>
        ///     Gets a portion of this instance.
        /// </summary>
        /// <param name="offset">
        ///     The zero-based starting character position of a substring in this instance.
        /// </param>
        /// <param name="length">
        ///     The number of characters in the substring.
        /// </param>
        /// <returns>
        ///     A new <see cref="SubString" /> instance.
        /// </returns>
        public SubString Sub(int offset, int length)
        {
            return IsNull
                ? throw new NullReferenceException(k_NullSourceExceptionMessage)
                : new SubString(Source, offset + Offset, length);
        }

        /// <summary>
        ///     Gets a portion of this instance.
        /// </summary>
        /// <param name="offset">
        ///     The zero-based starting character position of a substring in this instance.
        /// </param>
        /// <returns>
        ///     A new <see cref="SubString" /> instance.
        /// </returns>
        public SubString Sub(int offset)
        {
            return IsNull
                ? throw new NullReferenceException(k_NullSourceExceptionMessage)
                : new SubString(Source, offset + Offset, Length - offset);
        }

        public SubString UtfSub(int offset, int length)
        {
            if (IsNull)
                throw new NullReferenceException(k_NullSourceExceptionMessage);

            var charOffset = 0;

            for (var i = 0; i < offset; i++)
            {
                if (charOffset >= Length)
                    throw new ArgumentOutOfRangeException(nameof(offset));

                while (charOffset < Length && char.IsSurrogate(Source[Offset + charOffset]))
                    charOffset++;

                charOffset++;
            }

            var charLength = 0;

            for (var i = 0; i < length; i++)
            {
                if (charOffset + charLength >= Length)
                    throw new ArgumentOutOfRangeException(nameof(length));

                while (charOffset + charLength < Length
                    && char.IsSurrogate(Source[Offset + charOffset + charLength]))
                    charLength++;

                charLength++;
            }

            return new(Source, Offset + charOffset, charLength);
        }

        public SubString UtfSub(int offset)
        {
            if (IsNull)
                throw new NullReferenceException(k_NullSourceExceptionMessage);

            var charOffset = 0;

            for (var i = 0; i < offset; i++)
            {
                if (charOffset >= Length)
                    throw new ArgumentOutOfRangeException(nameof(offset));

                while (charOffset < Length && char.IsSurrogate(Source[Offset + charOffset]))
                    charOffset++;

                charOffset++;
            }

            return new(Source, Offset + charOffset, Length - charOffset);
        }

        public bool StartsWith(SubString prefix)
        {
            if (IsNull)
                throw new NullReferenceException(k_NullSourceExceptionMessage);

            if (prefix.IsNull)
                throw new ArgumentNullException(nameof(prefix));

            var prefixLength = prefix.Length;
            if (Offset + prefixLength > Source.Length)
                return false;

            unsafe
            {
                fixed (char* pSource = Source)
                fixed (char* pPrefixSource = prefix.Source)
                {
                    var pMe = pSource + Offset;
                    var pPrefix = pPrefixSource + prefix.Offset;
                    for (var i = 0; i < prefixLength; i++)
                        if (pPrefix[i] != pMe[i])
                            return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public int CompareTo(string other)
        {
            return CompareTo((SubString)other);
        }

        /// <inheritdoc />
        public unsafe int CompareTo(SubString other)
        {
            if (IsNull)
                throw new NullReferenceException(k_NullSourceExceptionMessage);

            var (from, length) = (Offset, Length);
            var (otherFrom, otherLength) = (other.Offset, other.Length);

            length = length < otherLength
                ? length
                : otherLength;

            fixed (char* pSource = Source)
            fixed (char* pOtherSource = other.Source)
            {
                var pSub = pSource + from;
                var pOtherSub = pOtherSource + otherFrom;

                for (var i = 0; i < length; i++)
                {
                    var comp = pSub[i].CompareTo(pOtherSub[i]);
                    if (comp != 0) return comp;
                }
            }

            return length - otherLength;
        }

        /// <summary>
        ///     Deconstructs this <see cref="SubString" />.
        /// </summary>
        /// <param name="source">
        ///     The source <see cref="string" /> from which this <see cref="SubString" /> is built.
        /// </param>
        /// <param name="offset">
        ///     The lower bound of the portion of <see cref="Source" /> to keep.
        /// </param>
        /// <param name="length">
        ///     The length of the portion of <see cref="Source" /> to keep.
        /// </param>
        public void Deconstruct(out string source, out int offset, out int length)
        {
            if (IsNull)
                throw new NullReferenceException(k_NullSourceExceptionMessage);

            source = Source;
            offset = Offset;
            length = Length;
        }

        /// <inheritdoc />
        public bool Equals(string other)
        {
            return other != null && Equals((SubString)other);
        }

        /// <inheritdoc />
        public bool Equals(SubString other)
        {
            if (IsNull)
                throw new NullReferenceException(k_NullSourceExceptionMessage);

            if (other.IsNull)
                return false;

            var length = Length;
            if (length != other.Length)
                return false;

            unsafe
            {
                fixed (char* pSource = Source)
                fixed (char* pOther = other.Source)
                {
                    var pChar = pSource + Offset;
                    var pOtherChar = pOther + other.Offset;

                    for (var i = 0; i < length; i++)
                        if (pChar[i] != pOtherChar[i])
                            return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (IsNull)
                throw new NullReferenceException(k_NullSourceExceptionMessage);

            if (m_HashCode.HasValue)
                return m_HashCode.Value;

            var hash1 = 5381;
            var hash2 = hash1;

            var (source, i, limit) = (Source, Offset, Offset + Length);
            while (i < limit)
            {
                var c = source[i];
                hash1 = ((hash1 << 5) + hash1) ^ c;

                if (++i == limit)
                    break;

                c = source[i];
                hash2 = ((hash2 << 5) + hash2) ^ c;
                i++;
            }

            var hashCode = hash1 + hash2 * 1566083941;
            m_HashCode = hashCode;
            return hashCode;
        }

        public override string ToString()
        {
            return IsApplied
                ? Source
                : Source.Substring(Offset, Length);
        }

        /// <summary>
        ///     Gets the sequence of <see cref="char" /> from the portion of the source
        ///     <see cref="string" /> covered by this <see cref="SubString" />.
        /// </summary>
        /// <returns>
        ///     The sequence of <see cref="char" /> from the portion of the source
        ///     <see cref="string" /> covered by this <see cref="SubString" />.
        /// </returns>
        IEnumerable<char> GetChars()
        {
            if (IsNull)
                throw new NullReferenceException(k_NullSourceExceptionMessage);

            var (source, from, to) = (Source, Offset, Offset + Length);
            for (var i = from; i < to; i++)
                yield return source[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetChars().GetEnumerator();
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return GetChars().GetEnumerator();
        }
    }
}
