using System.ComponentModel;

namespace Unity.Muse.Chat
{
    public enum ChatCommandType
    {
        [Description("Ask questions and get help")]
        Ask,
        [Description("Give Muse a task to do")]
        Run,
        [Description("Generate code with Muse")]
        Code,
        //[Description("Generate Match Three code with Muse")]
        //MatchThree,
    }
}
