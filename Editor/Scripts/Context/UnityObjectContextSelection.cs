using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Allows a Unity object or asset to be sent to the LLM for evaluation
    /// </summary>
    internal class UnityObjectContextSelection : IContextSelection
    {
        Object m_Target;

        static readonly List<string> k_ExtensionsToExtract = new() { ".cs", ".json",  ".shader" };

        public void SetTarget(Object target)
        {
            m_Target = target;
        }

        string IContextSelection.Classifier
        {
            get
            {
                if (m_Target == null)
                    return "Null";

                // We might want to special path for gameobjects to include all their components
                return $"UnityEngine.Object, {m_Target.GetType().Name}";
            }
        }

        string IContextSelection.Description
        {
            get
            {
                if (m_Target == null)
                    return "No object selected";

                return $"{m_Target.name} - {m_Target.GetType().Name}";
            }
        }

        string IContextSelection.Payload
        {
            get
            {
                if (m_Target == null)
                    return null;

                string path = AssetDatabase.GetAssetPath(m_Target);
                string fileContents = null;
                if (k_ExtensionsToExtract.Contains(Path.GetExtension(path)))
                {
                    fileContents = File.ReadAllText(path);
                }
                else if (m_Target is MonoBehaviour mono)
                {
                    var monoScript = MonoScript.FromMonoBehaviour(mono);
                    fileContents = monoScript.text;
                }

                if (fileContents != null)
                {
                    return $"\n{UnityDataUtils.OutputUnityObject(m_Target, true, false, 1, outputDirectory: true)}" +
                           $"\n\nFile contents:\"\n{fileContents}\"";
                }

                return $"\n{UnityDataUtils.OutputUnityObject(m_Target, true, false, 1, outputDirectory: true)}";
            }
        }

        string IContextSelection.DownsizedPayload
        {
            get
            {
                if (m_Target == null)
                    return null;

                return $"\n{UnityDataUtils.OutputUnityObject(m_Target, true, false, 0)}";
            }
        }

        string IContextSelection.ContextType
        {
            get
            {
                if (m_Target == null)
                    return null;

                string path = AssetDatabase.GetAssetPath(m_Target);
                if (path.EndsWith(".cs"))
                {
                    return "source code of c# script and its serialization data in json format";
                }

                if (path.EndsWith(".json"))
                {
                    return "content of json file and its serialization data in json format";
                }
                return $"{m_Target.GetType()} object serialization data in json format";
            }
        }

        string IContextSelection.TargetName => $"{m_Target.name}";

        bool System.IEquatable<IContextSelection>.Equals(IContextSelection other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (this == null || other == null)
                return false;

            if (other is not UnityObjectContextSelection otherSelection)
                return false;

            var asObjectContext = other as UnityObjectContextSelection;

            return asObjectContext.m_Target == m_Target;
        }
    }
}
