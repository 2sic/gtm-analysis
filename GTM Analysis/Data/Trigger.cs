using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace ToSic.Om.Gtm.Analysis.Data
{
    public class Trigger
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

        public dynamic ForTable()
        {
            dynamic data = new ExpandoObject();
            data.Name = Name;
            data.Type = Type;
            var exp = data as IDictionary<string, Object>;
            foreach (var f in Filters)
                exp.Add(f.Key, f.Value);
            return data;
        }
    }

}
