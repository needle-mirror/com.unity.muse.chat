using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.Muse.Chat.Context.SmartContext
{
    internal static class ContextRetrievalHelpers
    {
        internal static GameObject FindGameObject(string gameObjectName, bool isPrefab)
        {
            if (isPrefab)
            {
                return FuzzyGameObjectSearch(gameObjectName,
                        AssetDatabase.FindAssets("t:GameObject")
                            .Select(AssetDatabase.GUIDToAssetPath)
                            .Select(AssetDatabase.LoadAssetAtPath<GameObject>))
                    .FirstOrDefault();
            }

            return FuzzyGameObjectSearch(gameObjectName, Object.FindObjectsOfType<GameObject>())
                .FirstOrDefault();
        }

        internal static IEnumerable<GameObject> FuzzyGameObjectSearch(string pattern,
            IEnumerable<GameObject> objectsToSearch)
        {
            long outScore = 0;
            return objectsToSearch
                .Where(sceneObject => FuzzySearch.FuzzyMatch(pattern, sceneObject.name, ref outScore))
                .Select(sceneObject => new Tuple<GameObject, long>(sceneObject, outScore))
                .OrderByDescending(a => a.Item2)
                .Select(t => t.Item1);
        }
    }
}
