using System;
using System.Collections.Generic;

namespace Unity.Muse.Chat.Tokenization
{
    partial class VocabularyBuilder
    {
        class Vocabulary : IVocabulary
        {
            readonly Node<int, TokenDefinition> m_ByIdsLut;
            readonly Node<char, (TokenDefinition definition, TokenDefinition special)> m_ByValueLut;

            public Vocabulary(
                Node<char, (TokenDefinition definition, TokenDefinition special)> byValueLut,
                Node<int, TokenDefinition> byIdsLut)
            {
                m_ByValueLut = byValueLut;
                m_ByIdsLut = byIdsLut;
            }

            bool IVocabulary.Find(SubString input,
                out (ITokenDefinition definition, int length) output,
                string prefix)
            {
                return Find(input, out output, prefix);
            }

            bool IVocabulary.TryGetToken(string value, out ITokenDefinition definition,
                bool special)
            {
                return TryGetToken(value, out definition, special);
            }

            bool IVocabulary.TryGetToken(IEnumerable<int> id, out ITokenDefinition definition)
            {
                return TryGetToken(id, out definition);
            }

            public bool Find(
                SubString input,
                out (ITokenDefinition definition, int length) output,
                string prefix = null)
            {
                if (input.IsEmpty)
                    throw new ArgumentNullException(nameof(input));

                output = default;
                var current = m_ByValueLut;

                if (prefix is not null)
                    foreach (var c in prefix)
                    {
                        if (current.children is null)
                            return false;

                        if (!current.children.TryGetValue(c, out current))
                            return false;
                    }

                var outputLength = 0;

                var (source, offset, length) = input;

                for (int index = offset, limit = offset + length; index < limit; index++)
                {
                    if (current.children is null)
                        break;

                    if (!current.children.TryGetValue(source[index], out var next))
                        break;

                    outputLength++;
                    current = next;
                }

                while (current is not null && current.value.definition is null)
                {
                    current = current.parent;
                    outputLength--;
                }

                if (current is null)
                    return false;

                output = (current.value.definition, outputLength);
                return true;
            }

            public bool TryGetToken(string value, out ITokenDefinition definition,
                bool isSpecial = false)
            {
                definition = default;
                var current = m_ByValueLut;

                foreach (var c in value)
                    if (current.children is null
                        || !current.children.TryGetValue(c, out current))
                        return false;

                definition = isSpecial
                    ? current.value.special
                    : current.value.definition;

                return definition != null;
            }

            public bool TryGetToken(IEnumerable<int> ids, out ITokenDefinition definition)
            {
                definition = default;
                var current = m_ByIdsLut;

                foreach (var id in ids)
                    if (current.children is null
                        || !current.children.TryGetValue(id, out current))
                        return false;

                definition = current.value;

                return definition != null;
            }
        }
    }
}
