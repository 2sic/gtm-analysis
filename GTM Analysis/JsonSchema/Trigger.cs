using System.Diagnostics.CodeAnalysis;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Trigger: BaseElement
    {
        public int triggerId;
        public string name;
        public string type;
        public TriggerFilter[] filter;
    }
}
