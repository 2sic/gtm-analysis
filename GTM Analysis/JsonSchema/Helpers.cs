﻿using System.Linq;

namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    public static class Helpers
    {
        public static Parameter Find(this Parameter[] list, string key) => list.FirstOrDefault(p => p.key == key);
    }
}
