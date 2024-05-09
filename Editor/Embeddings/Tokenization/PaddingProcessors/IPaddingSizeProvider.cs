using System.Collections.Generic;

namespace Unity.Muse.Chat.Tokenization.PaddingProcessors
{
    /// <summary>
    ///     Computes the size of the padded sequence of tokens.
    /// </summary>
    interface IPaddingSizeProvider
    {
        /// <summary>
        ///     Computes the size of the padded sequence of tokens.
        /// </summary>
        /// <param name="sizes">
        ///     The size of all the sequences of tokens to pad.
        /// </param>
        /// <returns></returns>
        int GetPaddingSize(IEnumerable<int> sizes);
    }
}
