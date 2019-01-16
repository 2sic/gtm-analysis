using System.Diagnostics.CodeAnalysis;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ContainerVersion: BaseElementFingerprint
    {
        public string path;
        public string containerVersionId;
        public Container container;
        public Tag[] tag;
        public Trigger[] trigger;
        public Variable[] variable;
        public VariableBuiltIn[] builtInVariable;

    }

}
