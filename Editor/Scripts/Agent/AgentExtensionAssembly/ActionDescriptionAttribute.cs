using System;

namespace Unity.Muse.Agent.Dynamic
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
#if CODE_LIBRARY_INSTALLED
    public
#else
    internal
#endif
    class ActionDescriptionAttribute : Attribute
    {
        public string Description { get; }

        public ActionDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
