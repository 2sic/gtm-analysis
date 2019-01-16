using System.Diagnostics.CodeAnalysis;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TriggerFilter
    {                        
        public string type;
        public Parameter[] parameter;
    }
}
