using System.ComponentModel;

namespace Unity.Muse.Chat
{
    internal enum ChatCommandType
    {
        [Description("Ask questions and get help")]
        Ask,
#if ENABLE_ASSISTANT_BETA_FEATURES
        [Description("Give Muse a task to do")]
        Run,
        [Description("Generate code with Muse")]
        Code
#endif
    }
}
