using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Model;

namespace Unity.Muse.Chat
{
    internal class CodeBlockValidator
    {
        internal const string k_ValidatorAssemblyName = "Unity.Muse.CodeGen";

        readonly DynamicAssemblyBuilder m_Builder = new(k_ValidatorAssemblyName);

        public bool ValidateCode(string code, out string localFixedCode, out string compilationLogs)
        {
            var codeAssembly = m_Builder.CompileAndLoadAssembly(code, out compilationLogs, out localFixedCode);

            return codeAssembly != null;
        }

        internal async Task<string> Repair(MuseMessageId messageId, int messageIndex, string errorToRepair, string scriptToRepair)
        {
            var repairedMessage = await MuseEditorDriver.instance.RepairScript(messageId, messageIndex, errorToRepair, scriptToRepair, ScriptType.CodeGen);

            if (string.IsNullOrEmpty(repairedMessage))
                return null;

            var match = Regex.Match(repairedMessage, @"```csharp(.*?)```", RegexOptions.Singleline);
            if (match.Success)
            {
                var code = match.Groups[1].Value;
                return code;
            }

            return null;
        }
    }
}
