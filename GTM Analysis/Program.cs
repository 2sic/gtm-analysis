using System;
using System.IO;
using CommandLine;
using Newtonsoft.Json;
using ToSic.Om.Gtm.Analysis.Report;

namespace ToSic.Om.Gtm.Analysis
{
    internal class Program
    {
        private const bool StopWhenDebugging = false;

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptionsAndReturnExitCode)
                //.WithNotParsed<Options>((errs) => HandleParseError(errs))
                ;

            // await keypress before closing the window
            if (StopWhenDebugging && System.Diagnostics.Debugger.IsAttached) Console.ReadKey();
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            Console.WriteLine($"File: {opts.File}");

            var json = File.ReadAllText(opts.File);

            var data = JsonConvert.DeserializeObject<JsonSchema.File>(json);

            var csv = new ToCsv(opts.Path);
            csv.Create(data);

            Console.WriteLine("data read into object");
        }
    }
}
