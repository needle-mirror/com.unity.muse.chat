using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization.Decoders
{
    abstract class DecoderBase : IDecoder
    {
        public void Decode([NotNull] IEnumerable<string> input, IOutput<string> output)
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));

            DecodeInternal(input, output);
        }

        protected abstract void DecodeInternal(IEnumerable<string> input, IOutput<string> output);
    }
}
