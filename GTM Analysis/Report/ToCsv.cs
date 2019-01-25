using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using ToSic.Om.Gtm.Analysis.Data;

namespace ToSic.Om.Gtm.Analysis.Report
{
    public class ToCsv
    {
        public string Path;
        public ToCsv(string path)
        {
            Path = path;
            var dInfo = Directory.CreateDirectory(Path);
            Console.WriteLine($"Prepared path {dInfo.FullName}");
        }

        public void Create(JsonSchema.File file)
        {
            // Export triggers
            var trigs = file.containerVersion.trigger.Select(t => new Trigger2Csv(t)).ToList();
            var dynList = trigs.SelectMany(t => t.PrepareForCsv()).ToList();
            var csvTriggers = CreateCsv(dynList);
            File.WriteAllText(Path + "\\triggers.csv", csvTriggers, Encoding.UTF8);

            // Export Tags
            var flattenTable = true;
            var mapped = file.containerVersion.tag.SelectMany(t => new Tag2Csv(t, file.containerVersion, flattenTable).PrepareForCsv()).ToList();
            var knownKeysList = Tag2Csv.CsvFields(flattenTable).ToList();
            mapped = ExpandMissingProperties(mapped, knownKeysList, Tag2Csv.CsvFieldsToDrop(flattenTable));
            var csvTags = CreateCsv(mapped);
            File.WriteAllText(Path + "\\tags.csv", csvTags, Encoding.UTF8);

        }

        public static string CreateCsv(List<dynamic> data)
        {
            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(data);
                return writer.ToString();
            }
        }

        private static List<dynamic> ExpandMissingProperties(List<dynamic> list, List<string> keys, List<string> removals)
        {
            // Build final key list
            if(keys == null) keys = new List<string>();
            foreach (var li in list)
            {
                var dict = li as IDictionary<string, object>;
                foreach (var pair in dict)
                    if (!keys.Contains(pair.Key))
                        keys.Add(pair.Key);
            }

            // remove unwanted
            if (removals != null && removals.Count > 0)
                keys.RemoveAll(removals.Contains);

            //var newList = new List<dynamic>();
            var newList = list.Select(li =>
                {
                    var dict = li as IDictionary<string, object>;
                    var newDyn = new ExpandoObject() as IDictionary<string, object>;
                    keys.ForEach(k => newDyn.Add(k,dict.ContainsKey(k) ? dict[k]: null));
                    return (dynamic) newDyn;
                })
                .ToList();
            return newList;
        }

    }
}
