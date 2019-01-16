using System.Diagnostics.CodeAnalysis;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Container: BaseElementFingerprint
    {
        public string path;
        public string name;
        public string publicId;
        public string[] usageContext;
        public string tagManagerUrl;
    }
}
