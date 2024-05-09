using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace Unity.Muse.Chat.VectorStorage
{
    partial class VectorStore
    {
        struct FlatCSJob : IJob
        {
            public static float GetMagnitude(NativeArray<float> vector)
            {
                var sqrMagnitude = 0f;
                for (int i = 0, limit = vector.Length; i < limit; i++)
                    sqrMagnitude += vector[i] * vector[i];

                return Mathf.Sqrt(sqrMagnitude);
            }

            static float CosineSimilarity
                (NativeArray<float> a, float aMagnitude, NativeArray<float> b, float bMagnitude)
            {
                var dotProduct = 0f;
                for (int n = 0, limit = a.Length; n < limit; n++)
                {
                    var aN = a[n];
                    var bN = b[n];

                    dotProduct += aN * bN;
                }

                return dotProduct / (aMagnitude * bMagnitude);
            }

            [ReadOnly]
            readonly NativeArray<float> m_Request;

            readonly float m_RequestMagnitude;

            [ReadOnly]
            readonly NativeArray<float> m_Candidate;

            NativeArray<float> m_Result;

            public FlatCSJob(NativeArray<float> request, float requestMagnitude,
                NativeArray<float> candidate, NativeArray<float> result)
            {
                m_Request = request;
                m_RequestMagnitude = requestMagnitude;
                m_Candidate = candidate;
                m_Result = result;

            }

            public void Execute()
            {
                var candidateMagnitude = GetMagnitude(m_Candidate);
                m_Result[0] = CosineSimilarity(m_Request, m_RequestMagnitude, m_Candidate, candidateMagnitude);
            }
        }

        struct FlatCSParallelJob : IJobParallelFor
        {
            public unsafe static float GetMagnitude(float* vector, int count)
            {
                var sqrMagnitude = 0f;
                for (int i = 0; i < count; i++)
                    sqrMagnitude += vector[i] * vector[i];

                return Mathf.Sqrt(sqrMagnitude);
            }

            static float CosineSimilarity
                (ReadOnlySpan<float> a, float aMagnitude, ReadOnlySpan<float> b, float bMagnitude)
            {
                var dotProduct = 0f;
                for (int n = 0, limit = a.Length; n < limit; n++)
                    dotProduct += a[n] * b[n];

                return dotProduct / (aMagnitude * bMagnitude);
            }

            static unsafe float CosineSimilarity
                (NativeArray<float> a, float aMagnitude, NativeArray<float> b, int bIndex, float bMagnitude)
            {
                var dotProduct = 0f;
                var pA = (float*)a.GetUnsafeReadOnlyPtr();
                var pB = (float*) b.GetUnsafeReadOnlyPtr() + bIndex;

                for (int i = 0, limit = a.Length; i < limit; i++)
                    dotProduct += pA[i] * pB[i];

                return dotProduct / (aMagnitude * bMagnitude);
            }

            static unsafe float CosineSimilarity
                (float* pA, float aMagnitude, float* pB, float bMagnitude)
            {
                var dotProduct = 0f;
                for (int i = 0, limit = 384; i < limit; i++)
                    dotProduct += pA[i] * pB[i];

                return dotProduct / (aMagnitude * bMagnitude);
            }

            [ReadOnly]
            NativeArray<float> m_Request;

            float m_RequestMagnitude;

            [ReadOnly]
            //NativeArray<float> m_Candidates;
            NativeArray<IntPtr> m_Candidates;

            NativeArray<float> m_Results;

            //public FlatCSParallelJob(NativeArray<float> request, NativeArray<float> candidates, NativeArray<float> results)
            public unsafe FlatCSParallelJob(NativeArray<float> request, NativeArray<IntPtr> candidates, NativeArray<float> results)
            {
                m_Request = request;
                m_RequestMagnitude = GetMagnitude((float*)m_Request.GetUnsafeReadOnlyPtr(), request.Length);

                m_Candidates = candidates;

                m_Results = results;
            }

            public void Execute(int index)
            {
                var count = m_Request.Length;
                unsafe
                {
                    var candidate = (float*)m_Candidates[index];
                    var candidateMagnitude = GetMagnitude(candidate, count);
                    var pRequest = (float*)m_Request.GetUnsafeReadOnlyPtr();

                    m_Results[index] = CosineSimilarity(
                        pRequest,
                        m_RequestMagnitude,
                        candidate,
                        candidateMagnitude);
                }
            }
        }
    }
}
