using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Embeddings.Tokenization.PaddingProcessors
{
    /// <summary>
    ///     Base implementation for builtin padding processors.
    ///     Runs the necessary parameter validation before calling <see cref="PadInternal" />.
    /// </summary>
    abstract class PaddingProcessorBase : IPaddingProcessor
    {
        void IPaddingProcessor.Pad(
            IEnumerable<IEnumerable<int>> input,
            IOutput<IEnumerable<(int id, int attention)>> output)
        {
            Pad(input, output);
        }

        /// <inheritdoc cref="IPaddingProcessor.Pad" />
        public void Pad(
            [NotNull] IEnumerable<IEnumerable<int>> input,
            [NotNull] IOutput<IEnumerable<(int id, int attention)>> output)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            PadInternal(input, output);
        }

        /// <inheritdoc cref="IPaddingProcessor.Pad" />
        protected abstract void PadInternal(
            [NotNull] IEnumerable<IEnumerable<int>> input,
            [NotNull] IOutput<IEnumerable<(int id, int attention)>> output);
    }
}
