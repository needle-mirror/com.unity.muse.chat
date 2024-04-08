using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat
{
    internal static class ConsoleUtils
    {
        static FieldInfo s_ConsoleListViewField;
        static ConsoleWindow s_ConsoleWindow;
        static ListViewState s_ConsoleWindowListViewState;
        private static readonly LogEntry s_Entry = new();

        /// <summary>
        /// Returns the console window's ListViewState field info.
        /// </summary>
        /// <remarks>this method is internal for testing purposes</remarks>
        internal static FieldInfo GetConsoleWindowSelectionState()
        {
            if ((s_ConsoleListViewField ??= typeof(ConsoleWindow)
                    .GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic)) == null)
                return null;
            return s_ConsoleListViewField;
        }

        internal static T GetOpenWindow<T>() where T : EditorWindow
        {
            T[] windows = Resources.FindObjectsOfTypeAll<T>();
            if (windows != null && windows.Length > 0)
            {
                return windows[0];
            }
            return null;
        }

        internal static void GetSelectedConsoleLogs(List<LogReference> results)
        {
            results.Clear();
            var currentConsoleWindow = GetOpenWindow<ConsoleWindow>();
            // null if no console window can be found
            if (!ReferenceEquals(currentConsoleWindow, s_ConsoleWindow))
            {
                s_ConsoleWindow = currentConsoleWindow;
                if (s_ConsoleWindow == null)
                    return;
                var consoleWindowSelectionState = GetConsoleWindowSelectionState();
                s_ConsoleWindowListViewState = consoleWindowSelectionState.GetValue(s_ConsoleWindow) as ListViewState;
            }
            else
            {
                if (s_ConsoleWindow == null)
                    return;
            }

            // null if the m_ListView private field has been renamed or its type has changed</returns>
            if (s_ConsoleWindowListViewState == null)
                return;
            // no array allocation in any case.
            // true if the console window row with the same index is selected.
            bool[] selectedRows = s_ConsoleWindowListViewState.selectedItems;
            if (selectedRows == null)
            {
                results.Clear();
                return;
            }

            for (int i = 0; i < selectedRows.Length; i++)
            {
                if (!selectedRows[i])
                    continue;
                if (LogEntries.GetEntryInternal(i, s_Entry))
                    results.Add(LogEntryToReference(s_Entry));
            }
        }

        static LogReference LogEntryToReference(LogEntry entry)
        {
            var logRef = new LogReference();
            logRef.Message = entry.message;
            logRef.File = entry.file;
            logRef.Line = entry.line;
            logRef.Column = entry.column;
            var mode = (ConsoleWindow.Mode) entry.mode;
            if ((mode & (ConsoleWindow.Mode.Error | ConsoleWindow.Mode.Assert |
                                                   ConsoleWindow.Mode.Fatal | ConsoleWindow.Mode.AssetImportError |
                                                   ConsoleWindow.Mode.ScriptingError |
                                                   ConsoleWindow.Mode.ScriptCompileError |
                                                   ConsoleWindow.Mode.ScriptingException |
                                                   ConsoleWindow.Mode.GraphCompileError |
                                                   ConsoleWindow.Mode.ScriptingAssertion |
                                                   ConsoleWindow.Mode.StickyError | ConsoleWindow.Mode.ReportBug |
                                                   ConsoleWindow.Mode.DisplayPreviousErrorInStatusBar |
                                                   ConsoleWindow.Mode.VisualScriptingError
                                                   )) != 0)
            {
                logRef.Mode = LogReference.ConsoleMessageMode.Error;
            }
            else if ((mode & (ConsoleWindow.Mode.AssetImportWarning |
                              ConsoleWindow.Mode.ScriptingWarning |
                              ConsoleWindow.Mode.ScriptCompileWarning)) != 0)
            {
                logRef.Mode = LogReference.ConsoleMessageMode.Warning;
            }
            else
            {
                logRef.Mode = LogReference.ConsoleMessageMode.Log;
            }
            return logRef;
        }

        [MenuItem("internal:Muse/Internals/Log Random messages")]
        static void LogRandomMessage()
        {
            for (int i = 0; i < 20; i++)
            {
                var loremIpsum = "Lorem ipsum " + i;
                switch (i % 3)
                {
                    case 0:
                        Debug.Log(loremIpsum);
                        break;
                    case 1:
                        Debug.LogWarning(loremIpsum);
                        break;
                    case 2:
                        Debug.LogError(loremIpsum);
                        break;
                }
            }
        }

        [MenuItem("internal:Muse/Internals/Log Console Selection")]
        static void LogSelection()
        {
            List<LogReference> logs = new();
            GetSelectedConsoleLogs(logs);
            Debug.Log($"{logs.Count} entries:\n{string.Join("\n", logs.Select(l => l.Message))}");
        }
    }
}
