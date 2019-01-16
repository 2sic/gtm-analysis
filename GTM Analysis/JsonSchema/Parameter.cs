using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Parameter: ParameterLight
    {
        public ParameterListItem[] list;

        public override string GetValue
        {
            get
            {
                if (type != "LIST")
                    return base.GetValue;
                if (list == null || list.Length == 0) return "";

                return string.Join(";", list.Select(l => l.GetMap()));
            }
        }
    }
}
