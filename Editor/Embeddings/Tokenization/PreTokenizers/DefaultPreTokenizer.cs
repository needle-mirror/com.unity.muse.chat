namespace Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers
{
    /// <summary>
    ///     Default placeholder implementation of a pretokenizer.
    ///     Does not pre-cut the input.
    /// </summary>
    class DefaultPreTokenizer : PreTokenizerBase
    {
        protected override void PreTokenizeInternal(SubString input, IOutput<SubString> output)
        {
            output.Write(input);
        }
    }
}
