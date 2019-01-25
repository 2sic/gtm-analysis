using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace ToSic.Om.Gtm.Analysis.Data
{
    public class Trigger2Csv: IPreparesForCsv
    {
        public int Id;
        public string Name;
        public string Type;
        public TriggerFilter2Csv[] Filters;

        public Trigger2Csv(JsonSchema.Trigger original)
        {
            Type = original.type;
            Id = original.triggerId;
            Name = original.name;
            Filters = original.filter.Select(f => new TriggerFilter2Csv(f)).ToArray();
        }

        public List<dynamic> PrepareForCsv(/*bool flatten = false*/)
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
