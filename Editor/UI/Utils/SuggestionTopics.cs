using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity.Muse.Chat
{
    static class SuggestionTopics
    {
        static readonly string[] k_Topics =
        {
            "Teach me step by step to build my first game.",
            "How can I create a character controller for walking, running, and jumping?",
            "How can I make an inverse kinematic leg?",
            "How to create a rain shader"
        };

        public static IEnumerable<string> GetRandomList(int count = 3)
        {
            int topicsToSelect = Mathf.Min(count, k_Topics.Length);

            // Generate a list of shuffled indices
            var shuffledIndices = Enumerable.Range(0, k_Topics.Length)
                .OrderBy(_ => Random.value)
                .Take(topicsToSelect);

            return shuffledIndices.Select(index => k_Topics[index]).ToList();
        }
    }
}
