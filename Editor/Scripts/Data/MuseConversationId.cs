using System.Diagnostics;

namespace Unity.Muse.Chat
{
    [DebuggerDisplay("{Value}")]
    internal struct MuseConversationId
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public MuseConversationId(string value)
        {
            Value = value;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public readonly string Value;

        public static bool operator ==(MuseConversationId value1, MuseConversationId value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(MuseConversationId value1, MuseConversationId value2)
        {
            return !(value1 == value2);
        }

        public override bool Equals(object obj)
        {
            return obj is MuseConversationId other && Equals(other);
        }

        public bool Equals(MuseConversationId other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(Value) ? 0 : Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public bool IsValid => !string.IsNullOrEmpty(Value);
    }
}
