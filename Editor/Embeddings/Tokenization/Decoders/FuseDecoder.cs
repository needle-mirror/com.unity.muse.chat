using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization.Decoders
{
    /// <summary>
    ///     Fuse Decoder combine the tokens in list into a single large token.
    /// </summary>
    class FuseDecoder : DecoderBase
    {
        protected override void DecodeInternal(IEnumerable<string> input, IOutput<string> output)
        {
            output.Write(string.Concat(input));
        }
    }
}
