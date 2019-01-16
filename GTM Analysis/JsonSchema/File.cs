using System;
using System.Diagnostics.CodeAnalysis;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class File
    {
        public int exportFormatVersion;
        public DateTime exportTime;
        public ContainerVersion containerVersion;
    }
}
