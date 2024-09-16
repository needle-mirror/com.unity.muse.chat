using System;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Stores relevant data from console logs
    /// </summary>
    [Serializable]
    internal class LogReference
    {
        string m_Message;
        string m_File;
        int m_Line;
        int m_Column;
        ConsoleMessageMode m_Mode;
        internal enum ConsoleMessageMode
        {
            Log,
            Warning,
            Error
        }

        internal bool Equals(LogReference other)
        {
            return m_Line == other.m_Line && m_Column == other.m_Column && m_Mode == other.m_Mode &&
                   m_Message == other.m_Message && m_File == other.m_File;
        }

        internal string Message { get => m_Message; set => m_Message = value; }
        internal string File { get => m_File; set => m_File = value; }
        internal int Line { get => m_Line; set => m_Line = value; }
        internal int Column { get => m_Column; set => m_Column = value; }
        internal ConsoleMessageMode Mode { get => m_Mode; set => m_Mode = value; }
    }
}
