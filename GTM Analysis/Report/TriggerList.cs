using System.Collections.Generic;
using System.Data;
using System.Linq;
using ToSic.Om.Gtm.Analysis.Data;

namespace ToSic.Om.Gtm.Analysis.Report
{
    public class TriggerList
    {
        //public DataTable Data;
        public List<dynamic> Data;
        public TriggerList(IEnumerable<Trigger> triggers)
        {
            Data = triggers.Select(t => t.ForTable()).ToList();
            //Data = InitialTable();

            //foreach (var trigger in triggers)
            //{
            //    var row = Data.NewRow();
            //    row[ColName] = trigger.Name;
            //    row[ColType] = trigger.Type;
            //    foreach (var filter in trigger.Filters)
            //    {
            //        if (!Data.Columns.Contains(filter.Key))
            //            Data.Columns.Add(filter.Key);
            //        row[filter.Key] = filter.Value;
            //    }
            //}
        }

        //private DataTable InitialTable()
        //{
        //    var tbl = new DataTable();
        //    tbl.Columns.Add(ColName, typeof(string));
        //    tbl.Columns.Add(ColType, typeof(string));
        //    return tbl;
        //}

        //private List<dynamic> DynamicTable()
        //{
        //    var tbl = new List<dynamic>();
            
        //}
    }
}
