using System.Diagnostics.CodeAnalysis;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ParameterListItem
    {
        public string type;
        public ParameterLight[] map;

        public string GetMap()
        {
            var field = map.Find("fieldName")?.value;
            var value = map.Find("value")?.value;
            return type == "MAP" ? $"{field}={value}" : $"{field}:{value}";
        }
    }
}
