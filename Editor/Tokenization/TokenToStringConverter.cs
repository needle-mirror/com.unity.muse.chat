using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization
{
    class TokenToStringConverter : IConverter<IEnumerable<int>, IEnumerable<string>>
    {
        Pool<List<int>> m_ListOfIntPool;
        IVocabulary m_Vocabulary;

        public TokenToStringConverter([NotNull] IVocabulary vocabulary)
        {
            Init(PoolUtility.GetListOfIntPool(), vocabulary);
        }

        internal TokenToStringConverter(Pool<List<int>> listOfIntPool, IVocabulary vocabulary)
        {
            Init(listOfIntPool, vocabulary);
        }

        public IEnumerable<string> Convert(IEnumerable<int> input)
        {
            using var _ = m_ListOfIntPool.Get(out var ids);
            foreach (var id in input)
            {
                ids.Add(id);
                var found = m_Vocabulary.TryGetToken(ids, out var token);
                if (found && !token.IsSpecial)
                    yield return token.Value;
                ids.Clear();
            }
        }

        void Init(Pool<List<int>> listOfIntPool, IVocabulary vocabulary)
        {
            m_ListOfIntPool = listOfIntPool;
            m_Vocabulary = vocabulary;
        }
    }
}
