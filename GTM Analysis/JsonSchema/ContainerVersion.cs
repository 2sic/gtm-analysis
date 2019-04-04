using System.Linq;

// ReSharper disable InconsistentNaming
namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    public class ContainerVersion: BaseElementFingerprint
    {
        public string path;
        public string containerVersionId;
        public Container container;
        public Tag[] tag;
        public Trigger[] trigger;
        public Variable[] variable;
        public VariableBuiltIn[] builtInVariable;
        public Folder[] folder;

        public string ResolveVariable(string key)
        {
            if (key == null) return null;

            // only look it up if it is wrapped with {{ }}, otherwise return original
            if (!key.StartsWith("{{") || !key.EndsWith("}}")) return key;

            // the real key is between the {{ }}
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

        public string ResolveFolder(string key)
        {
            if (key == null) return null;
            if (folder == null || !folder.Any()) return key;
            var found = folder.FirstOrDefault(f => f.folderId == key);
            if (found == null) return key;
            return found.name;
        }
    }

}
