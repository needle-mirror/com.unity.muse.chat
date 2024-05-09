using System.Collections.Generic;
using System.Linq;

namespace Unity.Muse.Chat.Tokenization.PaddingProcessors
{
    /// <summary>
    ///     Placeholder padding processor.
    ///     Does not apply in padding rules.
    /// </summary>
    class DefaultPaddingProcessor : PaddingProcessorBase
    {
        protected override void PadInternal(
            IEnumerable<IEnumerable<int>> input,
            IOutput<IEnumerable<(int id, int attention)>> output)
        {
            foreach (var sequence in input)
                output.Write(sequence.Select(t => (t, 1)));
        }
    }
}
