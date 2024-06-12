using System;
using System.Runtime.InteropServices;

namespace Unity.Muse.Chat.Embeddings.VectorStorage
{
    class Base64KeyGenerator : IDisposable
    {
        static readonly char[] k_Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray();

        bool m_Disposed;
        readonly Random m_Random = new();
        unsafe char* m_Builder = (char*)Marshal.AllocHGlobal(10 * sizeof(char));

        ~Base64KeyGenerator() => DisposeObject();

        public unsafe string Generate()
        {
            if (m_Disposed)
                throw new ObjectDisposedException(null);

            for (var i = 0; i < 10; i++)
                m_Builder[i] = k_Characters[m_Random.Next(k_Characters.Length)];

            return new string(m_Builder);
        }

        public void Dispose()
        {
            DisposeObject();
            GC.SuppressFinalize(this);
        }

        unsafe void DisposeObject()
        {
            if (m_Disposed) return;
            Marshal.FreeHGlobal((IntPtr)m_Builder);
            m_Builder = null;
            m_Disposed = true;
        }
    }
}
