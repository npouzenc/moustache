using HandlebarsDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Moustache
{
    internal class Processor
    {
        /// <summary>
        /// Command line options
        /// </summary>
        public Options Options { get; set; }

        /// <summary>
        /// Document parsed from all inputs files
        /// </summary>
        public string Document { get; private set; }

        /// <summary>
        /// Mustache partials
        /// </summary>
        public Dictionary<string, string> Partials { get; private set; }

        /// <summary>
        /// Mustache template
        /// </summary>
        public string Mustache { get; private set; }

        public Processor(Options options)
        {
            Options = options;
            try
            {
                foreach (var file in options.InputFiles)
                {
                    // Removing the initial '---' and ending '...' for YAML documents
                    Document += ReadFile(file).Trim().TrimStart('-').TrimEnd('.');
                }
                
                Mustache = ReadFile(options.InputMustacheFile);
                Partials = ParsePartials();                
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception catched: {0}", ex);
            }
        }

        public string Run()
        {
            var mustache = CompileMustache();
            var extension = Path.GetExtension(Options.InputFiles.First());                    

            switch (extension.ToLower())
            {
                case ".json":
                    return mustache(GetJson(Document));
                case ".yml":
                case ".yaml":
                    return mustache(GetJsonFromYaml(Document));
                default:
                    throw new NotImplementedException("Cannot parse extension file: " + extension);
            }         
        }

        private dynamic GetJson(string data)
        {
            var json = JsonConvert.DeserializeObject<dynamic>(data);
            return json;
        }

        private dynamic GetJsonFromYaml(string data)
        {
            using (var reader = new StringReader(data))
            {
                var deserializer = new Deserializer();
                var yamlObject = deserializer.Deserialize(reader);
                var sb = new SerializerBuilder();
                sb.JsonCompatible();
                sb.DisableAliases();
                var serializer = sb.Build();
                string jsonData = serializer.Serialize(yamlObject);
                return GetJson(jsonData);
            }
        }

        private Func<object, string> CompileMustache()
        {
            foreach (var partial in Partials)
            {
                using (var reader = new StringReader(partial.Value))
                {
                    var template = Handlebars.Compile(reader);
                    Handlebars.RegisterTemplate(partial.Key, template);
                }
            }
            return Handlebars.Compile(Mustache);
        }

        private Dictionary<string, string> ParsePartials()
        {
            if (Options.Verbose)
            {
                Console.WriteLine("Parsing partials mustaches files");
            }
            var mustaches = new Dictionary<string, string>();
            foreach (var mustache in Options.InputPartialsMustacheFiles)
            {
                var templateName = Path.GetFileNameWithoutExtension(mustache);
                mustaches.Add(templateName, ReadFile(mustache));
            }
            return mustaches;
        }

        private string ReadFile(string file)
        {
            if (Options.Verbose)
            {
                Console.WriteLine("Parsing file: " + file); //TODO: NLog
            }
            using (StreamReader sr = new StreamReader(file))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
