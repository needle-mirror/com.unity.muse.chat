using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization.Decoders
{
    class DefaultDecoder : DecoderBase
    {
        protected override void DecodeInternal(IEnumerable<string> input, IOutput<string> output)
        {
            output.Write(input);
        }
    }
}
