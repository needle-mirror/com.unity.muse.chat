using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;
using Unity.Muse.Common.Editor.Integration;
using UnityEngine.SceneManagement;

namespace Unity.Muse.Chat.Context.SmartContext
{
    internal static partial class ContextRetrievalTools
    {
        internal interface IParentable<T>
        {
            T Parent { get; }
        }

        /// <summary>
        /// Stores object hierarchy in a tree structure.
        /// </summary>
        internal abstract class HierarchyMapEntry<T> where T : IParentable<T>
        {
            internal static int SmartContextLimit { get; set; }

            internal static int EstimatedSerializedLength { get; set; }

            internal static void Reset()
            {
                EstimatedSerializedLength = 0;
            }

            protected readonly T k_ObjectRef;
            public readonly List<HierarchyMapEntry<T>> Children = new();

            public abstract string ObjectName { get; }

            protected HierarchyMapEntry(T obj)
            {
                k_ObjectRef = obj;
            }

            protected abstract HierarchyMapEntry<T> CreateInstance(T obj, HierarchyMapEntry<T> parent);

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

            protected virtual bool Matches(T obj)
            {
                if (obj == null && ObjectName == null)
                {
                    return true;
                }

                return k_ObjectRef != null && k_ObjectRef.Equals(obj);
            }

            HierarchyMapEntry<T> InsertHere(T obj, int depth)
            {
                if (Matches(obj))
                {
                    return this;
                }

                foreach (var childMap in Children)
                {
                    if (childMap.Matches(obj))
                    {
                        return childMap;
                    }
                }

                var container = CreateInstance(obj, this);
                Children.Add(container);
                EstimatedSerializedLength +=
                    container.ObjectName.Length + depth + 1 +
                    2; // depth+1 for `-` prefix, 1 for space and 2 for line break

                return container;
            }

            readonly List<T> m_ParentsList = new();

            public void Insert(T obj)
            {
                // Build a hierarchy of parents:
                m_ParentsList.Clear();

                var parent = obj;
                m_ParentsList.Add(obj);
                while (true)
                {
                    parent = parent.Parent;
                    if (parent == null)
                    {
                        break;
                    }

                    m_ParentsList.Add(parent);
                }

                // Now we know all parents, insert the parents starting at the top of the hierarchy and then the given object:
                var parentEntry = this;
                for (int i = m_ParentsList.Count - 1; i >= 0; i--)
                {
                    parentEntry = parentEntry.InsertHere(m_ParentsList[i], m_ParentsList.Count - i - 1);
                }
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

                // After pruning, there may be duplicates because of truncated children, try a final collapse and serialize again if needed:
                if (Collapse())
                {
                    sb.Clear();
                    Serialize(sb, 0);
                }
            }

            /// <summary>
            /// Remove duplicates entries in the hierarchy.
            /// </summary>
            protected virtual bool Collapse()
            {
                return false;
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

            public string Serialized()
            {
                var sb = new StringBuilder();

                PruneAndSerialize(sb);

                return sb.ToString();
            }
        }

        internal class GameObjectInfo : IParentable<GameObjectInfo>
        {
            public readonly GameObject Object;
            public GameObjectInfo Parent { get; }
            public readonly int SceneObjectCount;

            public readonly string Name;

            internal GameObjectInfo(GameObject obj, Scene scene = default)
            {
                Object = obj;

                if (obj != null)
                {
                    Name = obj.name;
                }
                else if (scene.IsValid())
                {
                    Name = $"{scene.name} (This is a SubScene)";
                    SceneObjectCount = scene.GetRootGameObjects().Length;
                }

                if (obj == null)
                    return;

                if (obj.transform.parent == null)
                {
                    // Check if obj is in the current scene or a sub scene:
                    if (obj.scene != SceneManager.GetActiveScene())
                    {
                        Parent = new GameObjectInfo(null, obj.scene);
                    }
                }
                else
                {
                    Parent = new GameObjectInfo(obj.transform.parent?.gameObject);
                }
            }
        }

        internal class GameObjectHierarchyMapEntry : HierarchyMapEntry<GameObjectInfo>
        {
            readonly HashSet<GameObject> m_Copies = new(1);

            private GameObject Original { get; }

            private readonly GameObjectHierarchyMapEntry m_Parent;

            public GameObjectHierarchyMapEntry() : this(null, null)
            {
            }

            private GameObjectHierarchyMapEntry(GameObjectInfo obj, GameObjectHierarchyMapEntry parent) : base(obj)
            {
                m_Parent = parent;
                m_Copies.Add(obj?.Object);
                Original = obj?.Object;
            }

            public override bool Equals(object obj)
            {
                if (obj is not GameObjectHierarchyMapEntry other)
                {
                    return false;
                }

                return k_ObjectRef?.Name == other.k_ObjectRef?.Name && TruncatedChildCount > 0 &&
                       other.TruncatedChildCount > 0;
            }

            private bool Equals(GameObjectHierarchyMapEntry other)
            {
                return base.Equals(other) && Equals(m_Copies, other.m_Copies) && Equals(m_Parent, other.m_Parent) && Equals(Original, other.Original);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(base.GetHashCode(), m_Copies, m_Parent, Original);
            }

