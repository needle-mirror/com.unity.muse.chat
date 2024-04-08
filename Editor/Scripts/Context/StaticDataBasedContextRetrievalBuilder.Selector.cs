using System;
using System.Linq;
using UnityEditor;

namespace Unity.Muse.Chat
{
    partial class StaticDataBasedContextRetrievalBuilder
    {
        class Selector : IContextSelection, IEquatable<Selector>
        {
            string m_FilePath;
            string[] m_Parameters;

            public Selector(string type, string path, string description, string[] parameters)
            {
                Classifier =
                    $"The setting file {type} has this description: {description} has these parameters: {string.Join(" ", parameters)}";

                Description = description;

                m_FilePath = path;
                m_Parameters = parameters;
            }

            public bool Equals(IContextSelection other) =>
                other is Selector selector && Equals(selector);

            public bool Equals(Selector other) =>
                other != null
                && m_FilePath == other.m_FilePath
                && m_Parameters.SequenceEqual(other.m_Parameters);

            public string Classifier { get; }

            public string Description { get; }

            public string Payload
            {
                get
                {
                    var type = AssetDatabase.GetMainAssetTypeAtPath(m_FilePath);
                    var asset = AssetDatabase.LoadAssetAtPath(m_FilePath, type);

                    if (asset is null)
                        return default;

                    return UnityDataUtils.OutputUnityObject(
                        asset, false, false, rootFields: m_Parameters, useDisplayName: true);
                }
            }

            public bool Dirty => true;

            public void ResetDirty()
            {}
        }
    }
}
