using System.Diagnostics.CodeAnalysis;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Tag: BaseElementFingerprint
    {
        public string tagId; 
        public string name;
        public string type;
        public Parameter[] parameter;
        public string[] firingTriggerId;
        public string tagFiringOption;
        public bool paused;

    }
}
