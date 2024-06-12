namespace Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers
{
    /// <summary>
    ///     Splits on spaces and punctuation, removing spaces, and keeping each punctuation as
    ///     separated chunk.
    /// </summary>
    class BertPreTokenizer : PreTokenizerBase
    {
        protected override void PreTokenizeInternal(SubString input, IOutput<SubString> output)
        {
            var (source, offset, length) = input;
            var limit = offset + length;
            while (offset < limit)
            {
                // consume white spaces
                while (char.IsWhiteSpace(source[offset]))
                {
                    offset++;
                    if (offset == limit)
                        return;
                }

                // c is non-space character
                var c = source[offset];

                if (char.IsPunctuation(c))
                {
                    output.Write(new SubString(source, offset, 1));
                    offset++;
                }

                // alphanumeric character
                else
                {
                    for (var i = offset + 1; i <= limit; i++)
                    {
                        if (i == limit)
                        {
                            output.Write(SubString.FromTo(source, offset, limit));
                            offset = limit;
                            break;
                        }

                        c = source[i];
                        if (i == limit || char.IsPunctuation(c) || char.IsWhiteSpace(c))
                        {
                            output.Write(SubString.FromTo(source, offset, i));
                            offset = i;
                            break;
                        }
                    }
                }
            }
        }
    }
}
