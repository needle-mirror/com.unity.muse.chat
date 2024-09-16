using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat.Serialization.BuiltinOverrides
{
    // This class is used to remove potentially overly large arrays with animation curves and binding information.
    static class MaterialOverrides
    {
        const string k_EmptyArrayString = "[]";

        [SerializationOverride(typeof(Material), "m_Shader")]
        static string Shader(SerializedProperty property) => k_EmptyArrayString;
    }
}
