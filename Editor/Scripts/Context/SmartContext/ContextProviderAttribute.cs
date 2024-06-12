using System;

namespace Unity.Muse.Chat.Context.SmartContext
{
    /// <summary>
    ///     Marks a static method returning some context for Muse Chat as a <see cref="string"/>.
    ///     Each method parameter must have a <see cref="ParameterAttribute"/> attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ContextProviderAttribute : Attribute
    {
        /// <summary>
        ///     A description of the piece of context returned by the method.
        /// </summary>
        public readonly string Description;

        /// <summary>
        ///     Marks a static method returning some context for Muse Chat as a <see cref="string"/>.
        /// </summary>
        /// <param name="description">
        ///     A description of the piece of context returned by the method.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if the description is empty.</exception>
        public ContextProviderAttribute(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException(nameof(description),
                    $"Cannot be empty");
            }

            Description = description;
        }
    }

    /// <summary>
    ///     Marks a parameter of a method decorated with a <see cref="ContextProviderAttribute"/> attribute with a description of its purpose.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal class ParameterAttribute : Attribute
    {
        public readonly string Description;

        public ParameterAttribute(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException(nameof(description),
                    $"Cannot be empty");
            }

            Description = description;
        }
    }
}
