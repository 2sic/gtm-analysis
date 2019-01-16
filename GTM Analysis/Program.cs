using System;
using System.IO;
using System.Linq;
using CommandLine;
using Newtonsoft.Json;
using ToSic.Om.Gtm.Analysis.Data;
using ToSic.Om.Gtm.Analysis.Report;

namespace ToSic.Om.Gtm.Analysis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptionsAndReturnExitCode)
                //.WithNotParsed<Options>((errs) => HandleParseError(errs))
                ;

            // await keypress before closing the window
            if (System.Diagnostics.Debugger.IsAttached) Console.ReadKey();
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            Console.WriteLine($"File: {opts.File}");

            var json = File.ReadAllText(opts.File);

            var data = JsonConvert.DeserializeObject<JsonSchema.File>(json);

            var triggers = data.containerVersion.trigger;

            var trigs = triggers.Select(t => new Trigger(t)).ToList();

            //var tbl = new TriggerList(trigs);

            //var debug = JsonConvert.SerializeObject(tbl.Data);
            var debug = ToCsv.CsvTriggers(trigs);
            Console.Write("debug:\n" + debug);

            Console.WriteLine("data read into object");
        }
    }
}
