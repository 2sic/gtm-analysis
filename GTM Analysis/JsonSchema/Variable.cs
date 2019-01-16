using System.Diagnostics.CodeAnalysis;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Variable : BaseElementFingerprint
    {
        public string variableId;
        public string name;
        public string type;
        public Parameter[] parameter;
    }
}
