using System.Diagnostics.CodeAnalysis;
using System.Linq;

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

        public string ResolveVariable(string key)
        {
            if (key == null) return null;
            if (!key.StartsWith("{{") || !key.EndsWith("}}")) return key;

            var realKey = key.Substring(2, key.Length - 4);
            var found = variable.FirstOrDefault(v => v.name == realKey);
            if (found == null) return key;

            var result = found.parameter.Find("value")?.GetValue;
            if (result == null)
            {
                var maybeJs = found.parameter.Find("javascript")?.GetValue;
                if (maybeJs != null)
                    result = "js(...)";
            }
            return result ?? key;
        }
    }

}
