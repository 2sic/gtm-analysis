using System.Diagnostics.CodeAnalysis;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class VariableBuiltIn: BaseElement
    {
        public string type;
        public string name;
    }
}
