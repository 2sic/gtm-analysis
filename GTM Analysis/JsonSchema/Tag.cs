// ReSharper disable InconsistentNaming

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
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
