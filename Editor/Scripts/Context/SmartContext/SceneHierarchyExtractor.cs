using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using Unity.Muse.Common.Editor.Integration;

namespace Unity.Muse.Chat.Context.SmartContext
{
    internal static partial class ContextRetrievalTools
    {
        /// <summary>
        /// Stores object hierarchy in a tree structure.
        /// </summary>
        internal abstract class HierarchyMapEntry<T>
        {
            internal static int SmartContextLimit { get; set; }

            protected abstract T GetParent(T obj);
            protected abstract string GetName(T obj);
            protected virtual bool NeedsCollapsing => true;

            private readonly T k_ObjectRef;
            public readonly List<HierarchyMapEntry<T>> Children = new();

            public string ObjectName
            {
                get
                {
                    var result = k_ObjectRef != null ? GetName(k_ObjectRef) : null;

                    if (result != null && m_Suffix != null)
                    {
                        result += m_Suffix;
                    }

                    return result;
                }
            }

            private string m_Suffix;

            protected HierarchyMapEntry(T obj)
            {
                k_ObjectRef = obj;
            }

            protected abstract HierarchyMapEntry<T> CreateInstance(T obj);

            public override bool Equals(object obj)
            {
                if (obj is not HierarchyMapEntry<T> other)
                    return false;

                if (ObjectName != other.ObjectName)
                    return false;

                if (Children.Count != other.Children.Count)
                    return false;

                for (var i = 0; i < Children.Count; i++)
                {
                    var child1 = Children[i];
                    var child2 = other.Children[i];
                    if (!child1.Equals(child2))
                        return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(k_ObjectRef, Children);
            }

            public HierarchyMapEntry<T> Insert(T obj)
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
                childContainer = CreateInstance(obj);

                parentContainer.Children.Add(childContainer);

                return childContainer;
            }

            private HierarchyMapEntry<T> GetContainerForObject(T obj)
            {
                if (obj == null && ObjectName == null)
                {
                    return this;
                }

                HierarchyMapEntry<T> result;
                if (k_ObjectRef != null && k_ObjectRef.Equals(obj))
                {
                    result = this;
                }
                else
                {
                    result = null;
                    foreach (var childMap in Children)
                    {
                        var map = childMap.GetContainerForObject(obj);
                        if (map != null)
                        {
                            result = map;
                            break;
                        }
                    }
                }

                return result;
            }

            private void Serialize(StringBuilder sb, int depth)
            {
                if (!string.IsNullOrEmpty(ObjectName))
                {
                    sb.Append('-', depth);
                    sb.Append(' ');
                    sb.Append(ObjectName);
                    sb.Append('\n');
                }

                Children.Sort((a, b) => string.Compare(a.ObjectName, b.ObjectName, StringComparison.Ordinal));

                foreach (var child in Children)
                {
                    child.Serialize(sb, depth + 1);
                }
            }

