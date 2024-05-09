using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization
{
    class Output<T> : IOutput<T>
    {
        public ICollection<T> Target { get; set; }

        void IOutput<T>.Write(T value)
        {
            Target!.Add(value);
        }

        void IOutput<T>.Write([NotNull] IEnumerable<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            foreach (var value in values)
                Target.Add(value);
        }

        public void Reset()
        {
            Target = null;
        }
    }
}
