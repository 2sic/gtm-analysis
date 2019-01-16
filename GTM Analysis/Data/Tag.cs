using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ToSic.Om.Gtm.Analysis.JsonSchema;

namespace ToSic.Om.Gtm.Analysis.Data
{
    public class Tag: IPreparesForCsv
    {
        public int Id;
        public string Name => _original.name;
        public string Type => _original.type;
        public int[] Triggers;

        private readonly JsonSchema.ContainerVersion _container;
        private readonly JsonSchema.Tag _original;

        public Tag(JsonSchema.Tag original, JsonSchema.ContainerVersion container)
        {

            Id = Convert.ToInt32(original.tagId);
            Triggers = original.firingTriggerId.Select(t => Convert.ToInt32(t)).ToArray();

            _container = container;
            _original = original;
        }

        private const string KeyInteractive = "Interact";

        public List<dynamic> PrepareForCsv(bool flatten = false)
        {
            // not flat, just one record per tag
            if (!flatten)
            {
                var data = BuildCoreCsv(-1);
                return new List<dynamic> {data};
            }

            // flatten: many records per tag, resolve the triggers
            var list = Triggers.Select(ftid =>
            {
                var data = BuildCoreCsv(ftid);
                ((dynamic) data).Triggers = ftid;
                var foundTrigger = _container.trigger.FirstOrDefault(t => t.triggerId == ftid);
                if (foundTrigger != null)
                {
                    var triggerData = new Trigger(foundTrigger).PrepareForCsv().First() as IDictionary<string, object>;
                    var asDict = (IDictionary<string, object>) data;
                    foreach (var trigD in triggerData)
                        asDict.Add("!" + trigD.Key, trigD.Value);
                }
                return (dynamic) data;
            }).ToList();
            return list;
        }

        private ExpandoObject BuildCoreCsv(int triggerId = -1)
        {
            dynamic data = new ExpandoObject();
            data.Id = Id;
            data.Name = Name;
            data.Type = Type;
            data.Fire = NiceTagFiring();
            data.Trigger = triggerId != -1 ? triggerId.ToString() : string.Join(",", Triggers.Select(t => t.ToString()));
            var dict = data as IDictionary<string, object>;
            PrepUaProperties(dict);
            return data;
        }

        private void PrepUaProperties(IDictionary<string, object> dict)
        {
            if (Type != "ua") return;

            AddToDict(dict, "TrackId", "trackingId");
            AddToDict(dict, "Tracking", "trackType", MapTrackType);
            AddToDict(dict, "LogCat", "eventCategory");
            AddToDict(dict, "LocAct", "eventAction");
            AddToDict(dict, "LogLbl", "eventLabel");
            AddToDict(dict, "LogVal", "value");
            AddToDict(dict, KeyInteractive, "nonInteraction", MapInteraction);
        }


        private void AddToDict(IDictionary<string, object> dict, string property, string key)
            => AddToDict<string>(dict, property, key);

        private void AddToDict<T>(IDictionary<string, object> dict, string property, string key, Func<string, T> mapper = null)
        {
            var val = _original.parameter.Find(key)?.value;
            var save = mapper == null ? (object) val : mapper.Invoke(val);
            dict.Add(property, save);
        }

        private string NiceTagFiring()
        {
            var original = _original.tagFiringOption;
            switch (original)
            {
                case "ONCE_PER_EVENT": return "1X-EVENT";
                case "ONCE_PER_LOAD": return "1X-LOAD";
                default: return original;
            }
        }

        private const string Prefix = "TRACK_";

        private static string MapTrackType(string original)
            => original == null
                ? null
                : (original.StartsWith(Prefix) ? original.Substring(Prefix.Length) : original);

        private static bool? MapInteraction(string original) 
            => original == null ? (bool?) null : !Convert.ToBoolean(original);
    }

}
