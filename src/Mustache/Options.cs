using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mustache
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input hash file(s) to be processed with a mustache semantic template (json or YAML input).")]
        public IEnumerable<string> InputFiles { get; set; }

        [Option('m', "mustache", Required = true, HelpText = "Input Mustache or Handlebars template.")]
        public string InputMustacheFile { get; set; }

        [Option('p', "partials", Required = false, HelpText = "Registering partials template for mustache.")]
        public IEnumerable<string> InputPartialsMustacheFiles { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output file is specified, default is standart output.")]
        public string Output { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        [Usage(ApplicationAlias = "moustache")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Normal scenario", new Options { InputFiles = new string[]{"hash.yaml", "configuration.yaml"}, InputMustacheFile = "template.mustache", InputPartialsMustacheFiles = new string[] { "partial1.mustache" }, Output = "result.html", Verbose = true });
                yield return new Example("Normal scenario", new Options { InputFiles = new string[] { "hash.json" }, InputMustacheFile = "graphviz.mustache", Output = "mydiagram.dot", Verbose = false });
            }
        }
    }
}
