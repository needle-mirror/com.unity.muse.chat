using System;

namespace Unity.Muse.Chat
{
    /// <summary>
    ///     Marks a static method returning some context for Muse Chat as a <see cref="string"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MuseChatContextAttribute : Attribute
    {
        /// <summary>
        ///     Used for semantic search.
        /// </summary>
        public readonly string Classifier;

        /// <summary>
        ///     A description of the piece of context returned by the method.
        /// </summary>
        public readonly string Description;

        /// <summary>
        ///     Tells whether this method must be called no matter the request.
        /// </summary>
        public readonly bool AlwaysInclude;

        /// <summary>
        ///     Marks a static method returning some context for Muse Chat as a <see cref="string"/>.
        /// </summary>
        /// <param name="classifier">
        ///     Used for semantic search.
        /// </param>
        /// <param name="description">
        ///     A description of the piece of context returned by the method.
        /// </param>
        /// <param name="alwaysInclude">
        ///     Tells whether this method must be called no matter the request.
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public MuseChatContextAttribute(string classifier = default, string description = default, bool alwaysInclude = false)
        {
            if (!alwaysInclude && string.IsNullOrWhiteSpace(classifier))
                throw new ArgumentNullException(nameof(classifier),
                    $"Cannot be null if {nameof(alwaysInclude)} is false");

            Classifier = classifier;
            Description = description;
            AlwaysInclude = alwaysInclude;
        }
    }
}
