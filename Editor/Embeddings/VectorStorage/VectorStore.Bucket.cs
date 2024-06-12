using System;
using Unity.Collections;

namespace Unity.Muse.Chat.Embeddings.VectorStorage
{
    partial class VectorStore
    {
        public class Bucket : IDisposable
        {
            bool m_Disposed;

            NativeArray<float> m_Data;
            int m_VectorSize;

            public readonly int Capacity;

            public Bucket(int capacity, int vectorSize)
            {
                Capacity = capacity;
                m_VectorSize = vectorSize;
                m_Data = new NativeArray<float>(capacity * vectorSize, Allocator.Persistent);
            }

            ~Bucket()
            {
                DisposeObject();
            }

            public void Dispose()
            {
                DisposeObject();
                GC.SuppressFinalize(this);
            }

            void DisposeObject()
            {
                if (m_Disposed)
                    return;

                m_Data.Dispose();
                m_Disposed = true;
            }
        }
    }
}
