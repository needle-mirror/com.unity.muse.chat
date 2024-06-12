using System;
using System.Linq;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers
{
    class SequencePreTokenizer : PreTokenizerBase
    {
        readonly IPreTokenizer[] m_PreTokenizers;

        public SequencePreTokenizer([NotNull] params IPreTokenizer[] preTokenizers)
        {
            if (preTokenizers == null)
                throw new ArgumentNullException(nameof(preTokenizers));

            if (preTokenizers.Length == 0)
                throw new ArgumentException("At least one preTokenizer is required");

            if (preTokenizers.Any(t => t is null))
                throw new ArgumentNullException(nameof(preTokenizers),
                    $"None of the {nameof(preTokenizers)} can be null.");

            m_PreTokenizers = preTokenizers;
        }

        protected override void PreTokenizeInternal(SubString input, IOutput<SubString> output)
        {
            using var inputListHandle = PoolUtility.GetListOfSubStringPool().Get(out var inputList);
            using var outputListHandle =
                PoolUtility.GetListOfSubStringPool().Get(out var outputList);
            using var tempOutputHandle =
                PoolUtility.GetOutputOfSubStringPool().Get(out var tempOutput);

            outputList.Add(input);

            foreach (var preTokenizer in m_PreTokenizers)
            {
                (inputList, outputList) = (outputList, inputList);

                tempOutput.Target = outputList;
                foreach (var subString in inputList)
                    preTokenizer.PreTokenize(subString, tempOutput);

                tempOutput.Reset();
                inputList.Clear();
            }

            output.Write(outputList);
        }
    }
}
