using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal static class UXLoader
    {
        private static readonly StyleCache m_StaticStyleCache;

        /// <summary>
        /// Static constructor to initialize the static style cache
        /// </summary>
        static UXLoader()
        {
            m_StaticStyleCache = StyleCache.Get(MuseChatConstants.UIModulePath);
        }

        /// <summary>
        /// Load a custom asset from the editor if the asset is not already loaded as provided by the target.
        /// </summary>
        /// <param name="file">The asset to load</param>
        /// <param name="target">The target reference to load the asset into, if it is already set this method will do nothing</param>
        /// <param name="silentFailure">if true there will be no logs or throws in case of failure to load the asset</param>
        /// <typeparam name="T">The type of asset to load</typeparam>
        /// <returns>True if the target is already set or if the asset was loaded successfully</returns>
        public static bool LoadAsset<T>(string file, ref T target, bool silentFailure = false)
            where T : UnityEngine.Object
        {
            if (target != null)
            {
                // Asset is already loaded
                return true;
            }

            target = LoadAssetInternal<T>(file);
            if (target == null)
            {
                if (!silentFailure)
                {
                    Debug.LogErrorFormat("Failed to Load Asset: {0}", file);
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Load the shared style into a generic TemplateContainer (not managed)
        /// </summary>
        /// <param name="target">the target to load the style into</param>
        public static void LoadSharedStyle(this TemplateContainer target)
        {
#if UNITY_EDITOR
            string styleFile = string.Concat(MuseChatConstants.SharedStyleName, MuseChatConstants.StyleExtension);
#else
            string styleFile = Constants.SharedStyleName;
#endif

            var sheet = m_StaticStyleCache.Load(styleFile);
            if (sheet != null && !target.styleSheets.Contains(sheet))
            {
                target.styleSheets.Add(sheet);
            }
        }

        /// <summary>
        /// Load the shared style for a managed template
        /// </summary>
        /// <param name="target">the template to load the style for</param>
        internal static void LoadSharedStyle(this ManagedTemplate target)
        {
#if UNITY_EDITOR
            string styleFile = string.Concat(MuseChatConstants.SharedStyleName, MuseChatConstants.StyleExtension);
#else
            string styleFile = Constants.SharedStyleName;
#endif

            var sheet = m_StaticStyleCache.Load(styleFile);
            if (sheet != null && !target.styleSheets.Contains(sheet))
            {
                target.styleSheets.Add(sheet);
            }
        }

        static T LoadAssetInternal<T>(string file)
            where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            // First we try to load the asset directly via path
            T result = AssetDatabase.LoadAssetAtPath<T>(file);
            if (result != null)
            {
                return result;
            }

            // If that fails we fall through to try to load as resource
#endif

            var resourceIndex = file.ToLowerInvariant().IndexOf(MuseChatConstants.ResourceFolderName.ToLowerInvariant() + "/", StringComparison.Ordinal);
            if (resourceIndex >= 0)
            {
                int copyIndex = resourceIndex + MuseChatConstants.ResourceFolderName.Length + 1;
                file = file.Substring(copyIndex, file.Length - copyIndex);
            }

            var extensionIndex = file.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
            if (extensionIndex > 0)
            {
                file = file.Substring(0, extensionIndex);
            }

            return Resources.Load<T>(file);
        }
    }
}
