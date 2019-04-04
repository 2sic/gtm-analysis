// ReSharper disable InconsistentNaming
namespace ToSic.Om.Gtm.Analysis.JsonSchema
{
    public class Trigger: BaseElement
    {
        public int triggerId;
        public string name;
        public string type;
        public TriggerFilter[] filter;
    }
}
