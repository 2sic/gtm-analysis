using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ToSic.Om.Gtm.Analysis.JsonSchema;
// ReSharper disable InconsistentNaming

namespace ToSic.Om.Gtm.Analysis.Data
{
    public class Tag2Csv: IPreparesForCsv
    {
        public int Id;
        public string Name => _original.name;
        public string Type => _original.type;

        public string Folder => _container.ResolveFolder(_original.parentFolderId);

        public int[] Triggers;

        private readonly ContainerVersion _container;
        private readonly Tag _original;

        public readonly bool Flatten;

        public Tag2Csv(Tag original, ContainerVersion container, bool flatten)
        {

            Id = Convert.ToInt32(original.tagId);
            Triggers = original.firingTriggerId.Select(t => Convert.ToInt32(t)).ToArray();

            _container = container;
            _original = original;
            Flatten = flatten;
        }

        public List<dynamic> PrepareForCsv(/*bool flatten = false*/)
        {
            // not flat, just one record per tag
            if (!Flatten)
            {
                var data = BuildCoreCsv(DontShowTriggersInTag);
                AddTrailingProperties(data);
                return new List<dynamic> {data};
            }

            // flatten: many records per tag, resolve the triggers
            var list = Triggers.Select(ftid =>
            {
                var data = BuildCoreCsv(ftid);
                var asDict = (IDictionary<string, object>)data;
                asDict.Add(OutTrigId, ftid);
                //((dynamic) data).TrigId = ftid;
                var foundTrigger = _container.trigger.FirstOrDefault(t => t.triggerId == ftid);
                if (foundTrigger != null)
                {
                    var triggerData = new Trigger2Csv(foundTrigger).PrepareForCsv().First() as IDictionary<string, object>;
                    foreach (var trigD in triggerData)
                        asDict.Add("!" + trigD.Key, trigD.Value);
                }
                AddTrailingProperties(data);
                return (dynamic) data;
            }).ToList();
            return list;
        }

        private const int DontShowTriggersInTag = -1;
        private const string 
            OutId = "Id",
            OutFolder = "Folder",
            OutState = "State",
            OutName = "Name",
            OutType = "Type",
            OutFire = "Fire",
            OutTriggerNumbers = "Trigger(!)",
            OutTrackId = "TrackId",
            OutTracking = "Tracking",
            OutCat = "AnlytCat",
            OutAct = "AnlytAct",
            OutLbl = "AnlytLbl",
            OutVal = "AnlytVal",
            OutInteract = "Interact", 
            OutTrigId = "!TrgId";

        private static readonly string[] Fields =
        {
            OutId, OutFolder, OutState, OutName, OutType, OutFire, 
            OutTrackId, OutTracking, OutCat, OutAct, OutLbl, OutVal, OutInteract
        };

        private static readonly string[] TriggerFields = {"!", OutTriggerNumbers, OutTrigId, "!Name", "!Type", "!{{Click ID}}",
            "!{{Click Classes}}", "!{{Click Text}}", "!{{Click URL}}", "!{{Click Element}}",
            "!{{Page URL}}", "UaFields" };

        public static IEnumerable<string> CsvFields(bool flatten)
        {
            var fields = Fields.Concat(TriggerFields).ToList();
            var removals = CsvFieldsToDrop(flatten);
            if(removals.Count > 0)
                fields.RemoveAll(removals.Contains);
            return fields;
        }

        public static List<string> CsvFieldsToDrop(bool flatten) 
            => flatten ? new List<string> {OutTriggerNumbers} : new List<string>();

        private ExpandoObject BuildCoreCsv(int triggerId)
        {
            dynamic data = new ExpandoObject();
            var dict = data as IDictionary<string, object>;
            dict.Add(OutId, Id);
            dict.Add(OutFolder, Folder);
            dict.Add(OutState, _original.paused ? "pause" : "run");
            dict.Add(OutName, Name);
            dict.Add(OutType, Type);
            dict.Add(OutFire, NiceTagFiring());

            // only add trigger-id list if it's not in another column
            if (triggerId != DontShowTriggersInTag)
            {
                dict.Add(OutTriggerNumbers, triggerId);
                //var trig = triggerId != DontShowTriggersInTag
                //    ? triggerId.ToString()
                //    : string.Join(",", Triggers.Select(t => t.ToString()));
                //dict.Add(OutTrigger, trig);
            }
            PrepUaProperties(dict);
            return data;
        }

        /// <summary>
        /// Append any additional fields, which are typically not relevant but useful to include
        /// </summary>
        /// <param name="original"></param>
        private void AddTrailingProperties(ExpandoObject original)
        {
            if (Type != "ua") return;

            const string fieldsToSet = "fieldsToSet";
            var fields = _original.parameter.Find(fieldsToSet);
            if (fields != null)
                AddToDict(original, "UaFields", fieldsToSet);
        }


        private void PrepUaProperties(IDictionary<string, object> dict)
        {
            if (Type != "ua") return;

            AddToDict(dict, OutTrackId, "trackingId", ResolveVariableIfPossible);
            AddToDict(dict, OutTracking, "trackType", MapTrackType);
            AddToDict(dict, OutCat, "eventCategory", ResolveVariableIfPossible);
            AddToDict(dict, OutAct, "eventAction", ResolveVariableIfPossible);
            AddToDict(dict, OutLbl, "eventLabel", ResolveVariableIfPossible);
            AddToDict(dict, OutVal, "value", ResolveVariableIfPossible);
            AddToDict(dict, OutInteract, "nonInteraction", MapInteraction);
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
                case "ONCE_PER_EVENT": return "1/EVENT";
                case "ONCE_PER_LOAD": return "1/LOAD";
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
