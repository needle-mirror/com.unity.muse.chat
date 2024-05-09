using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Muse.Chat.Serialization;
using Unity.Serialization.Json;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Rendering;

[assembly: InternalsVisibleTo("Unity.Muse.Chat.Editor.Tests")]

namespace Unity.Muse.Chat
{
    static class UnityDataUtils
    {
        /// <summary>
        /// Describes how much of the version to retrieve when getting the project version
        /// </summary>
        public enum VersionDetail
        {
            /// <summary>Only the major version number</summary>
            Major = 0,
            /// <summary>Major and revision number</summary>
            Revision = 1,
            /// <summary>Major, revision, and patch number</summary>
            Patch = 2
        }

        /// <summary>
        /// Describes the type of rendering pipeline to retrieve
        /// </summary>
        public enum RenderingPipeLineType
        {
            /// <summary>The default rendering pipeline for the project</summary>
            Default = 0,
            /// <summary>The active rendering pipeline for the current quality level</summary>
            Current = 1
        }

        static ListRequest s_ListRequest;
        static Dictionary<string, string> s_PackageMap = new ();

        static int s_PackageUpdateCount = 0;

        static ISerializationOverrideProvider s_SerializationOverrideProvider;

        /// <summary>
        /// Queries the package manager for all packages in the project and caches the results
        /// </summary>
        /// <param name="invalidate">If ture, any pre-cached package list will be cleared</param>
        public static void CachePackageData(bool invalidate)
        {
            if (invalidate)
            {
                s_PackageMap.Clear();
            }

            EditorApplication.update -= CachePackagesUpdate;
            EditorApplication.update += CachePackagesUpdate;

            if (s_ListRequest != null && !s_ListRequest.IsCompleted)
                return;

            s_PackageUpdateCount++;
            s_ListRequest = UnityEditor.PackageManager.Client.List();
        }

        public static bool PackageDataReady()
        {
            return (s_ListRequest == null && s_PackageMap.Count > 0);
        }

        public static int PackageUpdateCount()
        {
            return s_PackageUpdateCount;
        }

        static void CachePackagesUpdate()
        {
            if (s_ListRequest == null)
            {
                EditorApplication.update -= CachePackagesUpdate;
                return;
            }

            if (s_ListRequest.IsCompleted)
            {
                // Save all the package data
                s_PackageMap = s_ListRequest.Result.ToDictionary(p => p.name, p => p.version);

                EditorApplication.update -= CachePackagesUpdate;
                s_ListRequest = null;
            }
        }

        /// <summary>
        /// Returns the Unity version this project is running on
        /// </summary>
        /// <param name="detail">How much of the version to include : Format is Major.revision.patch</param>
        /// <returns>The Unity version at the requested detail level</returns>
        internal static string GetProjectVersion(VersionDetail detail)
        {
            var version = Application.unityVersion;
            switch (detail)
            {
                case VersionDetail.Major:
                    return version.Substring(0, version.IndexOf("."));
                case VersionDetail.Revision:
                    return version.Substring(0, version.LastIndexOf("."));
            }
            return version;
        }

        /// <summary>
        /// Returns the active render pipeline for the current quality level or the default rendering pipeline for the project.
        /// </summary>
        /// <param name="type"> Whether to return the default rendering pipeline.</param>
        /// <returns>The rendering pipeline of the project.</returns>
        internal static string GetProjectRenderingPipeline(RenderingPipeLineType type = RenderingPipeLineType.Current)
        {
            var renderingPipeline = type == RenderingPipeLineType.Default
                ? GraphicsSettings.defaultRenderPipeline
                : GraphicsSettings.currentRenderPipeline;
            if (renderingPipeline == null)
            {
                return "No rendering pipeline is currently selected, the built-in pipeline is used.";
            }
            return renderingPipeline.name;
        }

        /// <summary>
        /// Returns the target platform for the current build settings.
        /// </summary>
        /// <returns>The current build target platform.</returns>
        internal static string GetTargetPlatform()
        {
            return EditorUserBuildSettings.activeBuildTarget.ToString();
        }

        /// <summary>
        /// Return the current api compatibility level for the project.
        /// </summary>
        /// <returns> The current API compatibility. </returns>
        internal static string GetCompatibilityLevel()
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var apiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
            return apiCompatibilityLevel.ToString();
        }

