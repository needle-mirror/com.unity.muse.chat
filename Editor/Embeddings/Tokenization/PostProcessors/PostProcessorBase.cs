using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Embeddings.Tokenization.PostProcessors
{
    /// <summary>
    ///     Base implementation for builtin <see cref="IPostProcessor" /> types.
    ///     Runs the necessary parameter validation before calling
    ///     <see cref="PostProcessInternal" />.
    /// </summary>
    abstract class PostProcessorBase : IPostProcessor
    {
        int IPostProcessor.GetNumAddedTokens(bool isPair)
        {
            return GetNumAddedTokens(isPair);
        }

        void IPostProcessor.PostProcess(
            IEnumerable<IEnumerable<int>> sequenceA,
            IEnumerable<IEnumerable<int>> sequenceB,
            bool addSpecialTokens,
            IOutput<IEnumerable<int>> output)
        {
            PostProcess(sequenceA, sequenceB, addSpecialTokens, output);
        }

        public abstract int GetNumAddedTokens(bool isPair);

        /// <inheritdoc cref="IPostProcessor.PostProcess" />
        public void PostProcess(
            [NotNull] IEnumerable<IEnumerable<int>> sequenceA,
            [CanBeNull] IEnumerable<IEnumerable<int>> sequenceB,
            bool addSpecialTokens,
            IOutput<IEnumerable<int>> output)
        {
            if (sequenceA == null)
                throw new ArgumentNullException(nameof(sequenceA));

            PostProcessInternal(sequenceA, sequenceB, addSpecialTokens, output);
        }

        /// <inheritdoc cref="IPostProcessor.PostProcess" />
        protected abstract void PostProcessInternal(
            [NotNull] IEnumerable<IEnumerable<int>> sequenceA,
            [CanBeNull] IEnumerable<IEnumerable<int>> sequenceB,
            bool addSpecialTokens,
            IOutput<IEnumerable<int>> output);
    }
}
