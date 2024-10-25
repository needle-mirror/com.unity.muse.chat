using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Agent.Dynamic
{
#if CODE_LIBRARY_INSTALLED
    public
#else
    internal
#endif
    class ExecutionResult
    {
        internal static readonly string WarningTextColor = EditorGUIUtility.isProSkin ? "#DFB33D" : "#B76300";

        static int k_NextExecutionId = 1;

        static readonly Regex k_PlaceholderRegex = new(@"%(\d+)", RegexOptions.Compiled);

        List<string> m_Result = new();
        string m_ConsoleLogs;
        int UndoGroup;

        public readonly int Id;
        public readonly string ActionName;

        public List<string> Result => m_Result;
        public string ConsoleLogs => m_ConsoleLogs;
        public bool SuccessfullyStarted { get; private set; }

        public ExecutionResult(string actionName)
        {
            Id = k_NextExecutionId++;
            ActionName = actionName;
        }

        public void RegisterObjectCreation(Object objectCreated)
        {
            Undo.RegisterCreatedObjectUndo(objectCreated, $"{objectCreated.name} was created");
        }

        public void RegisterObjectCreation(Component component)
        {
            Undo.RegisterCreatedObjectUndo(component, $"{component} was attached to {component.gameObject.name}");
        }

        public void RegisterObjectModification(Object objectToRegister, string operationDescription = "")
        {
            if (!string.IsNullOrEmpty(operationDescription))
                Undo.RecordObject(objectToRegister, operationDescription);
            else
                Undo.RegisterCompleteObjectUndo(objectToRegister, $"{objectToRegister.name} was modified");
        }

        public void DestroyObject(Object objectToDestroy)
        {
            if (!EditorApplication.isPlaying)
                Undo.DestroyObjectImmediate(objectToDestroy);
            else
                Object.Destroy(objectToDestroy);
        }

        public void Start()
        {
            SuccessfullyStarted = true;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName(ActionName ?? "Muse chat agent action");
            UndoGroup = Undo.GetCurrentGroup();

            Application.logMessageReceived += HandleConsoleLog;
        }

        public void End()
        {
            Application.logMessageReceived -= HandleConsoleLog;

            Undo.CollapseUndoOperations(UndoGroup);
        }

        public void Log(string log, params object[] references)
        {
            var formattedLog = RichTextFormat(log, references);
            m_Result.Add(formattedLog);
        }

        public void LogWarning(string log, params object[] references)
        {
            var formattedLog = RichTextFormat(log, references);
            m_Result.Add(formattedLog.RichColor(WarningTextColor));
        }

        public void LogError(string log, params object[] references)
        {
            LogWarning(log, references);
        }

        private static string RichTextFormat(string log, object[] references)
        {
            string formattedLog = k_PlaceholderRegex.Replace(log, match =>
            {
                int index = int.Parse(match.Groups[1].Value);
                if (index >= 0 && index < references.Length)
                {
                    var reference = references[index];
                    switch (reference)
                    {
                        case Object objectInstance:
                        {
                            if (objectInstance != null)
                            {
                                return $"<link={objectInstance.GetInstanceID()}>{objectInstance.name.RichColor("#8facef")}</link>";
                            }
                            else
                            {
                                return "<i>Destroyed Object</i>";
                            }
                        }
                        default:
                            return references[index]?.ToString();
                    }
                }

                return match.Value;
            });
            return formattedLog;
        }

        void HandleConsoleLog(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Warning)
            {
                m_ConsoleLogs += $"{type}: {logString}\n";
            }
        }
    }

    internal static class RichSyntax
    {
        public static string RichColor(this string text, string hexColor)
        {
            return $"<color={hexColor}>{text}</color>";
        }
    }
}
