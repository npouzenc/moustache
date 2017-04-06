using CommandLine;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moustache
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var results = Parser.Default.ParseArguments<Options>(args).WithParsed(options => Run(options));
            }
            catch (Exception exception) // last chance exception  handler
            {
                Console.Error.WriteLine(exception); // TODO: NLog
            }
        }

        private static void Run(Options options)
        {
            var processor = new Processor(options);
            var result = processor.Run();

            if (string.IsNullOrWhiteSpace(options.Output))
            {
                Console.Out.WriteLine(result);
            }
            else
            {
                Encoding utf8WithoutBom = new UTF8Encoding(false);
                using (StreamWriter writer = new StreamWriter(options.Output, false, utf8WithoutBom))
                {
                    writer.WriteLine(result);
                }
            }
        }

    }
}
