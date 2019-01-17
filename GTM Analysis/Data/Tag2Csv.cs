using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ToSic.Om.Gtm.Analysis.JsonSchema;

namespace ToSic.Om.Gtm.Analysis.Data
{
    public class Tag2Csv: IPreparesForCsv
    {
        public int Id;
        public string Name => _original.name;
        public string Type => _original.type;
        public int[] Triggers;

        private readonly ContainerVersion _container;
        private readonly Tag _original;

        public Tag2Csv(Tag original, ContainerVersion container)
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
                AddTrailingProperties(data);
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
                    var triggerData = new Trigger2Csv(foundTrigger).PrepareForCsv().First() as IDictionary<string, object>;
                    var asDict = (IDictionary<string, object>) data;
                    foreach (var trigD in triggerData)
                        asDict.Add("!" + trigD.Key, trigD.Value);
                }
                AddTrailingProperties(data);
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
            var dict = data as IDictionary<string, object>;
            var trig = triggerId != -1 ? triggerId.ToString() : string.Join(",", Triggers.Select(t => t.ToString()));
            dict.Add("Trigger(!)", trig);
            PrepUaProperties(dict);
            return data;
        }

        /// <summary>
        /// Append any additional fields, which are typically not relevant but usefull to include
        /// </summary>
        /// <param name="original"></param>
        private void AddTrailingProperties(ExpandoObject original)
        {
            if (Type != "ua") return;

            const string FieldsToSet = "fieldsToSet";
            var fields = _original.parameter.Find(FieldsToSet);
            if (fields != null)
            {
                //var fieldList = fields.GetValue;
                AddToDict(original, "UaFields", FieldsToSet);
            }
        }

        private const string OutTrackId = "TrackId";
        private const string OutTracking = "Tracking";
        private const string OutCat = "AnlytCat";
        private const string OutAct = "AnlytAct";
        private const string OutLbl = "AnlytLbl";
        private const string OutVal = "AnlytVal";
        public static readonly string[] Fields = {OutTrackId, OutTracking, OutCat, OutAct, OutLbl, OutVal};

        private void PrepUaProperties(IDictionary<string, object> dict)
        {
            if (Type != "ua") return;

            AddToDict(dict, OutTrackId, "trackingId", ResolveVariableIfPossible);
            AddToDict(dict, OutTracking, "trackType", MapTrackType);
            AddToDict(dict, OutCat, "eventCategory", ResolveVariableIfPossible);
            AddToDict(dict, OutAct, "eventAction", ResolveVariableIfPossible);
            AddToDict(dict, OutLbl, "eventLabel", ResolveVariableIfPossible);
            AddToDict(dict, OutVal, "value", ResolveVariableIfPossible);
            AddToDict(dict, KeyInteractive, "nonInteraction", MapInteraction);
        }

        private string ResolveVariableIfPossible(string key)
        {
            var found = _container.ResolveVariable(key);
            if (found != key) found = "$" + found;
            return found;
        }


        private void AddToDict(IDictionary<string, object> dict, string property, string key)
            => AddToDict<string>(dict, property, key);

        private void AddToDict<T>(IDictionary<string, object> dict, string property, string key, Func<string, T> mapper = null)
        {
            var val = _original.parameter.Find(key)?.GetValue;
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
