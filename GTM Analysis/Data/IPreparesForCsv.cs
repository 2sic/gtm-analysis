using System.Collections.Generic;

namespace ToSic.Om.Gtm.Analysis.Data
{
    public interface IPreparesForCsv
    {
        List<dynamic> PrepareForCsv(/*bool flatten = false*/);
    }
}
