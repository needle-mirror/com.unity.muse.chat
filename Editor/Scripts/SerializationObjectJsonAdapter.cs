using UnityEditor;
using Unity.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Chat.Serialization;
using UnityEngine;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Helper class to serialize any kind of Unity Object into a JSON string
    /// This is for ingestion/human and machine readability instead of file save/load
    /// </summary>
    class SerializationObjectJsonAdapter : IJsonAdapter<SerializedObject>
    {
        int m_ObjectDepth = 0;
        int m_MaxObjectDepth = -1;
        int m_CurrentDepth;
        Stack<int> m_Depths = new Stack<int>();
        HashSet<long> m_VisitedObjects = new HashSet<long>();
        HashSet<long> m_VisitedNodes = new HashSet<long>();

        /// <summary>
        /// If the type of the variable should be included when writing the key, which is otherwise just the name of the object
        /// </summary>
        public bool OutputType { get; set; } = false;

        /// <summary>
        /// If a tooltip related to the variable should be included when writing the key
        /// </summary>
        public bool OutputTooltip { get; set; } = false;

        public bool OutputDirectory { get; set; } = false;

        public int MaxObjectDepth { get => m_MaxObjectDepth; set => m_MaxObjectDepth = value; }

        public string[] RootParameters { get; set; }

        public SerializedObject RootObject { get; set; }
        public bool UseDisplayName { get; set; }

        public bool IgnorePrefabInstance { get; set; }

        public ISerializationOverrideProvider OverrideProvider { get; set; }

        /// <summary>
        /// Returns the key for the top level object
        /// </summary>
        /// <param name="value">A serialized object to retrieve the name from</param>
        /// <returns>The name for the serialized object, which includes the type and tooltip</returns>
        public string GetObjectKey(SerializedObject value)
        {
            var name = OutputType
                ? $"{value.targetObject.name} \n- Type: {value.targetObject.GetType().Name}"
                : value.targetObject.name;
            var directory = AssetDatabase.GetAssetPath(value.targetObject);
            return string.IsNullOrEmpty(directory) || !OutputDirectory ? name : $"{name} \n- Path: {directory}";
        }

        void IJsonAdapter<SerializedObject>.Serialize(in JsonSerializationContext<SerializedObject> context, SerializedObject value)
        {
            m_VisitedObjects.Add(value.targetObject.GetHashCode());

            // get iter
            // perform function on it
            // Each iter that has children makes new object scope
            // Go back to pure SO adapter
            var iter = value.GetIterator();
            iter.Next(true);
            m_CurrentDepth = iter.depth;

            m_ObjectDepth++;

            ProcessSerializedProperty(context, iter, true);

            m_ObjectDepth--;

            if (m_ObjectDepth == 0)
            {
                m_VisitedNodes.Clear();
                m_VisitedObjects.Clear();
            }
        }

        SerializedObject IJsonAdapter<SerializedObject>.Deserialize(in JsonDeserializationContext<SerializedObject> context)
        {
            throw new System.NotImplementedException();
        }

        void ProcessSerializedProperty(in JsonSerializationContext<SerializedObject> context, SerializedProperty current, bool newObject)
        {
            var writer = context.Writer;

            // Check if property has children
            // If it does, create new object scope
            // Go through children writing keys and values
            // If the child is a generic type, we'll end up back here

            if (newObject)
            {
                using (writer.WriteObjectScope())
                {
                    do
                    {
                        if(Validate(current))
                            ProcessSerializedPropertyInner(context, current, true);
                    } while (current.Next(false));
                }
            }
            else
            {
                do
                {
                    if(Validate(current))
                        ProcessSerializedPropertyInner(context, current, true);
                } while (current.Next(false));
            }

            return;

            bool Validate(SerializedProperty property) =>
                RootObject != property.serializedObject || RootParameters is null
                || RootParameters.Contains(property.name);
        }

        void ProcessSerializedPropertyInner(in JsonSerializationContext<SerializedObject> context, SerializedProperty current, bool writeKey = false)
        {
            if (current.depth < m_CurrentDepth)
            {
                return;
            }

            if (current.propertyType == SerializedPropertyType.ManagedReference && m_VisitedNodes.Contains(current.managedReferenceId))
            {
                return;
            }

            if (current.name == "m_PrefabInstance" && IgnorePrefabInstance)
            {
                return;
            }

            var writer = context.Writer;
            if (writeKey)
            {
                var key = UseDisplayName ? current.displayName : current.name;
                var type = current.propertyType.ToString();

                if (current.propertyType == SerializedPropertyType.Generic && current.isArray)
                {
                    type = $"Array({PrettifyString(current.arrayElementType)})";
                }
                if (current.propertyType == SerializedPropertyType.ObjectReference || current.propertyType == SerializedPropertyType.ExposedReference)
                {
                    if (current.objectReferenceValue != null)
                    {
                        type = current.objectReferenceValue.GetType().Name;
                    }
                    else
                    {
                        type = PrettifyString(current.type);
                    }
                }

                if (OutputType)
                    key += $" - {type}";

                if (OutputTooltip && !string.IsNullOrEmpty(current.tooltip))
                    key += $" - {current.tooltip}";

                // Override for GameObject's component list
                if (type == "Array(ComponentPair)")
                    key = "Components";

                writer.WriteKey(key);
            }

            m_CurrentDepth++;

            var serializationOverride = OverrideProvider.Find(
                current.serializedObject.targetObject.GetType().FullName, current.name);

            if (serializationOverride is not null)
            {
                var value = serializationOverride.Override(current);
                switch (value)
                {
                    case bool b:
                        writer.WriteValue(b);
                        break;
                    case char c:
                        writer.WriteValue(c);
                        break;
                    case double d:
                        SafeNumberWrite(writer, d);
                        break;
                    case float f:
                        SafeNumberWrite(writer, f);
                        break;
                    case int i:
                        writer.WriteValue(i);
                        break;
                    case long l:
                        writer.WriteValue(l);
                        break;
                    case string s:
                        writer.WriteValue(s);
                        break;
                    case uint ui:
                        writer.WriteValue(ui);
                        break;
                    case ulong ul:
                        writer.WriteValue(ul);
                        break;
                }
            }
            else
            {
                switch (current.propertyType)
                {
                    case SerializedPropertyType.Generic:
                    {
                        if (current.isArray)
                        {
                            using (writer.WriteArrayScope())
                            {
                                var length = current.arraySize;
                                for (var i = 0; i < length; i++)
                                {
                                    var arrayElement = current.GetArrayElementAtIndex(i);
                                    ProcessSerializedPropertyInner(context, arrayElement);
                                }
                            }
                        }
                        else
                        {
                            if (current.hasChildren)
                            {
                                var childProp = current.Copy();
                                childProp.Next(true);
                                ProcessSerializedProperty(context, childProp, true);
                            }
                            else
                                writer.WriteValue("Generic no children");
                        }
                    }
                        break;
                    case SerializedPropertyType.Integer:
                        writer.WriteValue(current.intValue);
                        break;
                    case SerializedPropertyType.Boolean:
                        writer.WriteValue(current.boolValue);
                        break;
                    case SerializedPropertyType.Float:
                        SafeNumberWrite(writer, current.floatValue);
                        break;
                    case SerializedPropertyType.String:
                        writer.WriteValue(current.stringValue);
                        break;
                    case SerializedPropertyType.Color:
                        writer.WriteValue(current.colorValue.ToString());
                        break;
                    case SerializedPropertyType.ObjectReference:
                    {
                        var objectReference = current.objectReferenceValue;
                        if (objectReference != null)
                        {
                            var instanceID = objectReference.GetInstanceID();
                            if (!m_VisitedObjects.Contains(instanceID))
                            {
                                if (m_MaxObjectDepth > -1 && m_ObjectDepth > m_MaxObjectDepth)
                                {
                                    context.SerializeValue($"{objectReference.name}");
                                }
                                else
                                {
                                    m_Depths.Push(m_CurrentDepth);
                                    var SO = new SerializedObject(objectReference);
                                    context.SerializeValue(SO);
                                    m_CurrentDepth = m_Depths.Pop();
                                }
                            }
                            else
                            {

                                context.SerializeValue(
                                    $"Already serialized - {objectReference.name}");
                            }
                        }
                        else
                        {
                            writer.WriteValue("null");
                        }
                    }
                        break;
                    case SerializedPropertyType.LayerMask:
                        writer.WriteValue(current.intValue);
                        break;
                    case SerializedPropertyType.Enum:
                    {
                        if (current.enumValueIndex >= 0 && current.enumValueIndex < current.enumDisplayNames.Length)
                        {
                            writer.WriteValue(current.enumDisplayNames[current.enumValueIndex]);
                        }
                        else
                        {
                            writer.WriteValue(current.enumValueFlag);
                        }
                    }
                        break;
                    case SerializedPropertyType.Vector2:
                        writer.WriteValue(current.vector2Value.ToString());
                        break;
                    case SerializedPropertyType.Vector3:
                        writer.WriteValue(current.vector3Value.ToString());
                        break;
                    case SerializedPropertyType.Vector4:
                        writer.WriteValue(current.vector4Value.ToString());
                        break;
                    case SerializedPropertyType.Rect:
                        writer.WriteValue(current.rectValue.ToString());
                        break;
                    case SerializedPropertyType.ArraySize:
                        writer.WriteValue(current.intValue);
                        break;
                    case SerializedPropertyType.Character:
                        // Todo: Better support
                        writer.WriteValue($"Character - {current.boxedValue}");
                        break;
                    case SerializedPropertyType.AnimationCurve:
                        // Todo: Better support
                        writer.WriteValue($"Animation curve - {current.animationCurveValue}");
                        break;
                    case SerializedPropertyType.Bounds:
                        writer.WriteValue($"{current.boundsValue}");
                        break;
                    case SerializedPropertyType.Gradient:
                        // Todo: Better support
                        writer.WriteValue($"Gradient - {current.gradientValue}");
                        break;
                    case SerializedPropertyType.Quaternion:
                        writer.WriteValue(current.quaternionValue.ToString());
                        break;
                    case SerializedPropertyType.ExposedReference:
                    {
                        var objectReference = current.objectReferenceValue;
                        if (objectReference != null)
                        {
                            var instanceID = objectReference.GetInstanceID();
                            if (!m_VisitedObjects.Contains(instanceID))
                            {
                                if (m_MaxObjectDepth > -1 && m_ObjectDepth > m_MaxObjectDepth)
                                {
                                    context.SerializeValue($"{objectReference.name}");
                                }
                                else
                                {
                                    m_Depths.Push(m_CurrentDepth);
                                    var SO = new SerializedObject(objectReference);
                                    context.SerializeValue(SO);
                                    m_CurrentDepth = m_Depths.Pop();
                                }
                            }
                            else
                            {
                                context.SerializeValue(
                                    $"Already serialized  - {objectReference.name}");
                            }
                        }
                        else
                        {
                            writer.WriteValue("null");
                        }
                    }
                        break;
                    case SerializedPropertyType.FixedBufferSize:
                        writer.WriteValue(current.intValue);
                        break;
                    case SerializedPropertyType.Vector2Int:
                        writer.WriteValue(current.vector2IntValue.ToString());
                        break;
                    case SerializedPropertyType.Vector3Int:
                        writer.WriteValue(current.vector3IntValue.ToString());
                        break;
                    case SerializedPropertyType.RectInt:
                        writer.WriteValue(current.rectIntValue.ToString());
                        break;
                    case SerializedPropertyType.BoundsInt:
                        writer.WriteValue(current.boundsIntValue.ToString());
                        break;
                    case SerializedPropertyType.ManagedReference:
                    {
                        var refId = current.managedReferenceId;
                        var visited = false;

                        if (!m_VisitedNodes.Contains(refId))
                        {
                            m_VisitedNodes.Add(current.managedReferenceId);
                            if (current.hasChildren)
                            {
                                visited = true;
                                var childProp = current.Copy();
                                childProp.Next(true);
                                ProcessSerializedProperty(context, childProp, true);
                            }
                        }

                        if (!visited)
                        {
                            var boxedValue = current.boxedValue;
                            writer.WriteValue($"Managed reference ID: {boxedValue}");
                        }

                    }
                        break;
                    case SerializedPropertyType.Hash128:
                        writer.WriteValue(current.hash128Value.ToString());
                        break;
                    default:
                        writer.WriteValue($"unsupported - {current.propertyType}");
                        break;
                }
            }

            m_CurrentDepth--;
        }

        static void SafeNumberWrite(JsonWriter writer, float value)
        {
            if (float.IsFinite(value))
                writer.WriteValue(value);
            else
                writer.WriteValue(value.ToString());
        }
        static void SafeNumberWrite(JsonWriter writer, double value)
        {
            if (double.IsFinite(value))
                writer.WriteValue(value);
            else
                writer.WriteValue(value.ToString());
        }

        static string PrettifyString(string toPrettify)
        {
            if (toPrettify.StartsWith("PPtr<"))
                return  toPrettify.Substring(5, toPrettify.Length - 6);

            return toPrettify;
        }
    }
}
