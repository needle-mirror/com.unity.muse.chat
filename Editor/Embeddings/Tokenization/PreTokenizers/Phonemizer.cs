namespace Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers
{
    /// <summary>
    ///     Converts the input into its phoneme representation, using ESpeak as conversion backend.
    /// </summary>
    class Phonemizer : PreTokenizerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Phonemizer" /> type.
        /// </summary>
        /// <param name="lang">
        ///     The language to use.
        /// </param>
        public Phonemizer(string lang = "en-US")
        {
            Lang = lang;
        }

        /// <summary>
        ///     The language used for the conversion to phonemes.
        /// </summary>
        public string Lang { get; }

        protected override void PreTokenizeInternal(SubString input, IOutput<SubString> output)
        {
            if (!EspeakWrapper.Initialized)
                EspeakWrapper.Initialize();

            EspeakWrapper.SetLang(Lang);
            var phonemized = EspeakWrapper.TextToPhonemes(input);
            output.Write(phonemized);
        }
    }
}
