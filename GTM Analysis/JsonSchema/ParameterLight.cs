using System.Diagnostics.CodeAnalysis;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ParameterLight
    {
        public string type;
        public string key;
        public string value { get; set; }

        public virtual string GetValue => value;

    }
}
