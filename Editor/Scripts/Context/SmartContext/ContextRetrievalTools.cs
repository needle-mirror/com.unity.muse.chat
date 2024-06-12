using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.Muse.Chat.Context.SmartContext
{
    static partial class ContextRetrievalTools
    {
        // Description strings that are used in multiple methods:
        const string k_NameFilter = "Name of the gameObject.";

        [ContextProvider("Returns file structure under the Assets/ folder. It can also be taken as returning " +
                         "all paths from AssetDatabase.GetAllAssetPaths(), separated by \n.")]
        public static string ProjectStructureExtractor()
        {
            var assetPath = Application.dataPath;

            StringBuilder sb = new("Project structure:\n");
            int depth = 1;

            DirectoryInfo directory = new(assetPath);

            ProcessDirectory(directory, depth);
            return sb.ToString();

            void ProcessDirectory(DirectoryInfo dir, int depth)
            {
                foreach (var file in dir.GetFiles())
                {
                    // Ignore meta files:
                    if (file.Extension == ".meta")
                        continue;

                    sb.Append('-', depth);
                    sb.Append(' ');
                    sb.Append(file.Name);
                    sb.Append('\n');
                }

                foreach (var subDir in dir.GetDirectories())
                {
                    sb.Append('-', depth);
                    sb.Append('>');
                    sb.Append(' ');
                    sb.Append(subDir.Name);
                    sb.Append('\n');
                    ProcessDirectory(subDir, depth + 1);
                }
            }
        }

        [ContextProvider("Returns the path of one asset or file under Assets/ folder matching the exact name filter.")]
        public static string AssetDatabasePathExtractor(
            [Parameter("The first asset or file that contains this string in its path is returned.")]
            string assetNameFilter)
        {
            assetNameFilter ??= string.Empty;

            var assetPaths = AssetDatabase.GetAllAssetPaths();
            foreach (var path in assetPaths)
            {
                if (path.Contains(assetNameFilter))
                {
                    return $"Path of asset containing the name '{assetNameFilter}': " + path;
                }
            }

            return string.Empty;
        }

        [ContextProvider("Returns a list of files under Assets/ folder matching the assetNameFilter. It can also be taken as return" +
                         "the names of assets matching AssetDatabase.FindAssets(assetNameFilter). This function can also be used to find all files/assets in certain type.")]
        public static string AssetDatabaseFindAssets(
            [Parameter("The file or asset name filter.")]
            string assetNameFilter)
        {
            var assetPaths = AssetDatabase.FindAssets(assetNameFilter);

            // Get all asset filenames from the GUIDs:
            var assetNames = assetPaths.Select(AssetDatabase.GUIDToAssetPath).Select(Path.GetFileName).ToList();
            if (!assetNames.Any())
            {
                return string.Empty;
            }
            return $"Assets with name filter \"{assetNameFilter}\":\n" + string.Join("\n", assetNames);
        }

        /// <summary>
        /// Stores object hierarchy in a tree structure.
        /// </summary>
        public class HierarchyMapEntry
        {
            public GameObject ObjectRef;
            public List<HierarchyMapEntry> Children = new();

            public HierarchyMapEntry(GameObject obj)
            {
                ObjectRef = obj;
            }

            GameObject GetParent(GameObject obj)
            {
                return obj.transform.parent == null ? null : obj.transform.parent.gameObject;
            }

            public HierarchyMapEntry Insert(GameObject obj)
            {
                // Check if there already is a container for the object, if it exists, do not add it again:
                var childContainer = GetContainerForObject(obj);
                if (childContainer != null)
                {
                    return childContainer;
                }

                // Insert the parent of the object.
                // Note: If the object has no parent, we are inserting null,
                // this will match the root container so the parentContainer
                // will be the root and the object will be inserted in the children of that.
                var parentContainer = Insert(GetParent(obj));

                // Add child to the parent container's children:
                childContainer = new HierarchyMapEntry(obj);
                parentContainer.Children.Add(childContainer);

                return childContainer;
            }

            HierarchyMapEntry GetContainerForObject(GameObject obj)
            {
                if (ObjectRef == obj)
                {
                    return this;
                }

                foreach (var childMap in Children)
                {
                    var map = childMap.GetContainerForObject(obj);
                    if (map != null)
                    {
                        return map;
                    }
                }

                return null;
            }

            string Serialized(int depth)
            {
                var sb = new StringBuilder();
                if (ObjectRef != null)
                {
                    sb.Append('-', depth);
                    sb.Append(' ');
                    sb.Append(ObjectRef.name);
                    sb.Append('\n');
                }

                foreach (var child in Children)
                {
                    sb.Append(child.Serialized(depth + 1));
                }

                return sb.ToString();
            }

            public string Serialized()
            {
                return Serialized(0);
            }
        }

        [ContextProvider(
            "Returns the hierarchy of gameObjects in the scene matching the given name filter. E.g:\n- A\n-- B\n--- C\n Where A is the parent of B and B is the parent of C.")]
        public static string SceneHierarchyExtractor(
            [Parameter(
                "Name filter of the gameObjects to extract the hierarchy for. Only objects matching the exact name are returned.")]
            params string[] gameObjectNameFilters)
        {
            // Store all objects in a tree structure first, then serialize it:
            var hierarchyMap = new HierarchyMapEntry(null);

            // Get all gameObjects:
            var allObjects = Object.FindObjectsOfType<GameObject>();

            // Loop through all GameObjects and if their names are in the list of args, add them to the hierarchy map:
            if (gameObjectNameFilters == null || gameObjectNameFilters.Length == 0)
            {
                foreach (var obj in allObjects)
                    hierarchyMap.Insert(obj);
            }
            else
            {
                foreach (var filter in gameObjectNameFilters)
                {
                    foreach (var obj in ContextRetrievalHelpers.FuzzyGameObjectSearch(filter, allObjects))
                    {
                        hierarchyMap.Insert(obj);
                    }
                }
            }

            var sceneHierarchy = hierarchyMap.Serialized();
            if (string.IsNullOrEmpty(sceneHierarchy))
            {
                return string.Empty;
            }
            return "Scene hierarchy:\n" + sceneHierarchy;
        }

        private static string GetComponents(GameObject gameObject)
        {
            return $"GameObject {gameObject.name} has the following components attached to it: " + string.Join("\n",
                gameObject.GetComponents<Component>().Select(c => c.GetType().Name));
        }


        [ContextProvider("Returns a list of component attached to the game object, no component property" +
                         "or serialization data included.")]
        public static string GameObjectComponentListExtractor(
            [Parameter(k_NameFilter)] string gameObjectName,
            [Parameter("Whether the gameObject is a prefab or an object in the scene.")]
            bool isPrefab)
        {
            var gameObject = ContextRetrievalHelpers.FindGameObject(gameObjectName, isPrefab);

            if (gameObject != null)
            {
                return GetComponents(gameObject);
            }

            return string.Empty;
        }

        [ContextProvider(
            "Returns the serialized data of the given component on the given game object. Game object is specified by its name.")]
        public static string GameObjectComponentDataExtractor(
            [Parameter(k_NameFilter)] string gameObjectName,
            [Parameter("Name of the component.")] string componentFilter,
            [Parameter("Whether the gameObject is a prefab or an object in the scene.")]
            bool isPrefab)
        {
            var gameObject = ContextRetrievalHelpers.FindGameObject(gameObjectName, isPrefab);

            if (gameObject != null)
            {
                // Find matching component:
                var component = gameObject.GetComponent(componentFilter);

                if (component != null)
                {
                    return UnityDataUtils.OutputUnityObject(component, true, false, 0);
                }

                return string.Empty;
            }

            return string.Empty;
        }

        [ContextProvider("Returns the serialized data of the game object.")]
        public static string GameObjectExtractor(
            [Parameter(k_NameFilter)] string gameObjectName,
            [Parameter("Whether the gameObject is a prefab or an object in the scene.")]
            bool isPrefab)
        {
            var gameObject = ContextRetrievalHelpers.FindGameObject(gameObjectName, isPrefab);

            if (gameObject != null)
            {
                return UnityDataUtils.OutputUnityObject(gameObject, true, false, 0);
            }

            return string.Empty;
        }
    }
}
