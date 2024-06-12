using Unity.Muse.Chat.Embeddings.Tokenization.PostProcessors;

namespace Unity.Muse.Chat.Embeddings.Tokenization
{
    /// <summary>
    ///     Identifies a sequence.
    ///     It is used in the <see cref="TemplatePostProcessor" />.
    /// </summary>
    internal enum SequenceIdentifier
    {
        /// <summary>
        ///     First sequence.
        /// </summary>
        A,

        /// <summary>
        ///     Second sequence.
        /// </summary>
        B
    }
}
