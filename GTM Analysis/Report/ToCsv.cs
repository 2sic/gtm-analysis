using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace ToSic.Om.Gtm.Analysis.Report
{
    public class ToCsv
    {
        public static string CsvTriggers(IEnumerable<Data.Trigger> triggers)
        {
            var data = new TriggerList(triggers);
            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(data.Data);

                return writer.ToString();
            }
        }
    }
}
