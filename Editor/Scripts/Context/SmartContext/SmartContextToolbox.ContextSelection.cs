using System;
using Unity.Muse.Chat.FunctionCalling;

namespace Unity.Muse.Chat.Context.SmartContext
{
    internal partial class SmartContextToolbox
    {
        class ContextSelection : IContextSelection
        {
            string IContextSelection.DownsizedPayload => Payload;

            string IContextSelection.ContextType => "background information";

            string IContextSelection.TargetName => "";

            public string Classifier { get; } = "smart";
            public string Description { get; }
            public string Payload { get; }

            public ContextSelection(CachedFunction selector, string result)
            {
                Payload = result;
                Description = selector.FunctionDefinition.Description;
            }

            public bool Equals(IContextSelection other)
            {
                return Classifier == other.Classifier
                    && Description == other.Description
                    && Payload == other.Payload;
            }
        }
    }
}
