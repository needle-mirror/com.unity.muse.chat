using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization
{
    class CachedConverter<TFrom, TTo> : IConverter<TFrom, TTo>
    {
        readonly Dictionary<TFrom, TTo> m_Cache;
        readonly IConverter<TFrom, TTo> m_Converter;

        public CachedConverter(IConverter<TFrom, TTo> converter) : this(converter,
            EqualityComparer<TFrom>.Default)
        {
        }

        public CachedConverter(
            [NotNull] IConverter<TFrom, TTo> converter,
            [NotNull] IEqualityComparer<TFrom> inputComparer)
        {
            if (inputComparer == null)
                throw new ArgumentNullException(nameof(inputComparer));

            m_Cache = new Dictionary<TFrom, TTo>(inputComparer);

            m_Converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        TTo IConverter<TFrom, TTo>.Convert(TFrom input)
        {
            return Convert(input);
        }

        public TTo Convert(TFrom input)
        {
            if (m_Cache.TryGetValue(input, out var output))
                return output;

            output = m_Converter.Convert(input);
            m_Cache.Add(input, output);
            return output;
        }
    }
}
