namespace Unity.Muse.Agent.Dynamic
{
#if CODE_LIBRARY_INSTALLED
    public
#else
    internal
#endif
    interface IAgentAction
    {
        public void Execute(ActionAttachment attachments, ExecutionResult result);
        public void BuildPreview(PreviewBuilder result);
    }
}
