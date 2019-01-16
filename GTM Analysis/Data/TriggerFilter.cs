﻿using System.Linq;
using ToSic.Om.Gtm.Analysis.JsonSchema;

namespace ToSic.Om.Gtm.Analysis.Data
{
    public class TriggerFilter
    {
        public string Type;
        public string Key;
        public string Value;

        public TriggerFilter(JsonSchema.TriggerFilter original)
        {
            Type = original.type;
            Key = original.parameter.Find("arg0").GetValue;
            Value = original.parameter.Find("arg1").GetValue;
        }



        public string OperatorCode
        {
            get
            {
             switch (Type.ToLowerInvariant())
            {
                case "contains":
                    return "~";
                default:
                    return "?";
            }
            }
        }
    }
}
