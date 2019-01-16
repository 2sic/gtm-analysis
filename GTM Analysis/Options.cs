using CommandLine;

namespace ToSic.Om.Gtm.Analysis
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose")]
        public bool Verbose { get; set; }

        [Option('i', "in", Required = true, HelpText = "always specify an in-file name")]
        public string File { get; set; }
    }
}
