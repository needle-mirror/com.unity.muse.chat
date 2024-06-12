using System;
using System.Collections.Generic;
using Unity.Muse.Chat.Embeddings.Tokenization.PostProcessors.Templating;

namespace Unity.Muse.Chat.Embeddings.Tokenization.PostProcessors
{
    class TemplatePostProcessor : PostProcessorBase
    {
        readonly Template m_PairSequenceTemplate;

        readonly Template m_SingleSequenceTemplate;

        readonly Dictionary<string, ITokenDefinition> m_SpecialTokens;

        public TemplatePostProcessor(Template single, Template pair,
            IEnumerable<ITokenDefinition> specialTokens)
        {
            m_SpecialTokens = new Dictionary<string, ITokenDefinition>();

            foreach (var token in specialTokens)
                m_SpecialTokens[token.Value] = token;

            if (single is not null)
            {
                CheckTemplate(single, nameof(single), m_SpecialTokens, true);
                m_SingleSequenceTemplate = single;
            }

            if (pair is not null)
            {
                CheckTemplate(pair, nameof(pair), m_SpecialTokens, false);
                m_PairSequenceTemplate = pair;
            }
        }

        static void CheckTemplate(
            Template template,
            string nameofTemplate,
            IReadOnlyDictionary<string, ITokenDefinition> specialTokens,
            bool isSingle)
        {
            var sequenceAFound = false;
            var sequenceBFound = false;

            foreach (var piece in template.Pieces)
                switch (piece)
                {
                    case SpecialToken specialToken
                        when !specialTokens.ContainsKey(specialToken.Value):
                        throw new KeyNotFoundException(
                            $"Token {specialToken.Value} in template {nameofTemplate} not found in the special tokens list.");
                    case Sequence sequence:
                    {
                        sequenceAFound |= sequence.Identifier is SequenceIdentifier.A;
                        sequenceBFound |= sequence.Identifier is SequenceIdentifier.B;
                        break;
                    }
                }

            if (!sequenceAFound)
                throw new FormatException(
                    "Sequence B cannot be used in a single sequence template.");

            switch (isSingle)
            {
                case true when sequenceBFound:
                    throw new FormatException(
                        "Sequence B cannot be used in a single sequence template.");
                case false when !sequenceBFound:
                    throw new FormatException("Sequence B must appears in the template.");
            }
        }

        public override int GetNumAddedTokens(bool isPair)
        {
            return 2;
        }

        protected override void PostProcessInternal(
            IEnumerable<IEnumerable<int>> sequenceA,
            IEnumerable<IEnumerable<int>> sequenceB,
            bool addSpecialTokens,
            IOutput<IEnumerable<int>> output)
        {
            var template = sequenceB is not null
                ? m_PairSequenceTemplate
                : m_SingleSequenceTemplate;

            using var enumA = sequenceA.GetEnumerator();
            using var enumB = sequenceB?.GetEnumerator();

            var (nextA, nextB) = (enumA.MoveNext(), enumB?.MoveNext() ?? false);

            while (nextA || nextB)
            {
                var tokensA = nextA ? enumA.Current : default;
                var tokensB = nextB ? enumB!.Current : default;

                output.Write(PostProcess(tokensA, tokensB, template, addSpecialTokens));

                (nextA, nextB) = (enumA.MoveNext(), enumB?.MoveNext() ?? false);
            }
        }

        IEnumerable<int> PostProcess(
            IEnumerable<int> tokensA,
            IEnumerable<int> tokensB,
            Template template,
            bool addSpecialTokens)
        {
            foreach (var piece in template.Pieces)
                switch (piece)
                {
                    case Sequence sequence:
                        var tokens = sequence.Identifier == SequenceIdentifier.A
                            ? tokensA
                            : tokensB;
                        if (tokens is not null)
                            foreach (var token in tokens)
                                yield return token;
                        break;
                    case SpecialToken specialToken:
                        if (addSpecialTokens)
                            foreach (var id in m_SpecialTokens[specialToken.Value].Ids)
                                yield return id;
                        break;
                }
        }
    }
}
