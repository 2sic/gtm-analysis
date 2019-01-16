using System.Linq;

namespace ToSic.Om.Gtm.Analysis.Data
{
    public class TriggerFilter
    {
        public string Type;
        public string Key;
        public string Value;

        public TriggerFilter(JsonSchema.TriggerFilter original)
        {
            Type = original.type;
            Key = original.parameter.First(p => p.key == "arg0").value;
            Value = original.parameter.First(p => p.key == "arg1").value;
        }



        public string OperatorCode
        {
            get
            {
             switch (Type.ToLowerInvariant())
            {
                case "contains":
                    return "~";
                default:
                    return "?";
            }
            }
        }
    }
}
