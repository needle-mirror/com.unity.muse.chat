using UnityEditor;

namespace Unity.Muse.Chat
{
    class CodeEditorProjectUtils
    {
        public static void Sync()
        {
            CodeEditorProjectSync.SyncEditorProject();
        }
    }
}