            private void PruneAndSerialize(StringBuilder sb)
            {
                while (true)
                {
                    sb.Clear();

                    // Check how long the resulting string would be and remove children if it's too long:
                    Serialize(sb, 0);
                    if (sb.Length > 0 && sb.Length > SmartContextLimit)
                    {
                        var prunedChildren = false;
                        Prune(ref prunedChildren, 1, GetDepth(0));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            public int GetDepth(int depth)
            {
                return Children.Select(child => child.GetDepth(depth + 1)).Prepend(depth).Max();
            }

            private void Prune(ref bool prunedChildren, int depth, int pruneDepth)
            {
                // Find first child node at pruneDepth that has no children and remove it:
                for (var i = 0; i < Children.Count; i++)
                {
                    var child = Children[i];

                    child.Prune(ref prunedChildren, depth + 1, pruneDepth);

                    if (depth >= pruneDepth)
                    {
                        if (child.Children.Count == 0)
                        {
                            Children.RemoveAt(i);
                            prunedChildren = true;
                        }
                    }

                    if (prunedChildren)
                    {
                        break;
                    }
                }
            }

            /// <summary>
            /// Remove duplicates entries in the hierarchy.
            /// </summary>
            private void Collapse()
            {
                if (!NeedsCollapsing)
                {
                    return;
                }

                // Remove children on the same level that have the same name and child hierarchy:
                for (var i = 0; i < Children.Count; i++)
                {
                    var child = Children[i];
                    child.Collapse();

                    int collapsedCount = 0;
                    for (var j = i + 1; j < Children.Count; j++)
                    {
                        var otherChild = Children[j];
                        otherChild.Collapse();

                        if (child.Equals(otherChild))
                        {
                            Children.RemoveAt(j);
                            j--;
                            collapsedCount++;
                        }
                    }

                    if (collapsedCount > 0)
                    {
                        child.m_Suffix = $" - There are {collapsedCount + 1} objects with this name";
                    }
                }
            }

            public string Serialized()
            {
                Collapse();

                var sb = new StringBuilder();
                PruneAndSerialize(sb);

                return sb.ToString();
            }
        }

        internal class GameObjectHierarchyMapEntry : HierarchyMapEntry<GameObject>
        {
            public GameObjectHierarchyMapEntry(GameObject obj) : base(obj)
            {
            }

            protected override GameObject GetParent(GameObject obj)
            {
                return obj.transform.parent == null ? null : obj.transform.parent.gameObject;
            }

            protected override string GetName(GameObject obj)
            {
                return obj.name;
            }

            protected override HierarchyMapEntry<GameObject> CreateInstance(GameObject obj)
            {
                return new GameObjectHierarchyMapEntry(obj);
            }
        }

        [ContextProvider(
            "Returns the hierarchy of gameObjects in the scene matching the given name filter." +
            "If no name filter is provided, all gameObjects in the scene are returned.")]
        internal static string SceneHierarchyExtractor(
            [Parameter(
                "Filters to specify which gameObjects' hierarchies to return.")]
            params string[] gameObjectNameFilters)
        {
            const string resultPrefix = "Scene hierarchy:\n";
            GameObjectHierarchyMapEntry.SmartContextLimit = SmartContextToolbox.SmartContextLimit - resultPrefix.Length;

            // Store all objects in a tree structure first, then serialize it:
            var hierarchyMap = new GameObjectHierarchyMapEntry(null);

            // Get all gameObjects:
            var allObjects = Object.FindObjectsOfType<GameObject>(true);

            // Loop through all GameObjects and if their names are in the list of args, add them to the hierarchy map:
            ICollection<GameObject> objectsToSearch;

            if (gameObjectNameFilters == null || gameObjectNameFilters.Length == 0 || gameObjectNameFilters[0] == "*")
            {
                objectsToSearch = allObjects;
            }
            else
            {
                objectsToSearch = new HashSet<GameObject>();
                foreach (var filter in gameObjectNameFilters)
                {
                    foreach (var obj in ContextRetrievalHelpers.FuzzyObjectSearch(filter, allObjects))
                    {
                        objectsToSearch.Add(obj);
                    }
                }
            }

            objectsToSearch = new List<GameObject>(objectsToSearch.OrderBy(GetDepth));

            var resultLength = 0;
            foreach (var obj in objectsToSearch)
            {
                hierarchyMap.Insert(obj);
                resultLength += obj.name.Length;

                if (resultLength > GameObjectHierarchyMapEntry.SmartContextLimit)
                {
                    break;
                }
            }

            if (hierarchyMap.Children.Count == 0)
            {
                return "";
            }

            return resultPrefix + hierarchyMap.Serialized();

            // Sort objects by their depth in the hierarchy:
            int GetDepth(GameObject obj)
            {
                if (obj.transform.parent == null)
                {
                    return 0;
                }

                return GetDepth(obj.transform.parent.gameObject) + 1;
            }
        }
    }
}