        /// <summary>
        /// Return the current input system for the project.
        /// </summary>
        /// <returns> The current input system</returns>
        internal static string GetInputSystem()
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            return "New Input System";
#elif ENABLE_LEGACY_INPUT_MANAGER && ENABLE_INPUT_SYSTEM
            return "Both New Input System and Legacy Input Manager";
#elif ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
            return "Legacy Input Manager";
#else
            return "None";
#endif
        }

        /// <summary>
        /// Returns a dictionary of package->version dependencies for the project
        /// </summary>
        /// <returns>A dictionary where the package is the key and version is the value</returns>
        public static Dictionary<string, string> GetPackageMap()
        {
            if (s_PackageMap.Count == 0)
                Debug.LogWarning("No package data available. Please call CachePackageData first.");

            return s_PackageMap;
        }

        /// <summary>
        /// Returns the project configuration summary including rendering pipeline, target platform, api compatibility level and input system.
        /// </summary>
        /// <returns>The project configuration summary.</returns>
        internal static Dictionary<string, string> GetProjectSettingSummary()
        {
            return new Dictionary<string, string>
            {
                {"Active Rendering Pipeline", GetProjectRenderingPipeline()},
                {"Target Platform/OS", GetTargetPlatform()},
                {"API Compatibility Level", GetCompatibilityLevel()},
                {"Input System", GetInputSystem()}
            };
        }

        public static string GetProjectId()
        {
            return $"{MuseChatConstants.ProjectIdTagPrefix}{PlayerSettings.productGUID.ToString().Replace("-", "")}";
        }

        /// <summary>
        /// Returns a string summary of the given log
        /// </summary>
        /// <param name="logData">The stored data for a single log entry</param>
        /// <param name="includeSource">If true, the content of the related source file will be included</param>
        /// <returns>A string summary of the given log message</returns>
        public static string OutputLogData(LogReference logData, bool includeSource)
        {
            if (includeSource)
            {
                var fileName = logData.File;
                if (!string.IsNullOrEmpty(fileName) && fileName.EndsWith(".cs") && File.Exists(fileName))
                {
                    return $"{logData.Message}\n{fileName} contains this code:\n{File.ReadAllText(fileName)}";
                }
            }

            return logData.Message;
        }

        /// <summary>
        /// Returns a string summary of the given object
        /// </summary>
        /// <param name="targetObject">The object to display as a string</param>
        /// <param name="includeTypes">If true, the type of each variable will be included in the output</param>
        /// <param name="includeTooltips">If true, the tooltip associated with each variable (if available) will be included in the output</param>
        /// <param name="maxDepth">If 0 or greater, how many levels deep of nested objects to travel</param>
        /// <param name="rootFields">The list of fields to write. Null means all fields.</param>
        /// <param name="useDisplayName">Write field using their beautified display name.</param>
        /// <param name="ignorePrefabInstance">If true, prefab instances are ignored.</param>
        /// <returns>A string summary of the given object and its components</returns>
        public static string OutputUnityObject(Object targetObject, bool includeTypes, bool includeTooltips, int maxDepth = -1, string[] rootFields = default, bool useDisplayName = false, bool ignorePrefabInstance = true)
        {
            if (targetObject == null)
                return string.Empty;

            var targetSerializedObject = new SerializedObject(targetObject);

            var adapters = new List<IJsonAdapter>();
            var parameters = new JsonSerializationParameters
            {
                DisableSerializedReferences = true,
                UserDefinedAdapters = adapters
            };
            var jsonAdapter = new SerializationObjectJsonAdapter();
            jsonAdapter.OutputType = includeTypes;
            jsonAdapter.OutputTooltip = includeTooltips;
            jsonAdapter.MaxObjectDepth = maxDepth;
            jsonAdapter.RootParameters = rootFields;
            jsonAdapter.RootObject = targetSerializedObject;
            jsonAdapter.UseDisplayName = useDisplayName;
            jsonAdapter.IgnorePrefabInstance = ignorePrefabInstance;
            jsonAdapter.OverrideProvider = GetSerializationOverrideProvider();

            adapters.Add(jsonAdapter);
            return $"{jsonAdapter.GetObjectKey(targetSerializedObject)}\n{JsonSerialization.ToJson(targetSerializedObject, parameters)}";
        }

        /// <summary>
        /// Retrieves a list of all project settings assets for serialization
        /// </summary>
        /// <returns>A list with each project setting asset loaded</returns>
        public static List<UnityEngine.Object> GetSettingsAssets()
        {
            var settingsList = new List<UnityEngine.Object>();

            var assetsPath = Application.dataPath;
            var projectSettingsPath = assetsPath.Substring(0, assetsPath.LastIndexOf("/Assets")) + "/ProjectSettings";
            var assetPaths = Directory.EnumerateFiles(projectSettingsPath, "*.asset");

            if (assetPaths.Count() == 0)
            {
                Debug.LogError("Project settings cannot be found.");
                return settingsList;
            }

            foreach (var assetPath in assetPaths)
            {
                var localPath = $"ProjectSettings/{Path.GetFileName(assetPath)}";

                var type = AssetDatabase.GetMainAssetTypeAtPath(localPath);
                var asset = AssetDatabase.LoadAssetAtPath(localPath, type);
                if (asset == null)
                    continue;

                settingsList.Add(asset);
            }

            return settingsList;
        }

        public static ISerializationOverrideProvider GetSerializationOverrideProvider()
        {
            if (s_SerializationOverrideProvider is not null)
                return s_SerializationOverrideProvider;

            var overrides = SerializationOverrideUtility
                .GetOverrideMethodsFromAttribute()
                .Select(t => SerializationOverrideUtility.CreateOverride(t.declaringType, t.field, t.@override));

            s_SerializationOverrideProvider = new SerializationOverrideProvider(overrides);
            return s_SerializationOverrideProvider;
        }
    }
}
