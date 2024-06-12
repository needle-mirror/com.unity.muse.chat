using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Embeddings.Tokenization
{
    /// <summary>
    ///     Assistant for <see cref="IVocabulary" /> instance creation.
    /// </summary>
    partial class VocabularyBuilder
    {
        /// <summary>
        ///     Keeps the list of definitions, assuring that they are unique by value and special
        ///     state.
        /// </summary>
        readonly HashSet<TokenDefinition> m_Definitions = new(new ValueSpecialComparer());

        /// <summary>
        ///     Assures that token ids are unique.
        /// </summary>
        readonly HashSet<TokenDefinition> m_DefinitionsByIds = new(new IdsComparer());

        /// <summary>
        ///     Adds a new definition to the builder.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="string" /> representation of the definition.
        /// </param>
        /// <param name="ids">
        ///     The sequence of <see cref="int" /> ids of the definition.
        /// </param>
        /// <param name="special">
        /// </param>
        /// <returns>
        ///     <see langword="this" />
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <list type="bullet">
        ///         <item>
        ///             <term>
        ///                 <paramref name="value" /> cannot be <see langword="null" />.
        ///             </term>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <paramref name="ids" /> cannot be <see langword="null" />.
        ///             </term>
        ///         </item>
        ///     </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="ids" /> must contain at least one id.
        /// </exception>
        public VocabularyBuilder Add(
            [NotNull] string value,
            [NotNull] IEnumerable<int> ids,
            bool special = false)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var tokenIds = ids.ToList().AsReadOnly();
            if (tokenIds.Count == 0)
                throw new ArgumentException("Must contain at least one id.", nameof(ids));

            return Add(value, tokenIds, special);
        }

        /// <summary>
        ///     Adds a new definition to the builder.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="string" /> representation of the definition.
        /// </param>
        /// <param name="id">
        ///     The <see cref="int" /> id of the definition.
        /// </param>
        /// <param name="special">
        /// </param>
        /// <returns>
        ///     <see langword="this" />
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="value" /> cannot be <see langword="null" />.
        /// </exception>
        public VocabularyBuilder Add(
            [NotNull] string value,
            int id,
            bool special = false)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var tokensIds = new ReadOnlyCollection<int>(new[] {id}.ToList());
            return Add(value, tokensIds, special);
        }

        VocabularyBuilder Add(
            string value,
            IReadOnlyCollection<int> ids,
            bool special)
        {
            var definition = new TokenDefinition(value, ids, special);
            if (m_Definitions.Contains(definition) || m_DefinitionsByIds.Contains(definition))
                throw new Exception($"A similar definition already exists for {definition}.");

            m_Definitions.Add(definition);
            m_DefinitionsByIds.Add(definition);

            return this;
        }

        /// <summary>
        ///     Builds a <see cref="IVocabulary" /> instance with all the added definitions.
        /// </summary>
        /// <returns>
        ///     A <see cref="IVocabulary" /> instance with all the added definitions.
        /// </returns>
        public IVocabulary Build()
        {
            var rootValueLut =
                new Node<char, (TokenDefinition definition, TokenDefinition special)>();

            var rootIdsLut = new Node<int, TokenDefinition>();

            foreach (var definition in m_Definitions)
            {
                AddToValueLut(definition, rootValueLut);
                AddToIdsLut(definition, rootIdsLut);
            }

            return new Vocabulary(rootValueLut, rootIdsLut);

            void AddToValueLut(
                TokenDefinition definition,
                Node<char, (TokenDefinition definition, TokenDefinition special)> byValueLut)
            {
                var current = byValueLut;
                foreach (var c in definition.Value)
                {
                    current.children ??=
                        new Dictionary<char, Node<char, (TokenDefinition definition, TokenDefinition
                            special)>>();

                    if (!current.children.TryGetValue(c, out var next))
                    {
                        next = new Node<char, (TokenDefinition, TokenDefinition)>
                        {
                            parent = current
                        };
                        current.children[c] = next;
                    }

                    current = next;
                }

                current.value = definition.IsSpecial
                    ? (current.value.definition, definition)
                    : (definition, current.value.special);
            }

            void AddToIdsLut(TokenDefinition definition, Node<int, TokenDefinition> byIdsLut)
            {
                var current = byIdsLut;
                foreach (var id in definition.Ids)
                {
                    current.children ??= new Dictionary<int, Node<int, TokenDefinition>>();

                    if (!current.children.TryGetValue(id, out var next))
                    {
                        next = new Node<int, TokenDefinition> {parent = current};
                        current.children[id] = next;
                    }

                    current = next;
                }

                current.value = definition;
            }
        }

        /// <summary>
        ///     Removes the definitions.
        /// </summary>
        /// <returns>
        ///     <see langword="this" />
        /// </returns>
        public VocabularyBuilder Clear()
        {
            m_Definitions.Clear();
            m_DefinitionsByIds.Clear();
            return this;
        }
    }
}
