using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace HappyCspp.Compiler
{
    public class CsProject
    {
        public string Name { get; private set; }

        public string[] ReferencedLibraries { get; private set; }

        public string[] SourceFiles { get; private set; }

        public string DestinationFolder { get; private set; }

        public string DefaultNamespace { get; private set; }

        public bool IsLibrary { get; private set; }

        public CsProject(string projectFile)
        {
            JObject json = JObject.Parse(File.ReadAllText(projectFile));

            string dir = Path.GetDirectoryName(projectFile);

            this.Name = Path.GetFileName(dir);

            // TODO: Load referenced libraries automatically
            this.ReferencedLibraries = this.Name == "corelib"
                ? new string[]{ }
                : new string[]{ Path.Combine(dir, "bin", "Debug", "corelib.dll") };

            this.SourceFiles = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories);

            this.DestinationFolder = Path.Combine(dir, "cpp");

            this.DefaultNamespace = (string)json["cspp"]["defaultNamespace"];

            this.IsLibrary = !(bool)json["buildOptions"]["emitEntryPoint"];
        }
    }
}
