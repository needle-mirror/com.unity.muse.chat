using System;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Defines a piece of data that can be sent to the LLM along with a user message
    /// </summary>
    interface IContextSelection : IEquatable<IContextSelection>
    {
        /// <summary>
        /// Used for semantic filtering
        /// </summary>
        public string Classifier { get; }

        /// <summary>
        /// Display-friendly description of the type of data returned by the context selection
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The actual data that will be sent to the LLM for evaluation
        /// </summary>
        public string Payload { get; }
    }
}
