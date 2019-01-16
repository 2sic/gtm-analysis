using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace ToSic.Om.Gtm.Analysis.Data
{
    public class Trigger: IPreparesForCsv
    {
        public int Id;
        public string Name;
        public string Type;
        public TriggerFilter[] Filters;

        public Trigger(JsonSchema.Trigger original)
        {
            Type = original.type;
            Id = original.triggerId;
            Name = original.name;
            Filters = original.filter.Select(f => new TriggerFilter(f)).ToArray();
        }

        public List<dynamic> PrepareForCsv(bool flatten = false)
        {
            dynamic data = new ExpandoObject();
            data.Name = Name;
            data.Type = Type;
            var exp = data as IDictionary<string, object>;
            foreach (var f in Filters)
                exp.Add(f.Key, f.OperatorCode + f.Value);
            return new List<dynamic> {data};
        }

    }

}
