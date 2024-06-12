using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization.Decoders
{
    interface IDecoder
    {
        /// <summary>
        ///     Decodes output token ids to strings and postprocess the string.
        /// </summary>
        void Decode(IEnumerable<string> input, IOutput<string> output);
    }
}
