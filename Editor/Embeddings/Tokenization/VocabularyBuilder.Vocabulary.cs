using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization
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

            bool IVocabulary.TryGetToken(SubString value, out ITokenDefinition definition,
                bool special, SubString? prefix)
            {
                return TryGetToken(value, out definition, special, prefix);
            }

            bool IVocabulary.TryGetToken(IEnumerable<int> id, out ITokenDefinition definition)
            {
                return TryGetToken(id, out definition);
            }

            public bool TryGetToken(string value, out ITokenDefinition definition,
                bool isSpecial = false, SubString? prefix = null)
            {
                definition = default;
                var current = m_ByValueLut;

                if (prefix.HasValue)
                    foreach (var c in prefix.Value)
                        if (current.children is null
                            || !current.children.TryGetValue(c, out current))
                            return false;

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
