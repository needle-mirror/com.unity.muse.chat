using System.Collections.Generic;
using UnityEditor;

namespace Unity.Muse.Chat
{
    internal static class MuseChatConstants
    {
        internal const int MaxInspirationCount = 100;
        internal const int MaxConversationHistory = 1000;
        internal const int MaxFeedbackMessageLength = 1000;
        internal const int MaxMuseMessageLength = 4000;

        internal const int CompactWindowThreshold = 700;
        internal const string CompactStyle = "mui-compact";
        internal const string IconStylePrefix = "mui-icon-";

        internal const int RoutesPopupOffset = 13;

        internal const string TextCutoffSuffix = "...";

        internal const char UnityPathSeparator = '/';
        internal const string TemplateExtension = ".uxml";
        internal const string StyleExtension = ".uss";

        internal const string ResourceFolderName = "Resources";
        internal const string PackageName = "com.unity.muse.chat";
        internal const string PackageRoot = "";
        internal const string BasePath = "Packages/" + PackageName + PackageRoot + "/";
        internal const string UIEditorPath = "Editor/UI/";

        internal const string AssetFolder = "Assets/";
        internal const string ViewFolder = "Views/";
        internal const string StyleFolder = "Styles/";

        internal const string UIModulePath = BasePath + UIEditorPath;
        internal const string UIStylePath = UIModulePath + StyleFolder;

        internal const string MuseChatBaseStyle = "MuseChat.tss";
        internal const string MuseChatSharedStyleDark = "MuseChatSharedDark";
        internal const string MuseChatSharedStyleLight = "MuseChatSharedLight";

        internal static readonly string SourceReferenceColor = EditorGUIUtility.isProSkin ? "FF85ABFF" : "881f49FF";
        internal static readonly string SourceReferencePrefix = "REF:";

        internal const string ProjectIdTagPrefix = "projId:";

        internal const string ContextTag = "#PROJECTCONTEXT#";
        internal static readonly string ContextTagEscaped = ContextTag.Replace("#", @"\#");

        internal const string DisclaimerText = @"// {0} AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

";

        internal const bool DebugMode = false;
        internal const string MediationPrompt = "";
        internal const bool SkipPlanning = false;

        internal const int SuggestedSelectedContextLimit = 5;
        internal const int PromptContextLimit = 34000;


        internal const string AgentModelPath = "Packages/com.unity.muse.chat/Editor/agentModel.asset";

        public static readonly RouteCommand AskRoute = new (
            "Ask",
            "/ask",
            "/ask [text]",
            "Ask a question in the Editor",
            ChatCommandType.Ask
        );

        public static readonly RouteCommand RunRoute = new(
            "Run",
            "/run",
            "/run [text]",
            "Run commands in the Editor",
            ChatCommandType.Run
        );

        public static readonly RouteCommand CodeRoute = new(
            "Code",
            "/code",
            "/code [text]",
            "Write scriptes with a dedicated code engine",
            ChatCommandType.Code
        );

      /*  public static readonly RouteCommand MatchThreeRoute = new(
            "Match 3",
            "/match3",
            "/match3 [quantity, size, shape]",
            "Generate a match 3 board",
            ChatCommandType.MatchThree
        );*/

        internal static readonly List<RouteCommand> Routes =  new (){
            RunRoute,
            CodeRoute,
          //  MatchThreeRoute,
        };
    }
}
