using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Unity.Muse.Agent.Dynamic
{
    static class AssemblyCSProject
    {
        public const string FileName = "Unity.Muse.Agent.Dynamic.Extension.csproj";

        static List<string> m_TemporaryFiles = new();
        public static IEnumerable<string> TemporaryFiles => m_TemporaryFiles;

        public static string CreateTemporaryFile()
        {
            var tempGuid = Guid.NewGuid().ToString();
            var path = Path.Combine(Application.dataPath, "..", "Temp", $"{tempGuid}.cs");

            m_TemporaryFiles.Add(path);

            return path;
        }

        public static void ClearTemporaryFiles()
        {
            m_TemporaryFiles.Clear();
        }
    }
}
