using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Unity.Muse.Common.Editor.Integration;

namespace Unity.Muse.Chat.Context.SmartContext
{
    internal static partial class ContextRetrievalTools
    {
        // Description strings that are used in multiple methods:
        const string k_NameFilter = "Name of the gameObject.";

        private static string GetComponents(GameObject gameObject)
        {
            return $"GameObject {gameObject.name} has the following components attached to it: " + string.Join("\n",
                gameObject.GetComponents<Component>().Select(c => c != null ? c.GetType().Name : "<Missing Script>"));
        }

        [ContextProvider("Returns the serialized data of the object or asset (GameObject, prefab, script, etc.)")]
        internal static string ObjectDataExtractor(
            [Parameter("Name of the object or asset to extract data from.")] string objectName,
            [Parameter(
                "Optional: Filter to specify a particular component on the object if it’s a GameObject.")]
            string componentFilter = null)
        {
            if (string.IsNullOrEmpty(objectName))
                return string.Empty;

            var matchingAsset = ContextRetrievalHelpers.FindObject<Object>(objectName);

            var objectContext = new UnityObjectContextSelection();

            if (matchingAsset == null)
            {
                return string.Empty;
            }

            var prefix = $"Contents of asset {matchingAsset.name}:\n";

            if (matchingAsset is GameObject gameObject && !string.IsNullOrEmpty(componentFilter))
            {
                // Find matching component:
                var component = ContextRetrievalHelpers.FindComponent(gameObject, componentFilter);

                if (component != null)
                {
                    prefix = $"Contents of component {component.GetType().Name} on GameObject {matchingAsset.name}:\n";

                    objectContext.SetTarget(component);
                    return prefix + ((IContextSelection)objectContext).Payload;
                }
            }

            var path = AssetDatabase.GetAssetPath(matchingAsset);
            if (!string.IsNullOrEmpty(path))
            {
                prefix = $"Contents of asset at path '{path}':\n";
            }

            objectContext.SetTarget(matchingAsset);
            var result = prefix + ((IContextSelection)objectContext).Payload;

            if (result.Length <= SmartContextToolbox.SmartContextLimit)
            {
                return result;
            }

            return prefix + ((IContextSelection)objectContext).DownsizedPayload;
        }
    }
}
