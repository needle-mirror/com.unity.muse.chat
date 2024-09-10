using System;
using System.Collections.Generic;
using System.IO;
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
        /// <summary>
        /// Returns the GameObject or prefab that most closely matches the given name.
        /// </summary>
        /// <param name="gameObjectName">Name to fuzzy search for</param>
        /// <returns>Matching GameObject</returns>
        internal static T FindObject<T>(string gameObjectName) where T : Object
        {
            var objectsToSearch =
                Object.FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID).ToList<Object>()
                    .Concat(
                        AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                            .Select(AssetDatabase.GUIDToAssetPath)
                            .Where(path => path.StartsWith("Assets"))
                            .Select(AssetDatabase.LoadAssetAtPath<T>));

            var listAsT = new List<T>();
            foreach (var o in objectsToSearch)
            {
                if (o is T oAsT)
                {
                    listAsT.Add(oAsT);
                }
            }

            return FuzzyObjectSearch(gameObjectName, listAsT).FirstOrDefault();
        }

        internal static IEnumerable<Component> FindComponents(GameObject gameObject, string componentName)
        {
            if (componentName.ToLower() == "script")
            {
                componentName = nameof(MonoBehaviour);
            }

            long outScore = 0;
            var result = gameObject.GetComponents<Component>()
                .Where(comp => comp != null)
                .Where(comp => FuzzySearch.FuzzyMatch(componentName, comp.GetType().Name, ref outScore))
                .Select(comp => new Tuple<Component, long>(comp, outScore))
                .ToList();

            // If there are no matching components, do a fuzzy search the other way around:
            if (!result.Any())
            {
                result = gameObject.GetComponents<Component>()
                    .Where(comp => comp != null)
                    .Where(comp => FuzzySearch.FuzzyMatch(comp.GetType().Name, componentName, ref outScore))
                    .Select(comp => new Tuple<Component, long>(comp, outScore))
                    .ToList();
            }

            // If there are still no matching components, do a fuzzy search on the base class:
            if (!result.Any())
            {
                result = gameObject.GetComponents<Component>()
                    .Where(comp => comp != null)
                    .Where(comp => FuzzySearch.FuzzyMatch(comp.GetType().BaseType?.Name, componentName, ref outScore))
                    .Select(comp => new Tuple<Component, long>(comp, outScore))
                    .ToList();
            }

            return result
                .OrderByDescending(a => a.Item2)
                .Select(a => a.Item1);
        }

        internal static IEnumerable<T> FuzzyObjectSearch<T>(string pattern, IEnumerable<T> objectsToSearch)
            where T : Object
        {
            pattern ??= string.Empty;

            pattern = pattern.ToLowerInvariant();

            var objects = objectsToSearch.ToArray();

            var includePathSearch = pattern.Contains("."); // If the search pattern contains a dot, it might be a path.

            // Search by entire string first:
            var results =
                objects
                    .Select(obj =>
                    {
                        long outScore = 0;
                        var isMatch = FuzzySearch.FuzzyMatch(pattern, obj.name, ref outScore);

                        // If the object name does not match, try searching by path:
                        if (includePathSearch && !isMatch)
                        {
                            var path = AssetDatabase.GetAssetPath(obj);
                            if (!string.IsNullOrEmpty(path))
                            {
                                isMatch = FuzzySearch.FuzzyMatch(pattern, path, ref outScore);
                            }
                        }

                        return new { obj, outScore, isMatch };
                    })
                    .Where(x => x.isMatch).ToList();

            // Also search by parts of the string.
            // Separate search pattern to find matches containing parts of the pattern:
            var splitSearchPatterns = pattern.Split(" ");

            var splitSearchResults = splitSearchPatterns
                .SelectMany(splitSearchPattern =>
                    objects
                        .Select(obj =>
                        {
                            long outScore = 0;
                            var isMatch = FuzzySearch.FuzzyMatch(splitSearchPattern, obj.name, ref outScore);
                            return new { obj, outScore, isMatch };
                        })
                        .Where(x => x.isMatch));

            // Add items from splitSearchResults that do not already have an obj in results:
            results.AddRange(splitSearchResults.Where(x => results.All(y => y.obj != x.obj)));

            var finalResult = results
                .OrderByDescending(x => x.outScore)
                .ThenBy(x => x.obj.name.ToLowerInvariant() != pattern) // Prefer objects that have an exact name match
                .Select(x => x.obj);

            return finalResult;
        }

        // When we receive a search for certain strings, they may not be in the asset name but in the extension, for example: "material" would be found if we search for the "mat" extension.
        private static readonly Dictionary<string, string> k_AlternateNameLookup = new() { { "material", "mat" } };

        internal static IEnumerable<string> FuzzySearchAssetsByName(string pattern, IEnumerable<string> namesToSearch)
        {
            long outScore = 0;

            var names = namesToSearch.ToArray();

            var results = DoSearch(pattern);

            if (results.Any())
                return results;

            // If no results were found, try to find an alternative name:
            var splitSearchPatterns = pattern.ToLowerInvariant().Split(" ");

            // Try looking up by alternative names instead:
            foreach (var splitSearchPattern in splitSearchPatterns)
            {
                if (k_AlternateNameLookup.TryGetValue(splitSearchPattern, out var alternativeName))
                {
                    var alternativeSearchPattern = pattern.Replace(splitSearchPattern, alternativeName);

                    results = DoSearch(alternativeSearchPattern);

                    if (results.Any())
                        return results;
                }
            }

            // There are no results, search by parts of the string:
            return splitSearchPatterns
                .SelectMany(splitSearchPattern =>
                    names
                        .Where(name => FuzzySearch.FuzzyMatch(splitSearchPattern,
                            Path.GetFileName(name) + " " + Path.GetExtension(name),
                            ref outScore))
                        .Select(name => new Tuple<string, long>(name, outScore))
                )
                .OrderByDescending(a => a.Item2)
                .Select(t => t.Item1).ToArray();


            string[] DoSearch(string searchPattern)
            {
                return names
                    .Where(name => FuzzySearch.FuzzyMatch(searchPattern,
                        Path.GetFileName(name) + " " + Path.GetExtension(name),
                        ref outScore))
                    .Select(name => new Tuple<string, long>(name, outScore))
                    .OrderByDescending(a => a.Item2)
                    .Select(t => t.Item1).ToArray();
            }
        }
    }
}