            public override string ObjectName
            {
                get
                {
                    var result = k_ObjectRef?.Name;

                    if (m_Copies.Count > 1)
                    {
                        result += $" - There are {m_Copies.Count} objects with this name";
                    }

                    // Check for truncated children:
                    var truncatedChildren = TruncatedChildCount;
                    if (truncatedChildren > 0)
                    {
                        result += " - There are truncated children";
                    }

                    return result;
                }
            }

            private int TruncatedChildCount
            {
                get
                {
                    if (k_ObjectRef == null)
                    {
                        return -1;
                    }

                    if (!k_ObjectRef.Object)
                    {
                        return k_ObjectRef.SceneObjectCount - Children.Count;
                    }

                    var childCopyCount = Children.Sum(child => ((GameObjectHierarchyMapEntry)child).m_Copies.Count);

                    return k_ObjectRef.Object.transform.childCount - childCopyCount;
                }
            }

            protected override HierarchyMapEntry<GameObjectInfo> CreateInstance(GameObjectInfo obj,
                HierarchyMapEntry<GameObjectInfo> parent)
            {
                return new GameObjectHierarchyMapEntry(obj, parent as GameObjectHierarchyMapEntry);
            }

            static bool DoNamesMatch(Transform a, Transform b)
            {
                return a?.name.GetHashCode() == b?.name.GetHashCode();
            }

            protected override bool Matches(GameObjectInfo obj)
            {
                // If either of the object infos has no game object, check if both have none and their names match:
                if (k_ObjectRef?.Object == null || obj.Object == null)
                {
                    return k_ObjectRef?.Object == null && obj.Object == null && k_ObjectRef?.Name == obj.Name;
                }

                if (obj == k_ObjectRef || obj.Object == k_ObjectRef?.Object)
                {
                    return true;
                }

                if (DoNamesMatch(k_ObjectRef.Object.transform, obj.Object.transform))
                {
                    if (DoChildrenMatch(k_ObjectRef.Object.transform, obj.Object.transform))
                    {
                        // Only add the object to the copies list if the parent is the first copy of the object:
                        if (m_Parent == null || m_Parent.Original == obj.Object.transform.parent?.gameObject)
                        {
                            m_Copies.Add(obj.Object);
                        }

                        return true;
                    }

                    return false;

                    static bool DoChildrenMatch(Transform a, Transform b)
                    {
                        if (a.childCount != b.childCount)
                        {
                            return false;
                        }

                        if (!DoNamesMatch(a, b))
                        {
                            return false;
                        }

                        for (int childIdx = 0; childIdx < a.transform.childCount; childIdx++)
                        {
                            if (!DoChildrenMatch(a.GetChild(childIdx), b.GetChild(childIdx)))
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                }

                return false;
            }

            protected override bool Collapse()
            {
                bool collapsed = false;

                // Remove children on the same level that have the same name and child hierarchy:
                for (var i = 0; i < Children.Count; i++)
                {
                    var child = (GameObjectHierarchyMapEntry)Children[i];
                    collapsed |= child.Collapse();

                    for (var j = i + 1; j < Children.Count; j++)
                    {
                        var otherChild = (GameObjectHierarchyMapEntry)Children[j];
                        collapsed |= otherChild.Collapse();

                        if (child.Equals(otherChild))
                        {
                            Children.RemoveAt(j);
                            j--;
                            collapsed = true;

                            child.m_Copies.UnionWith(otherChild.m_Copies);
                        }
                    }
                }

                return collapsed;
            }
        }

        [ContextProvider(
            "Returns the hierarchy of gameObjects in the scene matching the given name filter." +
            "If no name filter is provided, all gameObjects in the scene are returned.")]
        internal static SmartContextToolbox.ExtractedContext SceneHierarchyExtractor(
            [Parameter(
                "Filters to specify which gameObjects' hierarchies to return. Use an empty list if the full scene hierarchy is needed.")]
            params string[] gameObjectNameFilters)
        {
            GameObjectHierarchyMapEntry.SmartContextLimit = SmartContextToolbox.SmartContextLimit;

            // Store all objects in a tree structure first, then serialize it:
            var hierarchyMap = new GameObjectHierarchyMapEntry();

            // Get all gameObjects:
            var allObjects = Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.InstanceID);

            // Loop through all GameObjects and if their names are in the list of args, add them to the hierarchy map:
            ICollection<GameObject> objectsToSearch;

            GameObjectHierarchyMapEntry.Reset();

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

            // Make sort faster by caching depth instead of recalculating every time:
            Dictionary<GameObject, int> depthCache = new();
            objectsToSearch = new List<GameObject>(objectsToSearch.OrderBy(GetDepth));

            foreach (var obj in objectsToSearch)
            {
                hierarchyMap.Insert(new GameObjectInfo(obj));

                if (GameObjectHierarchyMapEntry.EstimatedSerializedLength >
                    GameObjectHierarchyMapEntry.SmartContextLimit)
                {
                    break;
                }
            }

            if (hierarchyMap.Children.Count == 0)
            {
                return null;
            }

            return new SmartContextToolbox.ExtractedContext
            {
                Payload = hierarchyMap.Serialized(),
                ContextType = "scene hierarchy"
            };

            // Sort objects by their depth in the hierarchy:
            int GetDepth(GameObject obj)
            {
                if (obj == null)
                {
                    return 0;
                }

                if (!depthCache.TryGetValue(obj, out var depth))
                {
                    depth = GetDepth(obj.transform.parent?.gameObject) + 1;
                    depthCache[obj] = depth;
                }

                return depth;
            }
        }
    }
}
