using System;
using System.IO;
using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Provides a base class to manage the lifecycle of a VisualElement template including helper functions for templates and other assets.
    /// To use this class, create a new class that inherits from ManagedTemplate and implement the InitializeView method to set up the template.
    /// For each element Initialize() must be called to initialize.
    ///
    /// Templates will also get a shared stylesheet loaded from the shared asset path if it is set.
    /// </summary>
    internal abstract class ManagedTemplate : TemplateContainer
    {
        readonly StyleCache m_StyleCache;
        readonly ViewCache m_ViewCache;

        readonly string m_AssetPath;
        string m_SharedAssetPath;

        string m_ResourceNameOverride = string.Empty;
        string m_ResourcePrefix = string.Empty;
        string m_ResourceSuffix = string.Empty;

        Type m_ElementType;

        /// <summary>
        /// Creates a new ManagedTemplate with the specified custom element type.
        /// </summary>
        /// <param name="customElementType">The type of the custom templated element</param>
        /// <param name="basePath">The base path to use for this template</param>
        protected ManagedTemplate(Type customElementType, string basePath = null)
            : this(basePath)
        {
            m_ElementType = customElementType;
        }

        /// <summary>
        /// Creates a new ManagedTemplate
        /// </summary>
        /// <param name="basePath">The base path to use for this template</param>
        /// <param name="subPath">The custom sub-path to use, can be left empty or null if not applicable</param>
        protected ManagedTemplate(string basePath = null, string subPath = null)
        {
            pickingMode = PickingMode.Ignore;

            m_ElementType = GetType();

            if (string.IsNullOrEmpty(basePath))
            {
                basePath = MuseChatConstants.ResourceBasePath;
            }

            m_ViewCache = ViewCache.Get(basePath, subPath);
            m_StyleCache = StyleCache.Get(basePath, subPath);

            m_AssetPath = basePath + MuseChatConstants.AssetFolder;
        }

        /// <summary>
        /// Event that is triggered when the visibility of the template changes.
        /// </summary>
        public event Action<bool> VisibilityChanged;

        /// <summary>
        /// Determines if the template is currently shown.
        /// </summary>
        public virtual bool IsShown { get; protected set; }

        /// <summary>
        /// Determines if the template has been initialized.
        /// </summary>
        protected bool IsInitialized { get; private set; }

        /// <summary>
        /// Initializes the template, this is mandatory to call before using the template.
        /// </summary>
        /// <param name="autoShowControl">If true the template will be shown by default, otherwise it will be loaded in its default state</param>
        public virtual void Initialize(bool autoShowControl = true)
        {
            ResourceName = string.IsNullOrEmpty(m_ResourceNameOverride)
                ? m_ElementType.Name
                : m_ResourceNameOverride;

            ResourceNameInvariant = ResourceName.ToLowerInvariant();

            DoInitView();
            DoInitStyle();
            DoInitAppUITheme();

            AddToClassList(MuseChatConstants.AppUIEditorClass);

            if (autoShowControl)
            {
                Show();
            }

            IsInitialized = true;
        }

        /// <summary>
        /// Get a class name prefixed with the current template
        /// </summary>
        /// <param name="className">The class name to prefix</param>
        /// <returns>The fully prefixed class name as applicable to this template</returns>
        public string GetPrefixedClassName(string className)
        {
            return $"{ResourceNameInvariant}__{className}";
        }

        /// <summary>
        /// Add a prefixed class to the template
        /// </summary>
        /// <param name="className">the base class name to add</param>
        public void AddPrefixedClass(string className)
        {
            string prefixedName = GetPrefixedClassName(className);
            AddToClassList(prefixedName);
        }

        /// <summary>
        /// Remove a prefixed class from the template
        /// </summary>
        /// <param name="className">the base class name to remove</param>
        public void RemovePrefixedClass(string className)
        {
            string prefixedName = GetPrefixedClassName(className);
            this.RemoveFromClassList(prefixedName);
        }

        /// <summary>
        /// Hides the templated element
        /// </summary>
        public virtual void Hide()
        {
            if (!IsShown)
            {
                return;
            }

            style.display = DisplayStyle.None;
            IsShown = false;

            VisibilityChanged?.Invoke(false);
        }

        /// <summary>
        /// Shows the templated element
        /// </summary>
        public virtual void Show()
        {
            if (IsShown)
            {
                return;
            }

            style.display = DisplayStyle.Flex;
            IsShown = true;

            VisibilityChanged?.Invoke(true);
        }

        protected Type ElementType => m_ElementType;

        protected bool IsVirtualControl { get; }

        protected string ResourceName { get; private set; }

        protected string ResourceNameInvariant { get; private set; }

        protected abstract void InitializeView(TemplateContainer view);

        protected void SetElementType(Type type)
        {
            m_ElementType = type;
        }

        protected void SetResourceName(string value)
        {
            m_ResourceNameOverride = value;
        }

        protected void SetResourcePrefix(string value)
        {
            m_ResourcePrefix = value;
        }

        protected void SetResourceSuffix(string value)
        {
            m_ResourceSuffix = value;
        }

        protected bool LoadView<T>(out VisualTreeAsset view)
        {
            return LoadView(typeof(T).Name, out view);
        }

        protected bool LoadView(string viewName, out VisualTreeAsset view)
        {
            string viewFile = $"{m_ResourcePrefix}{viewName}{m_ResourceSuffix}";

#if UNITY_EDITOR
            viewFile = string.Concat(viewFile, MuseChatConstants.TemplateExtension);
#endif

            view = m_ViewCache.Load(viewFile);
            return view != null;
        }

        protected bool LoadAsset<T>(string relativePath, ref T target)
            where T : UnityEngine.Object
        {
            string assetFile = $"{m_AssetPath}{m_ResourcePrefix}{relativePath}";
            return UXLoader.LoadAsset(assetFile, ref target);
        }

        protected bool LoadSharedAsset<T>(string relativePath, ref T target)
            where T : UnityEngine.Object
        {
            string assetFile = $"{m_SharedAssetPath ?? m_AssetPath}{m_ResourcePrefix}{relativePath}";
            return UXLoader.LoadAsset(assetFile, ref target);
        }

        protected void LoadImage(VisualElement parentElement, string elementName, string iconPath, ref Texture2D cache)
        {
            var targetElement = parentElement.Q<Image>(elementName);
            if (targetElement == null)
            {
                return;
            }

            LoadImage(targetElement, iconPath, ref cache);
        }

        protected void LoadImage(Image target, string iconPath, ref Texture2D cache)
        {
            if (LoadAsset(iconPath, ref cache))
            {
                target.image = cache;
            }
        }

        protected void LoadBackgroundImage(VisualElement target, string iconPath, ref Texture2D cache)
        {
            if (LoadAsset(iconPath, ref cache))
            {
                target.style.backgroundImage = cache;
            }
        }

        protected void LoadSharedImage(VisualElement parentElement, string elementName, string iconPath, ref Texture2D cache)
        {
            var targetElement = parentElement.Q<Image>(elementName);
            if (targetElement == null)
            {
                return;
            }

            LoadSharedImage(targetElement, iconPath, ref cache);
        }

        protected void LoadSharedImage(Image target, string iconPath, ref Texture2D cache)
        {
            if (LoadSharedAsset(iconPath, ref cache))
            {
                target.image = cache;
            }
        }

        protected void LoadSharedBackgroundImage(VisualElement target, string iconPath, ref Texture2D cache)
        {
            if (LoadSharedAsset(iconPath, ref cache))
            {
                target.style.backgroundImage = cache;
            }
        }

        protected void SetSharedAssetPath(string newSharedPath)
        {
            m_SharedAssetPath = newSharedPath;
        }

        void DoInitView()
        {
            if (this.IsVirtualControl)
            {
                return;
            }

            if (!LoadView(ResourceName, out VisualTreeAsset viewTree))
            {
                throw new InvalidDataException(ResourceName);
            }

            TemplateContainer view = viewTree.CloneTree();
            view.pickingMode = PickingMode.Ignore;

            view.AddToClassList(GetPrefixedClassName("element-root"));

            InitializeView(view);
            Add(view);
        }

        void DoInitStyle()
        {
            AddPrefixedClass("element");
        }

        void DoInitAppUITheme()
        {
            var queryBuilder = new UQueryBuilder<Panel>(this).OfType<Panel>();

            queryBuilder.ForEach(x =>
            {
                x.theme = EditorGUIUtility.isProSkin ? MuseChatConstants.AppUIThemeDark : MuseChatConstants.AppUIThemeLight;
                x.scale = MuseChatConstants.AppUIScale;
            });
        }
    }
}
